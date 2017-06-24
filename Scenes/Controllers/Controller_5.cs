//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;


namespace EPPZ.Geometry.Scenes
{


	using Lines;
	using Model;


	/// <summary>
	///  5. Polygon-Polygon containment
	/// </summary>
	public class Controller_5 : MonoBehaviour
	{


		public Color defaultColor;
		public Color passingColor;

		public Source.Polygon starSource;
		public Source.Polygon squareSource;
		public PolygonLineRenderer starRenderer;
		public PolygonLineRenderer squareRenderer;

		private Polygon star { get { return starSource.polygon; } }
		private Polygon square { get { return squareSource.polygon; } }
			

		void Update()
		{ RenderTestResult(IsPolygonInsideTest()); }

		bool IsPolygonInsideTest()
		{
			// Point containment.
			bool pointContainment = true;
			square.EnumeratePointsRecursive((Vector2 eachPoint) =>
			{
				pointContainment &= star.ContainsPoint(eachPoint);
			});

			// Segment-Polygon intersecion, Segment endpoint-permiter contaimnent.
			bool segmentIntersecting = false;
			bool permiterContainsSegment = false;
			foreach (Edge eachEdge in square.edges)
			{
				permiterContainsSegment |= star.PermiterContainsPoint(eachEdge.a) || star.PermiterContainsPoint(eachEdge.b);
				segmentIntersecting |= star.IsIntersectingWithSegment(eachEdge);
			}

			// A polygon contains another polygon, when
			// - other polygon vertices are contained by polygon,
			// - other polygon segments are not intersecting with polygon,
			// - other polygon vertices are not contained by polygon permiter.
			bool polygonInside = (
				pointContainment &&
				segmentIntersecting == false &&
				permiterContainsSegment == false
				);

			return polygonInside;
		}

		void RenderTestResult(bool testResult)
		{
			Color color = (testResult) ? passingColor : defaultColor;

			// Layout colors.
			starRenderer.lineColor = color;
			squareRenderer.lineColor = color;
		}
	}
}