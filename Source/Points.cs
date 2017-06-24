using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace EPPZ.Geometry.Source
{


	/// <summary>
	/// Utility component to create point transforms from mesh vertices
	/// suitable to feed data into `Source.Polygon.points` component.
	/// </summary>
	[ExecuteInEditMode]
	public class Points : MonoBehaviour
	{


		public float scale = 0.1f;

		
		[ContextMenu("Create")]
		void Create()
		{
			int index = 1;
			foreach (Vector3 eachVertex in GetComponent<MeshFilter>().mesh.vertices)
			{
				GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				point.transform.parent = transform;
				point.transform.localPosition = eachVertex;
				point.transform.localScale = Vector3.one * scale;
				point.name = "Point "+index.ToString("00");
				index++;
			}
		}
	}
}
