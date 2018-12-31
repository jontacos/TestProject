Shader "Custom/Edge"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_PosX("PosX", float) = 1
		_PosY("PosY", float) = 1
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float alpha = 1;
				float x = abs(i.uv.x - _PosX);
				float y = abs(i.uv.y - _PosY);
				float v = col.x * R_LUMINANCE + col.y * G_LUMINANCE + col.z * B_LUMINANCE;
				if (v * 3 > 2.0f)
					alpha = 0;
				col = float4(v, v, v, alpha);
				return col;
			}
		ENDCG
		}
	}
}
