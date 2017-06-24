//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace EPPZ.Geometry.Model
{


	public class Segment
	{


		static public float defaultAccuracy = 1e-6f;

		public enum ContainmentMethod { Default, Precise }
		static public ContainmentMethod defaultContainmentMethod = ContainmentMethod.Default;

		private Vector2 _a;
		public virtual Vector2 a
		{ 
			get { return _a; }
			set { _a = value; }
		}
		private Vector2 _b;
		public virtual Vector2 b
		{ 
			get { return _b; }
			set { _b = value; }
		}

	#region Calculations

		// If `alwaysCalculate` is on, every `normal` and `perpendicular` property access invokes recalculation of values based on actual topology.
		public bool alwaysCalculate = true;

		private Vector2 _normal;
		public Vector2 normal
		{
			get
			{
				if (_normal == Vector2.zero || alwaysCalculate) { CalculateNormal(); } // Lazy calculation or force calculate on every access
				return _normal;
			}

			set
			{ _normal = value; }
		}

		public Vector2 _perpendicular;
		public Vector2 perpendicular
		{
			get
			{
				if (_perpendicular == Vector2.zero || alwaysCalculate) { CalculatePerpendicular(); } // Lazy calculation or force calculate on every access
				return _perpendicular;
			}
			
			set
			{ _perpendicular = value; }
		}

		public void CalculateNormal()
		{
			_normal = this.perpendicular.normalized;
		}
		
		public void CalculatePerpendicular()
		{
			Vector2 translated = (this.b - this.a); // Translate to origin
			_perpendicular = new Vector2( -translated.y, translated.x); // Rotate CCW
		}

	#endregion

		
	#region Factories
		
		public static Segment SegmentWithSource(Source.Segment segmentSource)
		{
			return Segment.SegmentWithPointTransforms(segmentSource.points, segmentSource.coordinates);
		}
		
		public static Segment SegmentWithPointTransforms(Transform[] pointTransforms, Source.Segment.Coordinates coordinates = Source.Segment.Coordinates.World) // Uses Transform.localPosition.xy()
		{
			if (coordinates == Source.Segment.Coordinates.World)
			{ return Segment.SegmentWithPoints(pointTransforms[0].position, pointTransforms[1].position); }

			// Source.Segment.Coordinates.Local
			return Segment.SegmentWithPoints(pointTransforms[0].localPosition, pointTransforms[1].localPosition);
		}

		public static Segment SegmentWithPoints(Vector2 a_, Vector2 b_)
		{
			Segment instance = new Segment();
			instance.a = a_;
			instance.b = b_;
			return instance;
		}

	#endregion


	#region Model updates
		
		public void UpdateWithSource(Source.Segment segmentSource) // Assuming unchanged point count
		{
			UpdateWithTransforms(segmentSource.points, segmentSource.coordinates);
		}
		
		public void UpdateWithTransforms(Transform[] pointTransforms, Source.Segment.Coordinates coordinates = Source.Segment.Coordinates.World) // Assuming unchanged point count
		{
			if (coordinates == Source.Segment.Coordinates.World)
			{
				a = pointTransforms[0].position;
				b = pointTransforms[1].position;
				return;
			}

 			// Source.Segment.Coordinates.Local
			a = pointTransforms[0].localPosition;
			b = pointTransforms[1].localPosition;
		}
		
	#endregion

	
	#region Accessors

		public Rect bounds // Readonly
		{
			get
			{
				Rect bounds_ = new Rect();

				// Set bounds.
				bounds_.xMin = Mathf.Min(a.x, b.x);
				bounds_.xMax = Mathf.Max(a.x, b.x);
				bounds_.yMin = Mathf.Min(a.y, b.y);
				bounds_.yMax = Mathf.Max(a.y, b.y);

				return bounds_;
			}
		}

		public Rect ExpandedBounds(float accuracy)
		{
			float treshold = accuracy / 2.0f;
			Rect bounds_ = this.bounds;
			return Rect.MinMaxRect(
				bounds_.xMin - treshold,
				bounds_.yMin - treshold,
				bounds_.xMax + treshold,
				bounds_.yMax + treshold);
		}

		public bool ContainsPoint(Vector2 point)
		{ return ContainsPoint(point, defaultAccuracy); }

		public bool ContainsPoint(Vector2 point, float accuracy)
		{ return ContainsPoint(point, accuracy, ContainmentMethod.Default); }

		public bool IsPointLeft(Vector2 point)
		{ return Geometry.PointIsLeftOfSegment(point, this.a, this.b);  }

		public bool ContainsPoint(Vector2 point, float accuracy, ContainmentMethod containmentMethod)
		{			
			float treshold = accuracy / 2.0f;

			// Expanded bounds containment test.
			Rect expandedBounds = this.ExpandedBounds(accuracy);
			bool expandedBoundsContainment = expandedBounds.Contains(point);
			if (expandedBoundsContainment == false) return false; // Only if passed

			// Line distance test.
			float distance = this.DistanceToPoint(point);
			bool lineDistanceTest = distance < treshold;
			if (lineDistanceTest == false) return false; // Only if passed

			if (containmentMethod == ContainmentMethod.Precise)
			{
				// Perpendicular segment.
				Vector2 normalizedHalf = (this.b - this.a) / 2.0f;
				float halfLength = normalizedHalf.magnitude;
				Vector2 normalizedHalfPerpendicular = new Vector2( -normalizedHalf.y, normalizedHalf.x );
				Vector2 perpendicular_a = this.a + normalizedHalf;
				Vector2 perpendicular_b = this.a + normalizedHalf + normalizedHalfPerpendicular;

				// Perpendicular line distance test.
				float perpendicularDistance = Geometry.PointDistanceFromLine(point, perpendicular_a, perpendicular_b);
				bool perpendicularDistanceTest = perpendicularDistance < halfLength;

				// Endpoint distance test if previous failed.
				if (perpendicularDistanceTest == false)
				{
					float distanceFromEndPoints = Mathf.Min( Vector2.Distance(this.a, point), Vector2.Distance(this.b, point) );
					bool endpointDistanceTest = distanceFromEndPoints < treshold;
					if (endpointDistanceTest == false) return false; // Only if passed
				}
			}

			// All passed.
			return true;
		}

	#endregion


	#region Geometry features

		/// <summary>
		/// True when the two segments are intersecting. Not true when endpoints
		/// are equal, nor when a point is contained by other segment.
		/// Can say this algorithm has infinite precision.
		/// </summary>
		public bool IsIntersectingWithSegment(Segment segment)
		{
			// No intersecting if bounds don't even overlap (slight optimization).
			bool boundsOverlaps = this.bounds.Overlaps(segment.bounds);
			if (boundsOverlaps == false) return false;

			// Do the Bryce Boe test.
			return Geometry.AreSegmentsIntersecting(this.a, this.b, segment.a, segment.b);
		}

		/// <summary>
		/// Returns intersection when the two segments are intersecting. Not returns anything when endpoints
		/// are equal, nor when a point is contained by other segment.
		/// Can say this algorithm has infinite precision.
		/// </summary>
		public bool IntersectionWithSegment(Segment segment, out Vector2 intersectionPoint)
		{ return IntersectionWithSegmentWithAccuracy(segment, 0.0f, out intersectionPoint); }
		
		public bool IntersectionWithSegmentWithAccuracy(Segment segment, float accuracy, out Vector2 intersectionPoint)
		{ return IntersectionWithSegmentWithAccuracy(segment, accuracy, defaultContainmentMethod, out intersectionPoint); }

		public bool IntersectionWithSegmentWithAccuracy(Segment segment, ContainmentMethod containmentMethod, out Vector2 intersectionPoint)
		{ return IntersectionWithSegmentWithAccuracy(segment, defaultAccuracy, containmentMethod, out intersectionPoint); }

		public bool IntersectionWithSegmentWithAccuracy(Segment segment, float accuracy, ContainmentMethod containmentMethod, out Vector2 intersectionPoint)
		{
			intersectionPoint = Vector2.zero; // Default

			// No intersecting if bounds don't even overlap.
			Rect expandedBounds = this.ExpandedBounds(accuracy);
			Rect otherExpandedBounds = segment.ExpandedBounds(accuracy);
			bool boundsOverlaps = expandedBounds.Overlaps(otherExpandedBounds);
			if (boundsOverlaps == false)
			{
				return false; // No intersection
			}

			if (accuracy > 0.0f) // Only any accuracy is given
			{
				// Look up point containments.
				bool containsA = this.ContainsPoint (segment.a, accuracy, containmentMethod);
				if (containsA)
				{
						intersectionPoint = segment.a;
						return true; // Intersecting
				}

				bool containsB = this.ContainsPoint (segment.b, accuracy, containmentMethod);
				if (containsB)
				{
						intersectionPoint = segment.b;
						return true; // Intersecting
				}

				bool otherContainsA = segment.ContainsPoint (this.a, accuracy, containmentMethod);
				if (otherContainsA)
				{
						intersectionPoint = this.a;
						return true; // Intersecting
				}

				bool otherContainsB = segment.ContainsPoint (this.b, accuracy, containmentMethod);
				if (otherContainsB)
				{
						intersectionPoint = this.b;
						return true; // Intersecting
				}
			}

			// Do the Bryce Boe test.
			bool isIntersecting = Geometry.AreSegmentsIntersecting(this.a, this.b, segment.a, segment.b);
			if (isIntersecting == false)
			{
				return false; // No intersection
			}

			// All fine, intersection point can be determined.
			intersectionPoint = Geometry.IntersectionPointOfLines(this.a, this.b, segment.a, segment.b); // Actually the intersection of lines defined by segments
			return true; // Intersecting
		}

		public float DistanceToPoint(Vector2 point_)
		{
			return Geometry.PointDistanceFromLine(point_, this.a, this.b);
		}

	#endregion


	}
}