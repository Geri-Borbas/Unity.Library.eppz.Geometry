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


namespace EPPZ.Geometry.Source
{


	using AddOns;


	public class Mesh : MonoBehaviour
	{

		
		public TriangulatorType triangulator = TriangulatorType.Dwyer;
		public Color color = Color.white;
		
		public enum UpdateMode { Awake, Update, LateUpdate };
		public UpdateMode update = UpdateMode.Awake;

		Source.Polygon polygonSource;
		Model.Polygon polygon;
		MeshFilter meshFilter;


		void Awake()
		{
			polygonSource = GetComponent<Source.Polygon>();
			meshFilter = GetComponent<MeshFilter>();

			if (meshFilter == null)
			{
				Debug.LogWarning("No <b>MeshFilter</b> component on \""+name+"\" (for <b>PolygonMesh</b> to use as output). Disabled <i>GameObject</i>.");
				gameObject.SetActive(false);
			}

			if (polygonSource != null)			
			{ polygon = polygonSource.polygon; }

			if (update == UpdateMode.Awake)
			{ CreateMesh(); }
		}

		void Update()
		{
			if (update == UpdateMode.Update)
			{ CreateMesh(); }
		}

		void LateUpdate()
		{
			if (update == UpdateMode.LateUpdate)
			{ CreateMesh(); }
		}

		void CreateMesh()
		{
			if (polygonSource != null)
			{ polygon = polygonSource.polygon; }

			meshFilter.mesh = polygon.Mesh(color, triangulator);
		}
	}
}
