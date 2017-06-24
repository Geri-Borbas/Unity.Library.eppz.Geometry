//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#if EPPZ_LINES
using UnityEngine;
using System.Collections;


namespace EPPZ.Geometry.Lines
{


	using EPPZ.Lines;
	using Model;


	public class GeometryLineRenderer : DirectLineRenderer
	{


		protected void DrawSegment(Segment segment, Color color, bool drawNormals = false)
		{
			DrawLine(segment.a, segment.b, color);
			if (drawNormals)
			{
				Vector2 halfie = segment.a + ((segment.b - segment.a) / 2.0f);
				DrawLine(halfie, halfie + segment.normal * 0.1f, color);
			}
		}

		protected void DrawSegmentWithTransform(Segment segment, Color color, Transform transform_, bool drawNormals = false)
		{
			DrawLineWithTransform(segment.a, segment.b, color, transform_);
			if (drawNormals)
			{
				Vector2 halfie = segment.a + ((segment.b - segment.a) / 2.0f);
				DrawLineWithTransform(halfie, halfie + segment.normal * 0.1f, color, transform_);
			}
		}
		
		protected void DrawPolygon(Polygon polygon, Color color)
		{ polygon.EnumerateEdgesRecursive((Edge eachEdge) => DrawLine(eachEdge.a, eachEdge.b, color)); }

		protected void DrawPolygon(Polygon polygon, Color color, bool drawNormals)
		{
			polygon.EnumerateEdgesRecursive((Edge eachEdge) =>
			{
				DrawLine(eachEdge.a, eachEdge.b, color);
				if (drawNormals)
				{
					Vector2 halfie = eachEdge.a + ((eachEdge.b - eachEdge.a) / 2.0f);
					DrawLine(halfie, halfie + eachEdge.normal * 0.1f, color);
				}
			});
		}	

		protected void DrawPolygonWithTransform(Polygon polygon, Color color, Transform transform_)
		{ DrawPolygonWithTransform(polygon, color, transform_, false); }

		protected void DrawPolygonWithTransform(Polygon polygon, Color color, Transform transform_, bool drawNormals)
		{
			polygon.EnumerateEdgesRecursive((Edge eachEdge) =>
			{
				DrawLineWithTransform(eachEdge.a, eachEdge.b, color, transform_);
				if (drawNormals)
				{
					Vector2 halfie = eachEdge.a + ((eachEdge.b - eachEdge.a) / 2.0f);
					DrawLineWithTransform(halfie, halfie + eachEdge.normal * 0.1f, color, transform_);
				}
			});
		}
	}
}
#endif

