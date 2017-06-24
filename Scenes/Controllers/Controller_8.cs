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
	/// 8. Segment-Segment intersection point
	/// </summary>
	public class Controller_8 : MonoBehaviour
	{


		public Color defaultColor;
		public Color passingColor;
		
		public float accuracy = 0.0f;

		public Source.Segment segmentSourceA;
		public Source.Segment segmentSourceB;
		public SegmentLineRenderer segmentRendererA;
		public SegmentLineRenderer segmentRendererB;

		public GameObject intersectionPointObject;

		private Segment segment_a { get { return segmentSourceA.segment; } }
		private Segment segment_b { get { return segmentSourceB.segment; } }
			

		void Update()
		{ RenderTestResult(SegmentIntersectionTest()); }

		bool SegmentIntersectionTest()
		{
			Vector2 intersectionPoint;
			bool isIntersecting = segment_a.IntersectionWithSegmentWithAccuracy(segment_b, accuracy, out intersectionPoint);
			if (isIntersecting)
			{
				intersectionPointObject.transform.position = new Vector3(
					intersectionPoint.x,
					intersectionPoint.y,
					intersectionPointObject.transform.position.z
					); // Align point
			}

			return isIntersecting;
		}

		void RenderTestResult(bool testResult)
		{
			Color color = (testResult) ? passingColor : defaultColor;

			// Layout color.
			segmentRendererA.lineColor = color;
			segmentRendererB.lineColor = color;

			// Show / hide intersection point.
			intersectionPointObject.GetComponent<Renderer>().enabled = testResult;
		}
	}
}
