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


	public class ExtendedPolygonLineRenderer : PolygonLineRenderer
	{


		public GameObject windingObject;
		public TextMesh areaTextMesh;

		private float _previousArea;
		private Model.Polygon.Winding _previousWindingDirection;
		
		
		void Start()
		{
			// Model reference.
			Source.Polygon polygonSource = GetComponent<Source.Polygon>();
			if (polygonSource != null)
			{ polygon = polygonSource.polygon; }
		}

		void Update()
		{
			if (polygon == null) return; // Only having polygon

			// Layout winding direction object if any.
			bool hasWindingDirectionObject = (windingObject != null);
			bool windingChanged = (polygon.winding != _previousWindingDirection);
			if (hasWindingDirectionObject && windingChanged)
			{
				windingObject.transform.localScale = (polygon.isCW) ? Vector3.one : new Vector3 (1.0f, -1.0f, 1.0f);
				windingObject.transform.rotation = (polygon.isCW) ? Quaternion.identity : Quaternion.Euler( new Vector3 (0.0f, 0.0f, 90.0f) );
			}

			// Layout area text mesh if any.
			bool hasAreaTextMesh = (areaTextMesh != null);
			bool areaChanged = (polygon.area != _previousArea);
			if (hasAreaTextMesh && areaChanged)
			{
				areaTextMesh.text = polygon.area.ToString();
			}

			// Track.
			_previousWindingDirection = polygon.winding;
			_previousArea = polygon.area;
		}
	}
}
#endif