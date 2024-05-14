Shader "Hidden/HaihaiShader/DrawDepth" {
    Properties {
        
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

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            // TEXTURE2D(_MainTex);
            // SAMPLER(sampler_MainTex);

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
                float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, IN.screenPos.xy / IN.screenPos.w).r, _ZBufferParams);
                return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, IN.screenPos.xy / IN.screenPos.w).r;
            }
            ENDHLSL
        }
    }
}