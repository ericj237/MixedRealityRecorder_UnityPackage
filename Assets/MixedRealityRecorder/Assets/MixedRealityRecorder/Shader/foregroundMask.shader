Shader "MixedRealityRecorder/foregroundMask"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            sampler2D _DepthTex;
			float4 _DepthTex_ST;
			float _HmdDepth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _DepthTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

				// get depth from depth texture
				float depth = tex2D(_DepthTex, i.uv).r;

				if (depth > _HmdDepth + 0.0001f)
					return fixed4(1.0f, 1.0f, 1.0f, 1.0f);
				else
					return fixed4(0.0f, 0.0f, 0.0f, 1.0f);
            }
            ENDCG
        }
    }
}
