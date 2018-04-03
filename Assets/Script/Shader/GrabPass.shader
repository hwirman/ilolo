// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GrabPass" {
	Properties {
         _RampTex ("Ramp texture", 2D) = "white" {}
         _Color ("Color", Color) = (1,0,0,0)
         _filterR("_filterR", Range(0,1)) = 0
         _Speed ("_Speed", Range(-1,1)) = 0.05
      }
    SubShader
    {
        // Draw ourselves after all opaque geometry
        Tags { "Queue" = "Transparent" }

        // Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }

        // Render the object with the texture generated above, and invert the colors
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _RampTex;
            fixed4 _Color;
            half _Speed;
            half _filterR;
            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_base v) {
                v2f o;
                // use UnityObjectToClipPos from UnityCG.cginc to calculate 
                // the clip-space of the vertex
                o.pos = UnityObjectToClipPos(v.vertex);
                // use ComputeGrabScreenPos function from UnityCG.cginc
                // to get the correct texture coordinate
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            sampler2D _BackgroundTexture;

            half4 frag(v2f i) : SV_Target
            {
                half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);
               // bgcolor = bgcolor*_Color;
               // return 1 - bgcolor;
                //return bgcolor;
               half4 clear = _Color;
               
               float4 result;
              
               result.rgb = tex2D(_RampTex, float2(bgcolor.r+_Speed+0.2, 0.5)).rgb;
               result.a = tex2D(_BackgroundTexture, i.grabPos).a;
  
               result = lerp(result,clear,step(_filterR,result.r));
               return result;
            }
            ENDCG
        }

    }
}