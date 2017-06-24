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


	public class SegmentLineRenderer : GeometryLineRenderer
	{


		public Color lineColor;
		public Color boundsColor;
		public bool normals = false;
		
		Segment segment;
		Source.Segment segmentSource;
		
		void Start()
		{
			// Model reference.
			segmentSource = GetComponent<Source.Segment>();
			segment = segmentSource.segment;
		}
		
		protected override void OnDraw()
		{
			if (segmentSource.coordinates == Source.Segment.Coordinates.World)
			{
				DrawRect(segment.bounds, boundsColor);
				DrawSegment(segment, lineColor, normals);
			}
			else
			{
				DrawRectWithTransform(segment.bounds, boundsColor, this.transform);
				DrawSegmentWithTransform(segment, lineColor, this.transform, normals);
			}
		}
	}
}
#endif