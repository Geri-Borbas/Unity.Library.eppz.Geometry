//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// #define ADDONS_ENABLED
#if ADDONS_ENABLED
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using ClipperLib;

using TriangleNet.Algorithm;
using TriangleNet.Geometry;
using TriangleNet.Data;
using TriangleNet.Tools;
using MeshExplorer.Generators;



namespace EPPZ.Geometry
{
	

	// Clipper definitions.
	using Path = List<IntPoint>;
	using Paths = List<List<IntPoint>>;


	public static class Geometry_TriangleNet
	{
		

	#region Polygon

		public static InputGeometry InputGeometry(this Polygon this_)
		{
			InputGeometry geometry = new InputGeometry();
			int boundary;

			// Add points.
			boundary = 1;
			int pointIndexOffset = 0;
			this_.EnumeratePolygons((Polygon eachPolygon) =>
			{
				// Add points.
				eachPolygon.EnumeratePoints((Vector2 eachPoint) =>
				{
					geometry.AddPoint(
						(double)eachPoint.x,
						(double)eachPoint.y,
						boundary);
				});

				eachPolygon.EnumerateEdges((EPPZ.Geometry.Edge eachEdge) =>
				{
					int index_a = eachEdge.vertexA.index + pointIndexOffset;
					int index_b = eachEdge.vertexB.index + pointIndexOffset;
					geometry.AddSegment(index_a, index_b, boundary);
				});

				pointIndexOffset += eachPolygon.vertexCount; // Track point offsets.
				boundary++;
			});

			return geometry;
		}

	#endregion


	#region Voronoi

		public static Rect Bounds(this Voronoi this_)
		{
			float xmin = float.MaxValue;
			float xmax = 0.0f;
			float ymin = float.MaxValue;
			float ymax = 0.0f;
			foreach (VoronoiRegion region in this_.Regions)
			{
				foreach (Point eachPoint in region.Vertices)
				{
					if (eachPoint.X > xmax) xmax = (float)eachPoint.X;
					if (eachPoint.X < xmin) xmin = (float)eachPoint.X;
					if (eachPoint.Y > ymax) ymax = (float)eachPoint.Y;
					if (eachPoint.Y < ymin) ymin = (float)eachPoint.Y;
				}
			}
			return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
		}

	#endregion


	#region Generic

		public static Vector2 VectorFromPoint(Point point)
		{
			return new Vector2((float)point.X, (float)point.Y);
		}

		public static Vector2[] PointsFromVertices(ICollection<Point> vertices)
		{
			Vector2[] points = new Vector2[vertices.Count];
			int pointIndex = 0;
			foreach (Point eachPoint in vertices)
			{
				points[pointIndex] = VectorFromPoint(eachPoint);
				pointIndex++;
			}
			return points;
		}

		public static Paths ClipperPathsFromVoronoiRegions(List<VoronoiRegion> voronoiRegions, float scale = 1.0f)
		{
			Paths paths = new Paths();
			foreach (VoronoiRegion eachRegion in voronoiRegions)
			{
				Path eachPath = new Path();
				foreach (Point eachPoint in eachRegion.Vertices)
				{
					eachPath.Add(new IntPoint(
						eachPoint.X * scale,
						eachPoint.Y * scale
					));
				}
				paths.Add(eachPath);
			}
			return paths;
		}

	#endregion

	}
}
#endif
