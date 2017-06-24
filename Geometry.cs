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


namespace EPPZ.Geometry
{


	using Model; // For `Polygon` region only


	public static class Geometry
	{

		
	#region Point

		// Determine if points are equal with a given accuracy.
		public static bool ArePointsEqualWithAccuracy(Vector2 a, Vector2 b, float accuracy)
		{
			return Vector2.Distance(a, b) <= accuracy;
		}

		// Determine winding direction of three points.
		public static bool ArePointsCCW(Vector2 a, Vector2 b, Vector2 c)
		{
			return ((c.y - a.y) * (b.x - a.x) > (b.y - a.y) * (c.x - a.x));
		}

	#endregion


	#region Rect / Bounds

		// Determine if `rect2.size` fits into `rect1`.
		public static bool IsRectContainsRectSizeWithAccuracy(Rect rect1, Rect rect2) // Compare sizes only 
		{
			return Geometry.IsRectContainsRectSizeWithAccuracy(rect1, rect2, 0.0f);
		}

		// Determine if `rect2.size` fits into `rect1` with a given accuracy.
		public static bool IsRectContainsRectSizeWithAccuracy(Rect rect1, Rect rect2, float accuracy) // Compare sizes only 
		{
			return (
				Mathf.Abs(rect1.width + accuracy * 2.0f) >= Mathf.Abs(rect2.width)) &&
				(Mathf.Abs(rect1.height + accuracy * 2.0f) >= Mathf.Abs(rect2.height)
					);
		}

		// Determine if `rect2` is contained by `rect1` (even if permiters are touching).
		public static bool IsRectContainsRectWithAccuracy(Rect rect1, Rect rect2)
		{ return Geometry.IsRectContainsRectWithAccuracy(rect1, rect2, 0.0f); }

		// Determine if `rect2` is contained by `rect1` (even if permiters are touching) with a given accuracy.
		public static bool IsRectContainsRectWithAccuracy(Rect rect1, Rect rect2, float accuracy)
		{
			bool xMin = (rect1.xMin - accuracy <= rect2.xMin);
			bool xMax = (rect1.xMax + accuracy >= rect2.xMax);
			bool yMin = (rect1.yMin - accuracy <= rect2.yMin);
			bool yMax = (rect1.yMax + accuracy >= rect2.yMax);
			return xMin && xMax && yMin && yMax;
		}

	#endregion


	#region Line

		/// <summary>
		/// Returns intersection point of two lines (defined by segment endpoints).
		/// Returns zero, when segments have common points, or when a segment point lies on other.
		/// </summary>
		public static Vector2 IntersectionPointOfLines(Vector2 segment1_a, Vector2 segment1_b, Vector2 segment2_a, Vector2 segment2_b)
		{
			float crossProduct = (segment1_a.x - segment1_b.x) * (segment2_a.y - segment2_b.y) - (segment1_a.y - segment1_b.y) * (segment2_a.x - segment2_b.x);
			if (crossProduct == 0.0f) return Vector2.zero;

			float x = ((segment2_a.x - segment2_b.x) * (segment1_a.x * segment1_b.y - segment1_a.y * segment1_b.x) - (segment1_a.x - segment1_b.x) * (segment2_a.x * segment2_b.y - segment2_a.y * segment2_b.x)) / crossProduct;
			float y = ((segment2_a.y - segment2_b.y) * (segment1_a.x * segment1_b.y - segment1_a.y * segment1_b.x) - (segment1_a.y - segment1_b.y) * (segment2_a.x * segment2_b.y - segment2_a.y * segment2_b.x)) / crossProduct;

			// Skip segmental tests.
			// if (x < Mathf.Min(a1.x, b1.x) || x > Mathf.Max(a1.x, b1.x)) return Vector2.zero;
			// if (x < Mathf.Min(a2.x, b2.x) || x > Mathf.Max(a2.x, b2.x)) return Vector2.zero;

			return new Vector2(x, y);
		}

		// Determine point distance from line (defined by segment endpoints).
		public static float PointDistanceFromLine(Vector2 point, Vector2 segment_a, Vector2 segment_b)
		{
			float a_ = point.x - segment_a.x;
			float b_ = point.y - segment_a.y;
			float c_ = segment_b.x - segment_a.x;
			float d_ = segment_b.y - segment_a.y;
			return Mathf.Abs(a_ * d_ - c_ * b_) / Mathf.Sqrt(c_ * c_ + d_ * d_);
		}

