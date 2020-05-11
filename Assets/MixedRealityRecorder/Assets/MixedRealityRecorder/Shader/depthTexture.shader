Shader "MixedRealityRecorder/depthTexture"
{

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

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

			sampler2D _RawDepthTex;
			float4 _RawDepthTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _RawDepthTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				// get depth from depth texture
				float depth = tex2D(_RawDepthTex, i.uv).r;

				return fixed4(depth, depth, depth, 1.0f);
			}

			ENDCG
		}
	}
}