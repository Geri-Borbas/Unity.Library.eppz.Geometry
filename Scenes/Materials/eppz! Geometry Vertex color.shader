// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "eppz!/Geometry/Vertex color"
{
	
    Properties
    { }
    
	
    SubShader
    {    
    
    
    	Tags {"Queue" = "Transparent"}
        Pass
        {
        
        
        	Cull Off
    		Lighting Off
         	ZWrite Off
         	ZTest Always
         	Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
                    
                    
			CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"


 			struct vertexInput
 			{
            	float4 vertex : POSITION;
            	float4 color : COLOR;
            	float4 texcoord : TEXCOORD0;
        	};

			struct vertexOutput
			{
			    float4 position : SV_POSITION;
				float4 color : COLOR; // Vertex color
				float2 uv : TEXCOORD0;
			};


			vertexOutput vert (vertexInput input)
			{
			    vertexOutput output;
			    
			    // Usual projection stuff.
			    output.position = UnityObjectToClipPos(input.vertex);
 				output.color = input.color;
			    output.uv = input.texcoord;
			    
			    return output;
			}

			half4 frag (vertexOutput input) : COLOR
			{				
				// Color (vertex).
				half4 output = input.color;				
				return output;
			}


            ENDCG
        }
    }
}