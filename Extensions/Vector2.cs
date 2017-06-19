//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using UnityEngine;


namespace EPPZ.Geometry
{


	public static class Vector2_Extensions
	{


		public static Vector2 Rotated(this Vector2 this_, float degrees)
		{
			// Checks.
			float radians = degrees * Mathf.Deg2Rad;
			if (radians == 0.0f) return this_;
			if (radians == (Mathf.PI * 2.0f)) return this_;

			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);

			Vector2 rotated = new Vector2(
				(cos * this_.x) - (sin * this_.y),
				(sin * this_.x) + (cos * this_.y)
				);

			return rotated;
		}

		public static Vector2 RotatedAround(this Vector2  this_, Vector2 around, float degrees)
		{
			// Checks.
			float radians = degrees * Mathf.Deg2Rad;
			if (radians == 0.0f) return this_;
			if (radians == (Mathf.PI * 2.0f)) return this_;

			Vector2 τposition = this_ - around;
			τposition = τposition.Rotated(degrees);
			τposition = around + τposition;

			return τposition;
		}

		public static float AngleTo(this Vector2 this_, Vector3 to)
		{ return this_.AngleTo((Vector2)to); }

		public static float AngleTo(this Vector2 this_, Vector2 to)
		{
			Vector2 direction = to - this_;
			float angle = Mathf.Atan2(direction.y,  direction.x) * Mathf.Rad2Deg;
			if (angle < 0.0f) angle += 360.0f;
			return angle;
		}
	}
}

