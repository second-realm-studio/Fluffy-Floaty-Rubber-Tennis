Shader "HaihaiShader/FireHeadParticle" {
    Properties {
        //        _MainTex ("Mask", 2D) = "white" {}
        _BottomColor ("Bottom Color", Color) = (1, 1, 1, 1)
        _TipColor ("Tip Color", Color) = (1, 1, 1, 1)
        _BottomY ("Bottom Y", Float) = .3
        _TipY ("Tip Y", Float) = .45
        [Toggle] _EnableContact("Enable Contact", Float) = 0
        _DepthPower ("Contact Softness", Float) = 0.5
    }
    SubShader {
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalRenderPipeline"
        }

        Pass {
            Name "Diffuse"

            Tags {
                "LightMode" = "UniversalForward"
            }

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma shader_feature_local_fragment _ _ENABLECONTACT_ON

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "HaihaiFunctions.hlsl"

            struct Attributes
            {
                float4 posOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 posHClip : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 posWS : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                // float4 _MainTex_ST;
                float4 _BottomColor;
                float4 _TipColor;
                float _BottomY;
                float _TipY;
                float _DepthPower;
            CBUFFER_END

            // TEXTURE2D(_MainTex);
            // SAMPLER(sampler_MainTex);

            TEXTURE2D(_SelfShadowRampTex);
            SAMPLER(sampler_SelfShadowRampTex);

            TEXTURE2D(_ShadowMapRampTex);
            SAMPLER(sampler_ShadowMapRampTex);

            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                OUT.posHClip = TransformObjectToHClip(IN.posOS.xyz);
                OUT.uv = IN.uv;
                OUT.posWS = TransformObjectToWorld(IN.posOS.xyz);
                OUT.screenPos = ComputeScreenPos(OUT.posHClip);
                return OUT;
            }

            float4 Frag(Varyings IN) : SV_Target
            {
                float4 result = 1;
                
                #ifdef _ENABLECONTACT_ON
                float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, IN.screenPos.xy / IN.screenPos.w).r, _ZBufferParams);
                float contactMask = depth - IN.screenPos.w;
                // return float4(contactMask, contactMask, contactMask, 1);
                result.a = saturate(pow(contactMask, _DepthPower));
                #endif

                float t = (IN.posWS.y - _BottomY) / (_TipY - _BottomY);
                t = saturate(t);
                result.rgb = lerp(_BottomColor.rgb, _TipColor.rgb, t);
                return result;
            }
            ENDHLSL
        }
    }
}