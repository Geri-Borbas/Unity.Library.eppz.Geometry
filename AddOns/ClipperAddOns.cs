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


	// Clipper definitions.
	using Path = List<ClipperLib.IntPoint>;
	using Paths = List<List<ClipperLib.IntPoint>>;


	using Model;


	public static class ClipperAddOns
	{


	#region Polygon

		public static Polygon PolygonFromClipperPaths(Paths paths, float scale)
		{
			Polygon polygon = null;
			for (int index = 0; index < paths.Count; index++)
			{
				Path eachPath = paths[index];
				Polygon eachPolygon = PolygonFromClipperPath(eachPath, scale);

				if (index == 0)
				{ polygon = Polygon.PolygonWithPoints(eachPolygon.points); } // Parent polygon
				else
				{ polygon.AddPolygon(eachPolygon); } // Child polygons
			}
			return polygon;
		}

		public static Polygon PolygonFromClipperPath(Path path, float scale)
		{
			List<Vector2> points = new List<Vector2>();
			for (int index = path.Count - 1; index >= 0; index--) // Reverse enumeration (to flip normals)
			{
				IntPoint eachPoint = path[index];
				points.Add(new Vector2(eachPoint.X / scale, eachPoint.Y / scale));
			}
			return Polygon.PolygonWithPointList(points);
		}

		public static Paths ClipperPaths(this Polygon this_, float scale)
		{
			Paths paths = new Paths();
			this_.EnumeratePolygons((Polygon eachPolygon) =>
			{
				paths.Add(eachPolygon.ClipperPath(scale));
			});
			return paths;    
		}

		public static Path ClipperPath(this Polygon this_, float scale)
		{
			Path path = new Path();
			this_.EnumeratePoints((Vector2 eachPoint) =>
			{
				path.Add(new IntPoint(
					eachPoint.x * scale,
					eachPoint.y * scale
				));
			});
			return path;
		}

	#endregion


	#region Generic

		public static Vector2[] PointsFromClipperPath(Path path, float scale)
		{
			List<Vector2> points = new List<Vector2>();
			for (int index = path.Count - 1; index >= 0; index--) // Reverse enumeration (to flip normals)
			{
				IntPoint eachPoint = path[index];
				points.Add(new Vector2(eachPoint.X / scale, eachPoint.Y / scale));
			}
			return points.ToArray();
		}

	#endregion

	}
}
