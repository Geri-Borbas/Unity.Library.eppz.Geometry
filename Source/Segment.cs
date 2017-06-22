//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;


namespace EPPZ.Geometry.Source
{


	using Model;


	public class Segment : MonoBehaviour
	{


		[UnityEngine.Serialization.FormerlySerializedAs("pointTransforms")]
		public Transform[] points;

		public enum UpdateMode { Awake, Update, LateUpdate };
		public UpdateMode update = UpdateMode.Awake;	

		public enum Coordinates { World, Local }
		public Coordinates coordinates = Coordinates.World;

		public Model.Segment segment;


		void Awake()
		{
			segment = Model.Segment.SegmentWithSource(this);
		}

		void Update()
		{
			if (update == UpdateMode.Update)
			{ UpdateModel(); }
		}

		void LateUpdate()
		{
			if (update == UpdateMode.LateUpdate)
			{ UpdateModel(); }
		}

		void UpdateModel()
		{
			segment.UpdateWithSource(this);
		}
	}
}
