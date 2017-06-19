Shader "eppz!/Geometry/Solid color"
{
	Properties
	{
    	_Color ("Color", Color) = (1,1,1)
	}
 
	SubShader
	{
	    Color [_Color]
	    Pass {}
	} 
}