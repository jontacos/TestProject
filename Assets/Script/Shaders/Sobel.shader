﻿Shader "Custom/Sobel"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_OutlineThick("Outline Thick", float) = 1.0
		_OutlineThreshold("Outline Threshold", float) = 0.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

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
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _MainTex_TexelSize;
				float _OutlineThick;
				float _OutlineThreshold;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

#define R_LUMINANCE 0.298912
#define G_LUMINANCE 0.586611
#define B_LUMINANCE 0.114478
				fixed4 frag(v2f i) : SV_Target
				{
					float4 col = tex2D(_MainTex, i.uv);

					// 近隣のテクスチャ色をサンプリング
					float diffU = _MainTex_TexelSize.x * _OutlineThick;
					float diffV = _MainTex_TexelSize.y * _OutlineThick;
					half3 col00 = tex2D(_MainTex, i.uv + half2(-diffU, -diffV));
					half3 col01 = tex2D(_MainTex, i.uv + half2(-diffU, 0.0));
					half3 col02 = tex2D(_MainTex, i.uv + half2(-diffU, diffV));
					half3 col10 = tex2D(_MainTex, i.uv + half2(0.0, -diffV));
					half3 col12 = tex2D(_MainTex, i.uv + half2(0.0, diffV));
					half3 col20 = tex2D(_MainTex, i.uv + half2(diffU, -diffV));
					half3 col21 = tex2D(_MainTex, i.uv + half2(diffU, 0.0));
					half3 col22 = tex2D(_MainTex, i.uv + half2(diffU, diffV));


					// 水平方向のコンボリューション行列適用後の色を求める
					half3 horizontalColor = 0;
					horizontalColor += col00 * -1.0;
					horizontalColor += col01 * -1.0;
					horizontalColor += col02 * -1.0;
					horizontalColor += col20;
					horizontalColor += col21;
					horizontalColor += col22;

					// 垂直方向のコンボリューション行列適用後の色を求める
					half3 verticalColor = 0;
					verticalColor += col00;
					verticalColor += col10;
					verticalColor += col20;
					verticalColor += col02 * -1.0;
					verticalColor += col12 * -1.0;
					verticalColor += col22 * -1.0;

					// この値が大きく正の方向を表す部分がアウトライン
					// ※1
					half3 outlineValue = horizontalColor * horizontalColor + verticalColor * verticalColor;

					if (length(half4(outlineValue - _OutlineThreshold, 1)) > 1.1)
						return fixed4(0, 0, 0, 1);
					else
						discard;//return fixed4(1, 1, 1, 1);

					return col;// half4(outlineValue - _OutlineThreshold, 1);

					////// 近隣のテクスチャ色をサンプリング
					////half diffU = _MainTex_TexelSize.x * _OutlineThick;
					////half diffV = _MainTex_TexelSize.y * _OutlineThick;
					////half3 col00 = tex2D(_MainTex, i.uv + half2(-diffU, -diffV));
					////half3 col10 = tex2D(_MainTex, i.uv + half2(diffU, -diffV));
					////half3 col01 = tex2D(_MainTex, i.uv + half2(-diffU, diffV));
					////half3 col11 = tex2D(_MainTex, i.uv + half2(diffU, diffV));
					////half3 diff00_11 = col00 - col11;
					////half3 diff10_01 = col10 - col01;

					////// 対角線の色同士を比較して差分が多い部分をアウトラインとみなす
					////half3 outlineValue = diff00_11 * diff00_11 + diff10_01 * diff10_01;
				
					////if (length(half4(outlineValue - _OutlineThreshold, 1)) > 1.1)
					////	return fixed4(1, 1, 1, 1);
					////else
					////	return fixed4(0, 0, 0, 1);//discard;
					////return half4(outlineValue - _OutlineThreshold, 1);

				//	// 近隣のテクスチャ色をサンプリング
				//float diffU = _MainTex_TexelSize.x * _OutlineThick;
				//float diffV = _MainTex_TexelSize.y * _OutlineThick;
				//half3 col00 = tex2D(_MainTex, i.uv + half2(-diffU, -diffV));
				//half3 col01 = tex2D(_MainTex, i.uv + half2(-diffU, 0.0));
				//half3 col02 = tex2D(_MainTex, i.uv + half2(-diffU, diffV));
				//half3 col10 = tex2D(_MainTex, i.uv + half2(0.0, -diffV));
				//half3 col12 = tex2D(_MainTex, i.uv + half2(0.0, diffV));
				//half3 col20 = tex2D(_MainTex, i.uv + half2(diffU, -diffV));
				//half3 col21 = tex2D(_MainTex, i.uv + half2(diffU, 0.0));
				//half3 col22 = tex2D(_MainTex, i.uv + half2(diffU, diffV));

				//// 水平方向のコンボリューション行列適用後の色を求める
				//half3 horizontalColor = 0;
				//horizontalColor += col00 * -1.0;
				//horizontalColor += col01 * -1.0;
				//horizontalColor += col02 * -1.0;
				//horizontalColor += col20;
				//horizontalColor += col21;
				//horizontalColor += col22;

				//// 垂直方向のコンボリューション行列適用後の色を求める
				//half3 verticalColor = 0;
				//verticalColor += col00;
				//verticalColor += col10;
				//verticalColor += col20;
				//verticalColor += col02 * -1.0;
				//verticalColor += col12 * -1.0;
				//verticalColor += col22 * -1.0;

				//// この値が大きく正の方向を表す部分がアウトライン
				//half3 outlineValue = horizontalColor * horizontalColor + verticalColor * verticalColor;
				//if (length(half4(outlineValue - _OutlineThreshold, 1)) <= 1.1)
				//	return fixed4(1, 1, 1, 1);
				//else
				//	return fixed4(0, 0, 0, 1);

				//return half4(outlineValue - _OutlineThreshold, 1);
				}
				ENDCG
			}
		}
}