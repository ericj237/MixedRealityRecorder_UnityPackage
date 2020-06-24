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

            sampler2D _ColorTex;
			float4 _ColorTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _ColorTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

				float alpha = tex2D(_ColorTex, i.uv).a;

				if(alpha > 0.0f)
					return fixed4(alpha, alpha, alpha, 1.0f);
				else
					return fixed4(0.0f, 0.0f, 0.0f, 1.0f);
            }
            ENDCG
        }
    }
}
