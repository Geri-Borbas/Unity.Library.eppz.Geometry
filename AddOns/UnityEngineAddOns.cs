//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EPPZ.Geometry.AddOns
{


	using TriangleNet.Geometry;
	using TriangleNet.Meshing;
	using TriangleNet.Meshing.Algorithm;
	using TriangleNet.Tools;


	public enum TriangulatorType { Incremental, Dwyer, SweepLine };


	public static class UnityEngineAddOns
	{


		static ITriangulator TriangulatorForType(TriangulatorType triangulator)
		{
			switch (triangulator)
			{
				case TriangulatorType.Incremental : return new Incremental();
				case TriangulatorType.Dwyer : return new Dwyer();
				case TriangulatorType.SweepLine : return new SweepLine();
				default : return new Dwyer();				
			}
		}


	#region Polygon

		public static UnityEngine.Mesh Mesh(this EPPZ.Geometry.Model.Polygon this_, string name = "")
		{ return this_.Mesh(Color.white, TriangulatorType.Dwyer, name); }

		public static UnityEngine.Mesh Mesh(this EPPZ.Geometry.Model.Polygon this_, TriangulatorType triangulator, string name = "")
		{ return this_.Mesh(Color.white, triangulator, name); }

		public static UnityEngine.Mesh Mesh(this EPPZ.Geometry.Model.Polygon this_, Color color, TriangulatorType triangulator, string name = "")
		{
			// Create geometry.
			TriangleNet.Geometry.Polygon polygon = this_.TriangleNetPolygon();

			// Triangulate.
			ConstraintOptions options = new ConstraintOptions();
			// ConformingDelaunay
			// Convex
			// SegmentSplitting
    		QualityOptions quality = new QualityOptions();
			// MinimumAngle
			// MaximumArea
			// UserTest
			// VariableArea
			// SteinerPoints
			IMesh triangulatedMesh = polygon.Triangulate(options, quality, TriangulatorForType(triangulator));

			// Counts.
			int vertexCount = triangulatedMesh.Vertices.Count;
			int triangleCount = triangulatedMesh.Triangles.Count;

			// Mesh store.
			Vector3[] _vertices = new Vector3[vertexCount];
			Vector2[] _uv = new Vector2[vertexCount];
			Vector3[] _normals = new Vector3[vertexCount];
			Color[] _colors = new Color[vertexCount];
			int[] _triangles = new int[triangleCount * 3];

			int index = 0;
			foreach (TriangleNet.Geometry.Vertex eachVertex in triangulatedMesh.Vertices)
			{

				_vertices[index] = new Vector3(
					(float)eachVertex.x,
					(float)eachVertex.y,
					0.0f // As of 2D
				);

				_uv[index] = _vertices[index];
				_normals[index] = Vector3.forward;
				_colors[index] = color;

				index++;
			}

			int cursor = 0;
			foreach (TriangleNet.Topology.Triangle eachTriangle in triangulatedMesh.Triangles)
			{
				Debug.Log(
					"ID: "+eachTriangle.id+
					" id: "+eachTriangle.ID+
					// " Region: "+eachTriangle.Region+
					" Area: "+eachTriangle.Area
					// " Boundary: "+eachTriangle.GetVertex(0).Boundary
				);

				_triangles[cursor] = eachTriangle.GetVertexID(2); // P2
				_triangles[cursor + 1] = eachTriangle.GetVertexID(1); // P1
				_triangles[cursor + 2] = eachTriangle.GetVertexID(0); // P0
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

