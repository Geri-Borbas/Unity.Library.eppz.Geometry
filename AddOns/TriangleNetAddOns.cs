//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EPPZ.Geometry.AddOns
{


	using System.Linq;
	
	using ClipperLib;
	using Path = List<ClipperLib.IntPoint>;
	using Paths = List<List<ClipperLib.IntPoint>>;

	using Model;


	public static class TriangleNetAddOns
	{
		

	#region Polygon

		public static TriangleNet.Geometry.Polygon TriangleNetPolygon(this Polygon this_)
		{
			TriangleNet.Geometry.Polygon polygon = new TriangleNet.Geometry.Polygon();

			int boundary = 1;
			List<TriangleNet.Geometry.Vertex> vertices = new List<TriangleNet.Geometry.Vertex>();
			this_.EnumeratePolygons((Polygon eachPolygon) =>
			{
				// Collect vertices.
				vertices.Clear();
				eachPolygon.EnumeratePoints((Vector2 eachPoint) =>
				{
					vertices.Add(new TriangleNet.Geometry.Vertex(
						(double)eachPoint.x,
						(double)eachPoint.y,
						boundary
					));
				});

				// Add controur.
				polygon.Add(new TriangleNet.Geometry.Contour(vertices.ToArray(), boundary));

				// Track.
				boundary++;
			});

			return polygon;
		}

	#endregion


	#region Voronoi (beta 3)

		public static Rect Bounds(this TriangleNet.Voronoi.Legacy.SimpleVoronoi this_)
		{
			float xmin = float.MaxValue;
			float xmax = 0.0f;
			float ymin = float.MaxValue;
			float ymax = 0.0f;
			foreach (TriangleNet.Voronoi.Legacy.VoronoiRegion region in this_.Regions)
			{
				foreach (TriangleNet.Geometry.Point eachPoint in region.Vertices)
				{
					if (eachPoint.X > xmax) xmax = (float)eachPoint.X;
					if (eachPoint.X < xmin) xmin = (float)eachPoint.X;
					if (eachPoint.Y > ymax) ymax = (float)eachPoint.Y;
					if (eachPoint.Y < ymin) ymin = (float)eachPoint.Y;
				}
			}
			return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
		}

		public static Paths ClipperPathsFromVoronoiRegions(List<TriangleNet.Voronoi.Legacy.VoronoiRegion> voronoiRegions, float scale = 1.0f)
		{
			Paths paths = new Paths();

			foreach (TriangleNet.Voronoi.Legacy.VoronoiRegion eachRegion in voronoiRegions)
			{
				Path eachPath = new Path();
				foreach (TriangleNet.Geometry.Point eachPoint in eachRegion.Vertices)
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


	#region Generic

		public static Vector2 VectorFromPoint(TriangleNet.Geometry.Point point)
		{
			return new Vector2((float)point.X, (float)point.Y);
		}

		public static Vector2[] PointsFromVertices(ICollection<TriangleNet.Geometry.Point> vertices)
		{
			Vector2[] points = new Vector2[vertices.Count];
			int pointIndex = 0;
			foreach (TriangleNet.Geometry.Point eachPoint in vertices)
			{
				points[pointIndex] = VectorFromPoint(eachPoint);
				pointIndex++;
			}
			return points;
		}

	#endregion

	}
}
