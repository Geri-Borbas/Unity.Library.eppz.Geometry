//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace EPPZ.Geometry.Model
{


	using ClipperLib;
	using Path = List<ClipperLib.IntPoint>;
	using Paths = List<List<ClipperLib.IntPoint>>;


	public class Polygon
	{


		// Identifiers.
		public int index;
		public string name;

		// Windings.
		public bool isCW { get { return (Mathf.Sign(_area) < 0.0f); } }
		public bool isCCW { get { return (Mathf.Sign(_area) > 0.0f); } }		
		public enum Winding { CCW, CW };		
		public Winding winding { get { return (isCCW) ? Winding.CCW : Winding.CW; } }
		
		/// <remarks>
		/// For internal use.
		/// Edge, Vertex class can read from this directly
		/// Debug renderers can access raw points during development
		/// From the outside, use vertices, edges only, or enumerators
		/// </remarks>
		private Vector2[] _points;
		public Vector2[] points { get { return _points; } } // Readonly

		public Vertex[] vertices; // Vertices of this polygon (excluding sub polygon vertices)
		public Edge[] edges; // Edges of this polygon (excluding sub polygon edges)
		private List<Polygon> polygons = new List<Polygon>(); // Sub-polygons (if any)

		private Rect _bounds = new Rect();
		public Rect bounds { get { return _bounds; } }

		private float _area = 0.0f;
		public float area { get { return _area; } }

		// Yet for single polygons only (see centroid of compound polygons).
		private Vector2 _centroid;
		public Vector2 centroid { get { return _centroid; } }


	#region Factories
		
		public static Polygon PolygonWithPointList(List<Vector2> pointList)
		{ return Polygon.PolygonWithPoints(pointList.ToArray()); }

		public static Polygon PolygonWithSource(Source.Polygon polygonSource)
		{
			Polygon rootPolygon = Polygon.PolygonWithPointTransforms(polygonSource.points, polygonSource.coordinates);

			// Collect sub-polygons if any.
			foreach (Transform eachChildTransform in polygonSource.gameObject.transform)
			{
				GameObject eachChildGameObject = eachChildTransform.gameObject;
                Source.Polygon eachChildPolygonSource = eachChildGameObject.GetComponent<Source.Polygon>();
				if (eachChildPolygonSource != null)
				{
					Polygon eachSubPolygon = Polygon.PolygonWithSource(eachChildPolygonSource);
					rootPolygon.AddPolygon(eachSubPolygon);
				}
			}

			return rootPolygon;
		}

		public Polygon Copy()
		{ return Polygon.PolygonWithPolygon(this); }

		public static Polygon PolygonWithPolygon(Polygon polygon)
		{
			Polygon rootPolygon = null;

			// Compose with sub-olygons (if any).
			polygon.EnumeratePolygons((Polygon eachPolygon) =>
			{
				if (rootPolygon == null)
				{ rootPolygon = Polygon.PolygonWithPoints(eachPolygon.points); }
				else
				{ rootPolygon.AddPolygon(Polygon.PolygonWithPoints(eachPolygon.points)); }
			});

			return rootPolygon;
		}

		public static Polygon PolygonWithPointTransforms(Transform[] pointTransforms, Source.Polygon.Coordinates coordinates)
		{
			// Create points array.
			Vector2[] points = new Vector2[pointTransforms.Length];
			for (int index = 0; index < pointTransforms.Length; index++)
			{
				Transform eachPointTransform = pointTransforms[index];

				if (coordinates == Source.Polygon.Coordinates.World)
				{ points[index] = eachPointTransform.position.xy(); }

				if (coordinates == Source.Polygon.Coordinates.Local)
				{ points[index] = eachPointTransform.localPosition.xy(); }
			}
			
			return Polygon.PolygonWithPoints(points);
		}

		public static Polygon PolygonWithPoints(Vector2[] points)
		{ return PolygonWithPoints(points, 0.0f); }

		public static Polygon PolygonWithPoints(Vector2[] points, float angle)
		{
			Polygon polygon = new Polygon(points.Length);

			// Create points (copy actually).
			for (int index = 0; index < points.Length; index++)
			{
				if (angle == 0.0f)
				{ polygon._points[index] = points[index]; }
				else
				{ polygon._points[index] = points[index].Rotated(angle); }
			}

			// Polygon calculations.
			polygon.Calculate();

			// Create members.
			polygon.CreateVerticesFromPoints();
			polygon.CreateEdgesConnectingPoints();

			return polygon;
		}

		public Polygon(int pointCount = 1)
		{
			this._points = new Vector2[pointCount];
			this.vertices = new Vertex[pointCount];
			this.edges = new Edge[pointCount];
		}

	#endregion


	#region Model updates

		public void UpdatePointPositionsWithSource(Source.Polygon polygonSource) // Assuming unchanged point count
		{
			UpdatePointPositionsWithTransforms(polygonSource.points, polygonSource.coordinates);

			// Update sub-polygons if any.
			foreach (Transform eachChildTransform in polygonSource.gameObject.transform)
			{
				GameObject eachChildGameObject = eachChildTransform.gameObject;
                Source.Polygon eachChildPolygonSource = eachChildGameObject.GetComponent<Source.Polygon>();
				if (eachChildPolygonSource != null)
				{
					eachChildPolygonSource.polygon.UpdatePointPositionsWithTransforms(eachChildPolygonSource.points, eachChildPolygonSource.coordinates);
				}
			}
		}

		public void UpdatePointPositionsWithTransforms(Transform[] pointTransforms, Source.Polygon.Coordinates coordinates) // Assuming unchanged point count
		{
			for (int index = 0; index < pointTransforms.Length; index++)
			{
				Transform eachPointTransform = pointTransforms[index];

				if (coordinates == Source.Polygon.Coordinates.World)
				{ _points[index] = eachPointTransform.position.xy(); }

				if (coordinates == Source.Polygon.Coordinates.Local)
				{ _points[index] = eachPointTransform.localPosition.xy(); }
			}

			// Polygon calculations.
			Calculate();
		}

		public void AddPolygon(Polygon polygon)
		{
			polygons.Add(polygon);

			// Polygon calculations.
			Calculate();
		}

	#endregion


	#region Accessors
		
		public int pointCount { get { return _points.Length; } } // Readonly
		public int vertexCount { get { return vertices.Length; } } // Readonly
		public int edgeCount { get { return edges.Length; } } // Readonly
		public int polygonCount { get { return polygons.Count; } } // Readonly
		public int pointCountRecursive
		{
			get
			{
				int pointCountRecursive = pointCount;
				foreach (Polygon eachPolygon in polygons)
				{
					pointCountRecursive += eachPolygon.pointCount;
				}
				return pointCountRecursive;
			}
		}

	#endregion


	#region Enumerators
		
		public void EnumeratePoints(Action<Vector2> action)
		{
			// Enumerate local points.
			foreach (Vector2 eachPoint in _points)
			{
				action(eachPoint);
			}
		}
		
		public void EnumerateVertices(Action<Vertex> action)
		{
			// Enumerate local points.
			foreach (Vertex eachVertex in vertices)
			{
				action(eachVertex);
			}
		}
		
		public void EnumerateEdges(Action<Edge> action)
		{
			// Enumerate local points.
			foreach (Edge eachEdge in edges)
			{
				action(eachEdge);
			}
		}

		public void EnumeratePointsRecursive(Action<Vector2> action)
		{
			// Enumerate local points.
			foreach (Vector2 eachPoint in _points)
			{
				action(eachPoint);
			}
			
			// Enumerate each sub-polygon points.
			foreach (Polygon eachPolygon in polygons)
			{
				eachPolygon.EnumeratePointsRecursive((Vector2 eachPoint_) =>
				{
					action(eachPoint_);
				});
			}
		}
		
		public void EnumerateVerticesRecursive(Action<Vertex> action)
		{
			// Enumerate local points.
			foreach (Vertex eachVertex in vertices)
			{
				action(eachVertex);
			}
			
			// Enumerate each sub-polygon points.
			foreach (Polygon eachPolygon in polygons)
			{
				eachPolygon.EnumerateVerticesRecursive((Vertex eachVertex_) =>
				{
					action(eachVertex_);
				});
			}
		}
		
		public void EnumerateEdgesRecursive(Action<Edge> action)
		{
			// Enumerate local points.
			foreach (Edge eachEdge in edges)
			{
				action(eachEdge);
			}
			
			// Enumerate each sub-polygon points.
			foreach (Polygon eachPolygon in polygons)
			{
				eachPolygon.EnumerateEdgesRecursive((Edge eachEdge_) =>
				{
					action(eachEdge_);
				});
			}
		}

		public void EnumeratePolygons(Action<Polygon> action)
		{
			action(this); // Including this (a bit unexpected)

			// Enumerate sub-polygons.
			foreach (Polygon eachPolygon in polygons)
			{
				action(eachPolygon);
			}
		}

	#endregion


	#region Polygon calculations

		public void Calculate()
		{
			_CalculateBounds();
			_CalculateArea();
			_CalculateCentroid();
		}

		void _CalculateBounds()
		{
			float left = float.MaxValue; // Out in the right
			float right = float.MinValue; // Out in the left
			float top = float.MinValue; // Out in the bottom
			float bottom = float.MaxValue; // Out in the top
			
			// Enumerate points.
			EnumeratePointsRecursive((Vector2 eachPoint) =>
			{				
				// Track bounds.
				if (eachPoint.x < left) left = eachPoint.x; // Seek leftmost
				if (eachPoint.x > right) right = eachPoint.x; // Seek rightmost
				if (eachPoint.y < bottom) bottom = eachPoint.y; // Seek bottommost
				if (eachPoint.y > top) top = eachPoint.y; // Seek topmost
			});
			
			// Set bounds.
			_bounds.xMin = left;
			_bounds.yMin = bottom;
			_bounds.xMax = right;
			_bounds.yMax = top;
		}

		private void _CalculateArea()
		{

			// From https://en.wikipedia.org/wiki/Shoelace_formula
			Vector2[] points_ = new Vector2[_points.Length + 1];
			System.Array.Copy(_points, points_, _points.Length);
			points_[_points.Length] = _points[0];
			
			// Calculate area.
			float firstProducts = 0.0f;
			float secondProducts = 0.0f;
			for (int index = 0; index < points_.Length - 1; index++)
			{
				Vector2 eachPoint = points_[index];
				Vector2 eachNextPoint = points_[index + 1];
				
				firstProducts += eachPoint.x * eachNextPoint.y;
				secondProducts += eachPoint.y * eachNextPoint.x;
			}
			_area = (firstProducts - secondProducts) / 2.0f;

			// Add / Subtract sub-polygon areas.
			foreach (Polygon eachPolygon in polygons)
			{
				// Outer or inner polygon (supposing there is no self-intersection).
				_area += eachPolygon.area;
			}
		}

		private void _CalculateCentroid()
		{
			// Enumerate points.
			float Σx = 0.0f;
			float Σy = 0.0f;
			EnumeratePoints((Vector2 eachPoint) =>
			{				
				Σx += eachPoint.x;
				Σy += eachPoint.y;
			});

			// Average.
			float x = Σx / pointCount;
			float y = Σy / pointCount;

			// Assign.
			_centroid = new Vector2(x, y);
		}

		private void CreateVerticesFromPoints()
		{
			// Enumerate points (only for index).
			Vertex eachVertex = null;
			Vertex eachPreviousVertex = null;
			for (int index = 0; index < _points.Length; index++)
			{
				eachVertex = Vertex.VertexAtIndexInPolygon(index, this);

				// Inject references.
				if (eachPreviousVertex != null)
				{
					eachPreviousVertex.SetNextVertex(eachVertex);
					eachVertex.SetPreviousVertex(eachPreviousVertex);
				}

				// Collect.
				vertices[index] = eachVertex;

				// Track.
				eachPreviousVertex = eachVertex;
			}

			// Inject last references.
			Vertex firstVertex = vertices[0];
			eachVertex.SetNextVertex(firstVertex);
			firstVertex.SetPreviousVertex(eachVertex);
		}
		
		void CreateEdgesConnectingPoints()
		{
			// Enumerate vertices.
			Edge eachEdge = null;
			Edge eachPreviousEdge = null;
			EnumerateVertices((Vertex eachVertex) =>
			{
				int index = eachVertex.index;
				eachEdge = Edge.EdgeAtIndexWithVertices(index, eachVertex, eachVertex.nextVertex);

				// Inject references.
				if (eachPreviousEdge != null)
				{
					eachPreviousEdge.SetNextEdge(eachEdge);
					eachEdge.SetPreviousEdge(eachPreviousEdge);
				}

				// Collect.
				edges[index] = eachEdge;
				
				// Track.
				eachPreviousEdge = eachEdge;
			});

			// Inject last references.
			Edge firstEdge = edges[0];
			eachEdge.SetNextEdge(firstEdge);
			firstEdge.SetPreviousEdge(eachEdge);

			// Inject vertex edge references.
			EnumerateEdges((Edge eachEdge_) =>
			{
				eachEdge_.vertexA.SetPreviousEdge(eachEdge_.previousEdge);
				eachEdge_.vertexA.SetNextEdge(eachEdge_);
			});
		}

	#endregion


	#region Geometry features

		public bool ContainsPoint(Vector2 point)
		{
			return Geometry.IsPolygonContainsPoint(this, point);
		}

		public bool PermiterContainsPoint(Vector2 point)
		{ return PermiterContainsPoint(point, Segment.defaultAccuracy); }

		public bool PermiterContainsPoint(Vector2 point, float accuracy)
		{ return PermiterContainsPoint(point, accuracy, Segment.ContainmentMethod.Default); }

		public bool PermiterContainsPoint(Vector2 point, float accuracy, Segment.ContainmentMethod containmentMethod)
		{
			bool contains = false;
			EnumerateEdgesRecursive ((Edge eachEdge) =>
			{
				contains |= eachEdge.ContainsPoint(point, accuracy, containmentMethod);
			});

			return contains;
		}

		public bool IsIntersectingWithSegment(Segment segment)
		{
			bool isIntersecting = false;
			EnumerateEdgesRecursive ((Edge eachEdge) =>
			{
				isIntersecting |= segment.IsIntersectingWithSegment(eachEdge);
			});

			return isIntersecting;
		}
		
	#endregion
		

	#region Geometry features (Offset)

		// Clipper precision.
		public static float clipperScale = 10e+5f; 
		public static float clipperArcTolerance = 10e+3f; // 2 magnitude smaller

		// Not simplified, nor rounded.
		public Polygon OffsetPolygon(float offset)
		{ return OffsetPolygon(offset, false, false); }

		// Not simplified.
		public Polygon RoundedOffsetPolygon(float offset)
		{ return OffsetPolygon(offset, false, true); }

		// Full featured.
		public Polygon SimplifiedAndRoundedOffsetPolygon(float offset)
		{ return OffsetPolygon(offset, true, true); }

		Polygon OffsetPolygon(float offset, bool simplify, bool rounded)
		{
			// Calculate Polygon-Clipper scale.
			float maximum = Mathf.Max(bounds.width, bounds.height) + offset * 2.0f + offset;
			float maximumScale = (float)Int32.MaxValue / maximum;
			float scale = Mathf.Min(clipperScale, maximumScale);


			// Convert to Clipper.
			Paths paths = new Paths();
			{
				Path path = new Path();
				EnumeratePoints((Vector2 eachPoint) =>
				                {
					path.Add(new IntPoint(eachPoint.x * scale, eachPoint.y * scale));
				});
				paths.Add(path);
			}
			foreach (Polygon eachPolygon in polygons)
			{
				Path path = new Path();
				eachPolygon.EnumeratePoints((Vector2 eachPoint) =>
				{
					path.Add(new IntPoint(eachPoint.x * scale, eachPoint.y * scale));
				});
				paths.Add(path);
			}

			// Mode.
			JoinType joinType = (rounded) ? JoinType.jtRound : JoinType.jtMiter;

			// Clipper offset.
			Paths offsetPaths = new Paths();
			ClipperOffset clipperOffset = new ClipperOffset();
			if (rounded) { clipperOffset.ArcTolerance = 0.25 * clipperArcTolerance; } // "The default ArcTolerance is 0.25 units." from http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Classes/ClipperOffset/Properties/ArcTolerance.htm
			clipperOffset.AddPaths(paths, joinType, EndType.etClosedPolygon); 
			clipperOffset.Execute(ref offsetPaths, (double)offset * scale);

			// Remove self intersections (if requested).
			if (simplify)
			{ offsetPaths = Clipper.SimplifyPolygons(offsetPaths); }

			// Convert from Clipper.
			Polygon offsetPolygon = null;
			for (int index = 0; index < offsetPaths.Count; index++)
			{
				Path eachSolutionPath = offsetPaths[index];
				Polygon eachSolutionPolygon = PolygonFromClipperPath(eachSolutionPath, scale);

				if (index == 0)
				{
					offsetPolygon = Polygon.PolygonWithPoints(eachSolutionPolygon.points); // Copy
				}
				else
				{
					offsetPolygon.AddPolygon(eachSolutionPolygon);
				}
			}

			// Back to Polygon.
			return offsetPolygon;
		}

		public Polygon UnionPolygon()
		{
			// Calculate Polygon-Clipper scale.
			float maximum = Mathf.Max(bounds.width, bounds.height);
			float maximumScale = (float)Int32.MaxValue / maximum;
			float scale = Mathf.Min(clipperScale, maximumScale);

			// Convert to Clipper.
			Paths subjectPaths = new Paths();
			Paths clipPaths = new Paths();
			{
				Path path = new Path();
				EnumeratePoints((Vector2 eachPoint) =>
				{
					path.Add(new IntPoint(eachPoint.x * scale, eachPoint.y * scale));
				});
				subjectPaths.Add(path);
			}
			foreach (Polygon eachPolygon in polygons)
			{
				Path path = new Path();
				eachPolygon.EnumeratePoints((Vector2 eachPoint) =>
				{
					path.Add(new IntPoint(eachPoint.x * scale, eachPoint.y * scale));
				});
				clipPaths.Add(path);
			}

			// Clipper union.
			Paths unionPaths = new Paths();
			Clipper clipper = new Clipper();
			clipper.AddPaths(subjectPaths, PolyType.ptSubject, true); 
			clipper.AddPaths(clipPaths, PolyType.ptClip, true); 
			clipper.Execute(ClipType.ctUnion, unionPaths);

			// Remove self intersections.
			Paths simplifiedUnionPaths = new Paths();
			simplifiedUnionPaths = Clipper.SimplifyPolygons(unionPaths);

			// Convert from Cipper.
			Polygon simplifiedUnionPolygon = null;
			for (int index = 0; index < simplifiedUnionPaths.Count; index++)
			{
				Path eachSolutionPath = simplifiedUnionPaths[index];
				Polygon eachSolutionPolygon = PolygonFromClipperPath(eachSolutionPath, scale);

				if (index == 0)
				{
					simplifiedUnionPolygon = Polygon.PolygonWithPoints(eachSolutionPolygon.points); // Copy
				}
				else
				{
					simplifiedUnionPolygon.AddPolygon(eachSolutionPolygon);
				}
			}

			// Back to Polygon.
			return simplifiedUnionPolygon;
		}

		private Polygon PolygonFromClipperPath(Path path, float scale)
		{
			List<Vector2> points = new List<Vector2>();
			for (int index = path.Count - 1; index >= 0; index--) // Reverse enumeration (to flip normals)
			{
				IntPoint eachPoint = path[index];
				points.Add(new Vector2(eachPoint.X / scale, eachPoint.Y / scale));
			}
			return Polygon.PolygonWithPointList(points);
		}

	#endregion

		
	#region Geometry features (Transformations)

		public void Reverse()
		{
			// May be a feature later on (reverse `_points` using `System.Array.Copy()`.
		}

		public void AlignCentered()
		{
			Vector2 originalCenter = bounds.center;
			Vector2 offset = -originalCenter;
			Translate(offset);
		}

		public void Translate(Vector2 translation)
		{
			// Apply to each point.
			for (int index = 0; index < _points.Length; index++)
			{
				_points[index] += translation;
			}

			// Apply to each sub-polygon.
			foreach (Polygon eachPolygon in polygons)
			{
				eachPolygon.Translate(translation);
			}

			// Polygon calculations.
			Calculate();

			// Update (bounds).
			// _bounds.position += translation;
		}

		public void Scale(Vector2 scale)
		{
			if (scale == Vector2.one) return; // Only if any

			// Apply to each point.
			for (int index = 0; index < _points.Length; index++)
			{
				_points[index].x *= scale.x;
				_points[index].y *= scale.y;
			}
			
			// Apply to each sub-polygon.
			foreach (Polygon eachPolygon in polygons)
			{
				eachPolygon.Scale(scale);
			}
			
			// Polygon calculations.
			Calculate();
		}

	#endregion


	}
}
