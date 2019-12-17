Shader "Warden/Unlit/BeastLeaf"
{
    Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CutOff ("Alpha cutoff", Range(0,1)) = 0.1
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			fixed _CutOff;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				float d = _MainTex_TexelSize.xy * 2;

				half a1 = tex2D(_MainTex, i.uv + d * float2(-1, -1)).a;
				half a2 = tex2D(_MainTex, i.uv + d * float2(0, -1)).a;
				half a3 = tex2D(_MainTex, i.uv + d * float2(+1, -1)).a;

				half a4 = tex2D(_MainTex, i.uv + d * float2(-1, 0)).a;
				half a6 = tex2D(_MainTex, i.uv + d * float2(+1, 0)).a;

				half a7 = tex2D(_MainTex, i.uv + d * float2(-1, +1)).a;
				half a8 = tex2D(_MainTex, i.uv + d * float2(0, +1)).a;
				half a9 = tex2D(_MainTex, i.uv + d * float2(+1, +1)).a;

				float gx = -a1 - a2 * 2 - a3 + a7 + a8 * 2 + a9;
				float gy = -a1 - a4 * 2 - a7 + a3 + a6 * 2 + a9;

				float w = sqrt(gx * gx + gy * gy) / 4;
				half4 source = tex2D(_MainTex, i.uv);
				//fixed4 col = tex2D(_MainTex, i.uv);
				clip(source.a - 0.2);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return half4(lerp(source.rgb, fixed4(0, 0, 0, 1), w), 1);
			}

			/*fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				clip(col.a - _CutOff);
				UNITY_APPLY_FOG(i.fogCoord, col);

				float edgeHeight = 0.015;
				return lerp(
					col,
					fixed4(0, 4, 2, 1),
					step(col.a, _CutOff - edgeHeight)
				);
			}*/


			ENDCG
		}
	}
}
