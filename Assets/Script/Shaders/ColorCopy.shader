Shader "Custom/ColorCopy"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SourceTex("Texture", 2D) = "white" {}
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
			sampler2D _SourceTex;
			float4 _SourceTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 mainCol = tex2D(_MainTex, i.uv);
				if (mainCol.a < 0.01f)
					discard;

				fixed4 col = tex2D(_SourceTex, i.uv);
				// 一枚も塗り絵を作ってない画像は初期で透過画像が設定されているので
				// alpha見て作ってないのがわかったら元画像表示
				if(col.a < 0.01f)
					col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
