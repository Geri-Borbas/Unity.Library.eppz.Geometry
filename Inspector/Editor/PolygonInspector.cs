//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;
using UnityEditor;


namespace EPPZ.Geometry.Inspector.Editor
{


	using Model;


	[CustomEditor(typeof(EPPZ.Geometry.Inspector.PolygonInspector))]
	public class PolygonInspector : UnityEditor.Editor 
	{


		public override void OnInspectorGUI()
		{
			// References.
			Inspector.PolygonInspector polygonInspector = (Inspector.PolygonInspector)target;
			Polygon polygon = polygonInspector.polygon;
			if (polygon == null) return;
			Edge edge = polygon.edges[polygonInspector.currentEdgeIndex];
			if (edge == null) return;

			if (GUILayout.Button("-"))
			{
				polygonInspector.currentEdgeIndex = edge.previousEdge.index;
				edge = polygon.edges[polygonInspector.currentEdgeIndex];

				ShowUpEdges(edge);
			}

			if (GUILayout.Button("Show up ("+polygonInspector.currentEdgeIndex.ToString()+")"))
			{
				ShowUpEdges(edge);
			}

			if (GUILayout.Button("+"))
			{
				polygonInspector.currentEdgeIndex = edge.nextEdge.index;
				edge = polygon.edges[polygonInspector.currentEdgeIndex];

				ShowUpEdges(edge);
			}
		}

		private void ShowUpEdges(Edge edge)
		{
			Debug.DrawLine(edge.previousEdge.a, edge.previousEdge.b, Color.blue, 1.0f);
			Debug.DrawLine(edge.a, edge.b, Color.red, 1.0f);
			Debug.DrawLine(edge.nextEdge.a, edge.nextEdge.b, Color.green, 1.0f);
		}
	}
}