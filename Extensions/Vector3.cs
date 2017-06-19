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


	public static class Vector3_Extensions
	{
		

		public static Vector2 xy(this Vector3 this_)
		{
			return new UnityEngine.Vector2( this_.x,  this_.y);
		}
		
		public static Vector2 xz(this Vector3  this_)
		{
			return new UnityEngine.Vector2( this_.x,  this_.z);
		}
		
		public static Vector2 yz(this Vector3  this_)
		{
			return new UnityEngine.Vector2( this_.y,  this_.z);
		}
		
		public static Vector2 yx(this Vector3  this_)
		{
			return new UnityEngine.Vector2( this_.y,  this_.x);
		}
		
		public static Vector2 zx(this Vector3  this_)
		{
			return new UnityEngine.Vector2( this_.z,  this_.x);
		}
		
		public static Vector2 zy(this Vector3  this_)
		{
			return new UnityEngine.Vector2( this_.z,  this_.y);
		}
	}
}

