
Shader "Unlit/ScreenCutoutShader" // unlit so no lighting on plane
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
			};

            // finds size of game view
			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.screenPos = ComputeScreenPos(OUT.vertex);
				return OUT;
			}
			
			sampler2D _MainTex;

            // sets texture to be size of game view
			fixed4 frag (v2f IN) : SV_Target
			{
				IN.screenPos /= IN.screenPos.w;
				fixed4 color = tex2D(_MainTex, float2(IN.screenPos.x, IN.screenPos.y));
				
				return color;
			}
			ENDCG
		}
	}
}