	#endregion


	#region Segment

		// Determine if a given point lies on the left side of a segment (line beneath).
		public static bool PointIsLeftOfSegment(Vector2 point, Vector2 segment_a, Vector2 segment_b)
		{
			float crossProduct = (segment_b.x - segment_a.x) * (point.y - segment_a.y) - (segment_b.y - segment_a.y) * (point.x - segment_a.x);
			return (crossProduct > 0.0f);
		}
			
		// Determine if segments (defined by endpoints) are equal with a given accuracy.
		public static bool AreSegmentsEqualWithAccuracy(Vector2 segment1_a, Vector2 segment1_b, Vector2 segment2_a, Vector2 segment2_b, float accuracy)
		{
			return (
				(ArePointsEqualWithAccuracy(segment1_a, segment2_a, accuracy) && ArePointsEqualWithAccuracy(segment1_b, segment2_b, accuracy)) ||
				(ArePointsEqualWithAccuracy(segment1_a, segment2_b, accuracy) && ArePointsEqualWithAccuracy(segment1_b, segment2_a, accuracy))
				);
		}
		
		// Determine if segments (defined by endpoints) have common points with a given accuracy.
		public static bool HaveSegmentsCommonPointsWithAccuracy(Vector2 segment1_a, Vector2 segment1_b, Vector2 segment2_a, Vector2 segment2_b, float accuracy)
		{
			return (
				ArePointsEqualWithAccuracy(segment1_a, segment2_a, accuracy) ||
				ArePointsEqualWithAccuracy(segment1_a, segment2_b, accuracy) ||
				ArePointsEqualWithAccuracy(segment1_b, segment2_a, accuracy) ||
				ArePointsEqualWithAccuracy(segment1_b, segment2_b, accuracy)
				);
		}

		/// <summary>
		/// Determine if two segments defined by endpoints are intersecting (defined by points).
		/// True when the two segments are intersecting. Not true when endpoints
		/// are equal, nor when a point is contained by other segment.
		/// From http://bryceboe.com/2006/10/23/line-segment-intersection-algorithm/ 
		/// </summary>
		public static bool AreSegmentsIntersecting(Vector2 segment1_a, Vector2 segment1_b, Vector2 segment2_a, Vector2 segment2_b)
		{
			return (
				(ArePointsCCW(segment1_a, segment2_a, segment2_b) != ArePointsCCW(segment1_b, segment2_a, segment2_b)) &&
				(ArePointsCCW(segment1_a, segment1_b, segment2_a) != ArePointsCCW(segment1_a, segment1_b, segment2_b))
				);
		}

	#endregion 

			
	#region Polygon
			
		/// <summary>
		/// Test if a polygon contains the given point (checks for sub-polygons recursive).
		/// Uses the same Bryce boe algorythm, so considerations are the same.
		/// From https://en.wikipedia.org/wiki/Point_in_polygon#Ray_casting_algorithm
		/// </summary>
		public static bool IsPolygonContainsPoint(Polygon polygon, Vector2 point)
		{
			// Winding ray left point.
			Vector2 left = new Vector2(polygon.bounds.xMin - polygon.bounds.width, point.y);
			
			// Enumerate polygon segments.
			int windingNumber = 0;
			polygon.EnumerateEdgesRecursive((Edge eachEdge) =>
			{
				// Test winding ray against each polygon segment.
				if (AreSegmentsIntersecting(left, point, eachEdge.a, eachEdge.b)) 
				{ windingNumber++; }
			});
			
			bool isOdd = (windingNumber % 2 != 0); // Odd winding number means point falls outside
			return isOdd;
		}

		public static Vector2 CentroidOfPolygons(Polygon[] polygons)
		{
			// From https://en.wikipedia.org/wiki/Centroid#By_geometric_decomposition
			float ΣxA = 0.0f;
			float ΣyA = 0.0f;
			float ΣA = 0.0f;
			foreach (Polygon eachPolygon in polygons)
			{
				// Get centroid.
				Vector2 eachCentroid = eachPolygon.centroid;

				// Sum weighted.
				ΣxA += eachCentroid.x * eachPolygon.area;
				ΣyA += eachCentroid.y * eachPolygon.area;
				ΣA += eachPolygon.area;	
			}

			// "Remove" area.
			float x = ΣxA / ΣA;
			float y = ΣyA / ΣA;

			return new Vector2(x, y);
		}
			
	#endregion


	}
}
