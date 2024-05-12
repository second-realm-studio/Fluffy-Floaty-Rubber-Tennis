Shader "Jeff/Toon3DLitWithFace" {
    Properties{
        [MainTexture] _MainTex("Albedo Texture", 2D) = "white" {}
        _FaceTex("Face Texture", 2D) = "white" {}
        _EmissiveFaceTex("Emissive Face Texture", 2D) = "black" {} // Emissive face texture
        _EmissiveIntensity("Emissive Intensity", Float) = 1.0 // Intensity for the emissive effect

        _CutoffThreshold("_Cutoff Threshold", Range(0, 1)) = 0.5
        _NormalTex("Normal Texture", 2D) = "bump" {}
        _NormalStrength("Normal Strength", Range(0, 1)) = 1
        _RoughnessTex("Roughness Texture", 2D) = "white" {}
        _ColorRampTex("Color Ramp Texture", 2D) = "white" {}
        _AddLightRampTex("Additional Light Ramp Texture", 2D) = "white" {}
        _MainLightInfluence("Main Light Influence", Range(0, 1)) = 1
        _AdditionalLightsInfluence("Additional Light Influence", Range(0, 5)) = 1
        [Toggle]_CastShadows("Cast Shadows", Float) = 1
        _ShadowColor("Shadow Color", Color) = (0.2, 0.2, 0.2, 0)
        [Toggle]_Silhouette("Enable Silhouette", Float) = 0
        _SilhouetteColor("Silhouette Color", Color) = (0, 0, 0, 1)
        _SilhouetteMax("Silhouette Start Distance", Float) = 6
        _SilhouetteMin("Silhouette Max Distance", Float) = 4
        [Toggle]_AntiOcclusion("Enable Anti-Occlusion", Float) = 0
        _AntiOcclusionMax("Solid Distance", Float) = 6
        _AntiOcclusionMin("Transparent Distance", Float) = 4
    }
        SubShader{
            Tags {
                "RenderType" = "Opaque"
                "Queue" = "Geometry"
                "RenderPipeline" = "UniversalRenderPipeline"
            }

            Pass {
                Name "UniversalForward"
                Tags {
                    "LightMode" = "UniversalForwardOnly"
                }

            // Render State
            Cull Back
            ZWrite On

            HLSLPROGRAM
            #pragma vertex ComputeVertex
            #pragma fragment ComputeFragment

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            #pragma multi_compile_local_fragment _ _SILHOUETTE_ON
            #pragma multi_compile_local_fragment _ _ANTIOCCLUSION_ON

            #include "Assets/XiheRendering/Shader/Toon/ToonFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1: TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float2 uv0 : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float4 tangentWS : TEXCOORD3;
                float2 silhouette : TEXCOORD4; // silhouette factor and dithering factor
                float4 screenPos : TEXCOORD5;
                float2 uv1 : TEXCOORD6;
            };

            // Properties
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _MainTex_TexelSize;
                float4 _NormalTex_TexelSize;
                float4 _RoughnessTex_TexelSize;
                float4 _ColorRampTex_TexelSize;
                float4 _AddLightRampTex_TexelSize;
                float _CutoffThreshold;
                float _NormalStrength;
                float _MainLightInfluence;
                float _AdditionalLightsInfluence;
                float _AdditionalLightsColorMultiplier;
                float4 _ShadowColor;
                float4 _SilhouetteColor;
                float _SilhouetteMin;
                float _SilhouetteMax;
                float _AntiOcclusionMax;
                float _AntiOcclusionMin;
                float _EmissiveIntensity;
            CBUFFER_END

                // Textures and Samplers
                SamplerState sampler_linear_clamp;
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);

                TEXTURE2D(_FaceTex);
                SamplerState sampler_FaceTex;
                float4 _FaceTex_ST;

                TEXTURE2D(_EmissiveFaceTex);
                SamplerState sampler_EmissiveFaceTex;

                TEXTURE2D(_NormalTex);
                SAMPLER(sampler_NormalTex);

                TEXTURE2D(_RoughnessTex);
                SAMPLER(sampler_RoughnessTex);

                TEXTURE2D(_ColorRampTex);
                SAMPLER(sampler_ColorRampTex);

                TEXTURE2D(_AddLightRampTex);
                SAMPLER(sampler_AddLightRampTex);

                TEXTURE2D(_CameraDepthTexture);
                SAMPLER(sampler_CameraDepthTexture);

                // 4x4 Dithering pattern
                static const float4x4 ditherPattern = float4x4(
                    0.0625, 0.5625, 0.1875, 0.6875,
                    0.8125, 0.3125, 0.9375, 0.4375,
                    0.25, 0.75, 0.125, 0.625,
                    0.875, 0.375, 0.0, 0.5
                );

                float Dithering(float2 screenPos)
                {
                    // Find position in dither pattern based on screen position
                    int2 pos = int2(fmod(screenPos.x, 4), fmod(screenPos.y, 4));
                    return ditherPattern[pos.x][pos.y];
                }

                Varyings ComputeVertex(Attributes IN)
                {
                    Varyings OUT;
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                    OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                    OUT.uv0 = TRANSFORM_TEX(IN.uv0.xy, _MainTex);
                    OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                    VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
                    OUT.tangentWS = float4(normalInputs.tangentWS, IN.tangentOS.w);
                    float3 originWS = TransformObjectToWorld(float3(0, 0, 0));
                    originWS.x = _WorldSpaceCameraPos.x;
                    originWS.y = _WorldSpaceCameraPos.y;
                    float camDistance = originWS.z - _WorldSpaceCameraPos.z;
                    float silhouetteFactor = (clamp(camDistance, _SilhouetteMin, _SilhouetteMax) - _SilhouetteMin) / (_SilhouetteMax - _SilhouetteMin);
                    float ditheringFactor = (clamp(camDistance, _AntiOcclusionMin, _AntiOcclusionMax) - _AntiOcclusionMin) / (_AntiOcclusionMax - _AntiOcclusionMin);
                    OUT.silhouette = float2(silhouetteFactor, ditheringFactor);
                    OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                    OUT.uv1 = IN.uv1.xy;
                    return OUT;
                }

                float4 ComputeFragment(Varyings IN) : SV_Target
                {
                    float4 mainColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0);
                    float4 faceColor = SAMPLE_TEXTURE2D(_FaceTex, sampler_FaceTex, IN.uv1);
                    float4 emissiveFaceColor = SAMPLE_TEXTURE2D(_EmissiveFaceTex, sampler_EmissiveFaceTex, IN.uv1);
                    float4 finalColor = lerp(mainColor, faceColor, faceColor.a);

                    // Apply emissive effect
                    float3 emissiveContribution = emissiveFaceColor.rgb * _EmissiveIntensity;
                    finalColor.rgb += emissiveContribution;

                    clip(finalColor.a - _CutoffThreshold);
                    float3 normalWS = normalize(IN.normalWS);
                    float3 viewDirectionWS = normalize(_WorldSpaceCameraPos - IN.positionWS);
                    float3 reflectedViewWS = reflect(-viewDirectionWS, normalWS);
                    Light light = GetMainLight();
                    float3 lightDirectionWS = normalize(light.direction);
                    float3 surfaceNormalWS = normalize(normalWS);
                    return finalColor;
                }
                ENDHLSL
            }
        }
}