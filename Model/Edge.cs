//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;


namespace EPPZ.Geometry.Model
{


	public class Edge : Segment
	{


		private int _index;
		public int index { get { return _index; } } // Readonly
		public Polygon polygon { get { return vertexA.polygon; } } // Readonly

		public Vertex vertexA;
		public Vertex vertexB;


	#region Factory

		public static Edge EdgeAtIndexWithVertices(int index, Vertex vertexA, Vertex vertexB)
		{
			Edge instance = new Edge();
			instance._index = index;
			instance.vertexA = vertexA;
			instance.vertexB = vertexB;
			return instance;
		}

	#endregion


	#region Override segment point accessors (perefencing polygon points directly).

		public override Vector2 a
		{
			get { return polygon.points[vertexA.index]; }
			set { polygon.points[vertexA.index] = value; }
		}
		
		public override Vector2 b
		{
			get { return polygon.points[vertexB.index]; }
			set { polygon.points[vertexB.index] = value; }
		}
		
	#endregion		
		

	#region Accessors
		
		public Edge _previousEdge;
		public virtual Edge previousEdge { get { return _previousEdge; } } // Readonly
		public void SetPreviousEdge(Edge edge) { _previousEdge = edge; } // Explicit setter (injected at creation time)
		
		public Edge _nextEdge;
		public virtual Edge nextEdge  { get { return _nextEdge; } } // Readonly
		public void SetNextEdge(Edge edge) { _nextEdge = edge; } // Explicit setter (injected at creation time)

	#endregion


	#region Polygon features

		public bool ForwardIntersection(out Edge intersectingEdge, out Vector2 intersectionPoint, bool checkEntirePolygonLoop)
		{
			// Default.
			intersectingEdge = null;
			intersectionPoint = Vector2.zero;
			bool intersecting = false;

			// Only if there are edges enough to test.
			if (this.polygon.edges.Length <= 3) return false;

			Edge testEdge = this.nextEdge.nextEdge; // Skip next neighbour
			while(true)
			{
				intersecting = this.IntersectionWithSegment(testEdge, out intersectionPoint);
				if (intersecting)
				{
					intersectingEdge = testEdge;
					break;
				}

				// Step.
				testEdge = testEdge.nextEdge;

				// End conditions.
				bool end;
				if (checkEntirePolygonLoop)
				{
					end = (testEdge == this.previousEdge.previousEdge); // Only up till the previous neighbour
				}
				else
				{
					end = (testEdge == this.polygon.edges[0].previousEdge); // Only up till the end of the polygon loop
				}
				if (end) break;
			}

			return intersecting;
		}

	#endregion


	}
}