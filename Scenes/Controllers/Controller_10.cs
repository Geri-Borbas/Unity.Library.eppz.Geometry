//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections.Generic;


namespace EPPZ.Geometry.Scenes
{


	using Model;
	using Lines;


	/// <summary>
	/// 10. Multiple polygon centroid
	/// </summary>
	public class Controller_10 : MonoBehaviour
	{

		public Transform centroid;
		public Source.Polygon[] polygonSources;
		List<Polygon> polygons = new List<Polygon>();


		void Update()
		{
			// Collect polygons.
			polygons.Clear();
			foreach (Source.Polygon eachPolygonSource in polygonSources)
			{ polygons.Add(eachPolygonSource.polygon); }

			// Calculate compund centroid.
			centroid.position = Geometry.CentroidOfPolygons(polygons.ToArray());
		}
	}
}