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
	/// 0. Polygon-Point containment
	/// </summary>
	public class Controller_0 : MonoBehaviour
	{


		public Color defaultColor;
		public Color passingColor;

		public Source.Polygon polygonSource;
		public GameObject[] pointObjects;
		public PolygonLineRenderer polygonRenderer;

		Polygon polygon { get { return polygonSource.polygon; } }
			

		void Update()
		{ RenderTestResult(PointContainmentTest()); }

		bool PointContainmentTest()
		{
			bool containsAllPoints = true;
			foreach (GameObject eachPointObject in pointObjects)
			{
				Vector2 eachPoint = eachPointObject.transform.position.xy();
				containsAllPoints &= polygon.ContainsPoint(eachPoint);
			}
			return containsAllPoints;
		}
		
		void RenderTestResult(bool testResult)
		{
			Color color = (testResult) ? passingColor : defaultColor;

			// Layout colors.
			polygonRenderer.lineColor = color;
			foreach (GameObject eachPointObject in pointObjects)
			{ eachPointObject.GetComponent<Renderer>().material.color = color; }
		}
	}
}