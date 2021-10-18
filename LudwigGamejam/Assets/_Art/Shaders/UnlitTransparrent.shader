Shader "Custom/UnlitTransparrent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _MainCol ("Main Color", Color) = (1,1,1,1)
        _Alpha ("Alpha", Range(0.0, 1.0)) = 1
        [HDR] _RimCol ("Rim Color", Color) = (1,1,1,1)
        _RimEffect ("Rim Amount", float) = 0
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
			ZWrite Off
			Cull Off
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
            half4 _MainCol;
            fixed _Alpha;

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
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

				half4 finalCol = col * _MainCol;
				finalCol.a = _Alpha;
                return finalCol;
            }
            ENDCG
        }

		Pass 
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
			Blend One One
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _RimCol;
			fixed _RimEffect;

			struct v2f {
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				half3 posWorld : TEXCOORD2;
			};


			v2f vert(appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal.xyz));
				o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0)));
				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}


			fixed4 frag(v2f i) : COLOR {
				float t = tex2D(_MainTex, i.uv);
				float val = 1 - abs(dot(i.viewDir, i.normal)) * _RimEffect;
				float4 objectOrigin = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));


				return _RimCol * _RimCol.a * val * val * t;
			}

			ENDCG
		}
    }
}
