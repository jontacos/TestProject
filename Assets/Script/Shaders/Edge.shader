Shader "Custom/Edge"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_ChromaKeyHueRange("ChromaKeyHueRange", Range(0, 1.0)) = 0
		_ChromaKeySaturationRange("ChromaKeySaturationRange", Range(0, 1.0)) = 0
		_ChromaKeyBrightnessRange("ChromaKeyBrightnessRange", Range(0, 1.0)) = 0
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
			float4 _Color;

//#define R_LUMINANCE 0.3
//#define G_LUMINANCE 0.3
//#define B_LUMINANCE 0.3
#define R_LUMINANCE 0.298912
#define G_LUMINANCE 0.586611
#define B_LUMINANCE 0.114478

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			inline float3 ChromaKeyRGB2HSV(float3 rgb)
			{
				float4 k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp(float4(rgb.bg, k.wz), float4(rgb.gb, k.xy), step(rgb.b, rgb.g));
				float4 q = lerp(float4(p.xyw, rgb.r), float4(rgb.r, p.yzx), step(p.x, rgb.r));
				float d = q.x - min(q.w, q.y);
				float e = 1e-10;
				return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			//float4 _ChromaKeyColor;
			float  _ChromaKeyHueRange;
			float  _ChromaKeySaturationRange;
			float  _ChromaKeyBrightnessRange;
			inline float3 ChromaKeyCalcDiffrence(float4 col)
			{
				//_ChromaKeyColor = float4(1, 1, 1, 1);
				float3 hsv = ChromaKeyRGB2HSV(col);
				float3 key = ChromaKeyRGB2HSV(_Color/*_ChromaKeyColor*/);
				return abs(hsv - key);
			}
			inline float3 ChromaKeyGetRange()
			{
				return float3(_ChromaKeyHueRange, _ChromaKeySaturationRange, _ChromaKeyBrightnessRange);
			}
			inline void ChromaKeyApplyCutout(float4 col)
			{
				float3 d = ChromaKeyCalcDiffrence(col);
				if (!all(step(0.0, ChromaKeyGetRange() - d)))
					discard;
			}
			inline void ChromaKeyApplyAlpha(inout float4 col)
			{
				float3 d = ChromaKeyCalcDiffrence(col);
				if (!all(step(0.0, ChromaKeyGetRange() - d)))
					col = float4(1, 1, 1, 1);//discard;
				col.a *= saturate(length(d / ChromaKeyGetRange()) - 1.0);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				if (col.a < 0.8)
					return fixed4(1,1,1,0);

				//ChromaKeyApplyCutout(col);
				ChromaKeyApplyAlpha(col);
				col.a = 1;


				float alpha = 0.8;
				float v = col.x * R_LUMINANCE + col.y * G_LUMINANCE + col.z * B_LUMINANCE;
				if (col.a < 0.5f)
					v = 1;
				if (v * 3 > 2.2f)
					alpha = 0;

				/*if (3.0 - (col.r + col.g + col.b) > 2.0f) 
				{
					v = 0;
				}
				else
					alpha = 0;*/

				col = float4(v, v, v, alpha);


				return col;
			}

		ENDCG
		}
	}
}
