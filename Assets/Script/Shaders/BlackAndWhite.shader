Shader "Custom/BlackAndWhite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PosX("PosX", float) = 1
		_PosY("PosY", float) = 1
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _PosX;
			float _PosY;
			float4 _Color;

#define R_LUMINANCE 0.5
#define G_LUMINANCE 0.5
#define B_LUMINANCE 0.5

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				//float x = abs(i.uv.x - _PosX);
				//float y = abs(i.uv.y - _PosY);
				//float v = col.x * R_LUMINANCE + col.y * G_LUMINANCE + col.z * B_LUMINANCE;
				//if (v > 0.5)
				//	v = 1.0f;
				//else
				//	v = 0;

				//if (x < 0.0045f && y < 0.008f) // 16:9比率
				//	v = 0.5f;
				//col = float4(v, v, v, 1);

				///*float r = 1.0 - col.r;
				//float g = 1.0 - col.g;
				//float b = 1.0 - col.b;*/
				////int r = col.r; int g = col.g; int b = col.b;
				////col = float4(pow(r,2), pow(g, 2), pow(b, 2), 1);
				////col = float4(r,g,b,1);

				//// apply fog
				////UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
