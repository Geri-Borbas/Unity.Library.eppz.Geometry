//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using UnityEngine;
using UnityEditor;


namespace EPPZ.Geometry.Editor
{


	[InitializeOnLoad]
	public class Define
	{


		const string define = "EPPZ_GEOMETRY";


		static Define()
		{ AddDefineIfNeeded(); }

		static void AddDefineIfNeeded()
		{
			BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			if (defines.Contains(define)) return; // Change only if needed
			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (defines + ";" + define));
			Debug.LogWarning("<b>"+define+"</b> added to <i>Scripting Define Symbols</i> for selected build target ("+EditorUserBuildSettings.activeBuildTarget.ToString()+").");
		}
	}
}
