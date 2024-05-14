Shader "HaihaiShader/CharacterLit" {
    Properties {
        _MainTex ("Albedo", 2D) = "white" {}
        _FaceTex ("Face", 2D) = "white" {}
        _AmbientColor ("Ambient Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (0.5,0.5,0.5,1)
        _SelfShadowRampTex ("Self Shadow Ramp", 2D) = "white" {}
        _ShadowMapRampTex ("Shadow Map Ramp", 2D) = "white" {}
        [Toggle]_CastShadows ("Cast Shadows", Float) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass {
            Name "ForwardLit"

            Tags {
                "LightMode"="UniversalForwardOnly"
            }

            Cull Back
            ZWrite On

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "HaihaiFunctions.hlsl"

            struct Attributes
            {
                float4 posOS : POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float3 normalOS: NORMAL;
                float4 tangentOS: TANGENT;
            };

            struct Varyings
            {
                float4 posHClip : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalWS: TEXCOORD1;
                float4 tangentWS: TEXCOORD2;
                float3 posWS: TEXCOORD3;
                float2 uv1 : TEXCOORD4;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _FaceTex_ST;
                float4 _SelfShadowRampTex_ST;
                float4 _ShadowMapRampTex_ST;
                float4 _AmbientColor;
                float4 _ShadowColor;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_FaceTex);
            SAMPLER(sampler_FaceTex);

            TEXTURE2D(_SelfShadowRampTex);
            SAMPLER(sampler_SelfShadowRampTex);

            TEXTURE2D(_ShadowMapRampTex);
            SAMPLER(sampler_ShadowMapRampTex);

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                OUT.posHClip = TransformObjectToHClip(IN.posOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.tangentWS = float4(normalInput.tangentWS, IN.tangentOS.w);
                OUT.uv0 = TRANSFORM_TEX(IN.uv0.xy, _MainTex);
                OUT.uv1 = TRANSFORM_TEX(IN.uv1.xy, _FaceTex);
                OUT.posWS = TransformObjectToWorld(IN.posOS.xyz).xyz;
                return OUT;
            }

            float4 Frag(Varyings IN) : SV_Target
            {
                float4 result = 0;
                result.a = 1;
                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0);
                float4 face = SAMPLE_TEXTURE2D(_FaceTex, sampler_FaceTex, IN.uv1);
                albedo.rgb = lerp(albedo.rgb, face.rgb, face.a);

                // return albedo;
                float3 normalWS = normalize(IN.normalWS);
                result.rgb = ComputeToonSurface(_AmbientColor.rgb, albedo, _ShadowColor, normalWS, IN.posWS, 1.0, 1.0,
                                                  _SelfShadowRampTex, sampler_SelfShadowRampTex, _ShadowMapRampTex, sampler_ShadowMapRampTex);
                return result;
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/DEPTHONLY"

        UsePass "Universal Render Pipeline/Lit/DEPTHNORMALS"

        UsePass "Universal Render Pipeline/Lit/SHADOWCASTER"
    }
}