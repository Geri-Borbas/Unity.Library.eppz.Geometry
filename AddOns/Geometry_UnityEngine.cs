//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// #define ADDONS_ENABLED
#if ADDONS_ENABLED
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TriangleNet.Algorithm;
using TriangleNet.Geometry;
using TriangleNet.Data;
using TriangleNet.Tools;
using MeshExplorer.Generators;



namespace EPPZ.Geometry
{


	public static class Geometry_UnityEngine
	{


	#region Polygon

		public static UnityEngine.Mesh Mesh(this Polygon this_, string name = "")
		{ return this_.Mesh(Color.white, name); }

		public static UnityEngine.Mesh Mesh(this Polygon this_, Color color, string name = "")
		{
			// Create geometry.
			InputGeometry geometry = this_.InputGeometry();

			// Triangulate.
			TriangleNet.Behavior behavior = new TriangleNet.Behavior();
			behavior.Convex = false;
			behavior.NoHoles = false;
			// behavior.Algorithm = TriangleNet.TriangulationAlgorithm.Incremental; // SweepLine, Dwyer
			behavior.UseBoundaryMarkers = true;

			TriangleNet.Mesh triangulatedMesh = new TriangleNet.Mesh(behavior);
			triangulatedMesh.Triangulate(geometry);

			// Counts.
			int vertexCount = triangulatedMesh.vertices.Count;
			int triangleCount = triangulatedMesh.triangles.Count;

			// Debug.Log("Mesh.vertexCount ("+vertexCount+")"); // NumberOfInputPoints
			// Debug.Log("Mesh.triangleCount ("+triangleCount+")"); // NumberOfInputPoints

			// Mesh store.
			Vector3[] _vertices = new Vector3[vertexCount];
			Vector2[] _uv = new Vector2[vertexCount];
			Vector3[] _normals = new Vector3[vertexCount];
			Color[] _colors = new Color[vertexCount];
			int[] _triangles = new int[triangleCount * 3];

			foreach (KeyValuePair<int, TriangleNet.Data.Vertex> eachEntry in triangulatedMesh.vertices)
			{
				int index = eachEntry.Key;
				TriangleNet.Data.Vertex eachVertex = eachEntry.Value;

				_vertices[index] = new Vector3(
					(float)eachVertex.x,
					(float)eachVertex.y,
					0.0f // As of 2D
				);

				_uv[index] = _vertices[index];
				_normals[index] = Vector3.forward;
				_colors[index] = color;
			}

			int cursor = 0;
			foreach (KeyValuePair<int, TriangleNet.Data.Triangle> eachPair in triangulatedMesh.triangles)
			{
				TriangleNet.Data.Triangle eachTriangle = eachPair.Value;

				Debug.Log(
					"ID: "+eachTriangle.id+
					" id: "+eachTriangle.ID+
					" Region: "+eachTriangle.Region+
					" Area: "+eachTriangle.Area+
					" Boundary: "+eachTriangle.GetVertex(0).Boundary
				);

				_triangles[cursor] = eachTriangle.P2;
				_triangles[cursor + 1] = eachTriangle.P1;
				_triangles[cursor + 2] = eachTriangle.P0;
				cursor += 3;
			}

			// Create / setup mesh.
			Mesh mesh = new Mesh();
			mesh.vertices = _vertices;
			mesh.uv = _uv;
			mesh.normals = _normals;
			mesh.colors = _colors;
			mesh.subMeshCount = 1;
			mesh.SetTriangles(_triangles, 0);
			mesh.name = name;

			return mesh;
		}

	#endregion

	}
}
#endif

