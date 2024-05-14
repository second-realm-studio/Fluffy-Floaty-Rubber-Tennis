#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

float NDotL(float3 normalWS, half3 lightDirectionWS)
{
    return dot(normalWS, lightDirectionWS);
}

void GetAdditionalLightsInfo(float3 positionWS, float lightColorInfluence, out float shadowMask, out float3 lightColor)
{
    float shadow = 0;
    float3 color = 0;
    for (int lightIndex = 0; lightIndex < GetAdditionalLightsCount(); lightIndex++)
    {
        Light light = GetAdditionalLight(lightIndex, positionWS, 1.0);
        float3 lightC = light.color * clamp(light.distanceAttenuation * lightColorInfluence, 0, 1);
        color += lightC;

        float lightIntensity = 0.299 * light.color.r + 0.587 * light.color.g + 0.114 * light.color.b;
        float shadowAtten = light.shadowAttenuation * light.distanceAttenuation * lightIntensity;
        shadow += shadowAtten;
    }

    shadowMask = clamp(shadow, 0, 1);
    lightColor = color;
}

const float EPSILON = 1e-10;

float3 HUEtoRGB(in float hue)
{
    // Hue [0..1] to RGB [0..1]
    // See http://www.chilliant.com/rgb2hsv.html
    float3 rgb = abs(hue * 6. - float3(3, 2, 4)) * float3(1, -1, -1) + float3(-1, 2, 2);
    return clamp(rgb, 0., 1.);
}

float3 RGBtoHCV(in float3 rgb)
{
    // RGB [0..1] to Hue-Chroma-Value [0..1]
    // Based on work by Sam Hocevar and Emil Persson
    float4 p = (rgb.g < rgb.b) ? float4(rgb.bg, -1., 2. / 3.) : float4(rgb.gb, 0., -1. / 3.);
    float4 q = (rgb.r < p.x) ? float4(p.xyw, rgb.r) : float4(rgb.r, p.yzx);
    float c = q.x - min(q.w, q.y);
    float h = abs((q.w - q.y) / (6. * c + EPSILON) + q.z);
    return float3(h, c, q.x);
}

float3 HSVtoRGB(in float3 hsv)
{
    // Hue-Saturation-Value [0..1] to RGB [0..1]
    float3 rgb = HUEtoRGB(hsv.x);
    return ((rgb - 1.) * hsv.y + 1.) * hsv.z;
}

float3 HSLtoRGB(in float3 hsl)
{
    // Hue-Saturation-Lightness [0..1] to RGB [0..1]
    float3 rgb = HUEtoRGB(hsl.x);
    float c = (1. - abs(2. * hsl.z - 1.)) * hsl.y;
    return (rgb - 0.5) * c + hsl.z;
}

float3 RGBtoHSV(in float3 rgb)
{
    // RGB [0..1] to Hue-Saturation-Value [0..1]
    float3 hcv = RGBtoHCV(rgb);
    float s = hcv.y / (hcv.z + EPSILON);
    return float3(hcv.x, s, hcv.z);
}

float GetLuminance(float r, float g, float b)
{
    return 0.333 * r + 0.333 * g + 0.333 * b;
}

float GetLuminance(float3 color)
{
    return 0.333 * color.r + 0.333 * color.g + 0.333 * color.b;
}

float3 ComputeToonMainLight(float4 shadowColor, float3 normalWS, float3 vertexPosWS, Texture2D colorRampTex, sampler colorRampTexSampler)
{
    //main light color
    float4 shadowCoord = TransformWorldToShadowCoord(vertexPosWS);
    Light mainLight = GetMainLight(shadowCoord);
    float mainLightNdotL = NDotL(normalWS, mainLight.direction);
    mainLightNdotL = mainLightNdotL * 0.5 + 0.5;
    float mainShadowAttenuation = mainLight.shadowAttenuation * mainLightNdotL;
    float4 ramp = SAMPLE_TEXTURE2D(colorRampTex, colorRampTexSampler, float2(mainShadowAttenuation, 0));
    mainShadowAttenuation = ramp.r;
    float3 mainLightColor = mainLight.color * mainShadowAttenuation;
    float3 desatruatedShadowColor = RGBtoHSV(shadowColor.rgb);
    desatruatedShadowColor.y *= ramp.g;//saturation
    desatruatedShadowColor.z *= ramp.b;//value
    desatruatedShadowColor = HSVtoRGB(desatruatedShadowColor);
    mainLightColor = lerp(mainLightColor, desatruatedShadowColor, 1 - mainShadowAttenuation);
    return mainLightColor;
}

float3 ComputeToonAddLights(float3 vertexPosWS, Texture2D addColorRampTex, sampler addColorRampTexSampler)
{
    float3 additionalLightColor = 0;
    for (int lightIndex = 0; lightIndex < GetAdditionalLightsCount(); lightIndex++)
    {
        Light light = GetAdditionalLight(lightIndex, vertexPosWS, 1.0);
        float addShadowAttenuation = light.shadowAttenuation * light.distanceAttenuation * GetLuminance(light.color);
        addShadowAttenuation = SAMPLE_TEXTURE2D(addColorRampTex, addColorRampTexSampler, float2(saturate(addShadowAttenuation), 0)).r;
        additionalLightColor += light.color * addShadowAttenuation;
    }

    return additionalLightColor;
}

float4 ComputeToonSurface(float3 ambient, float4 albedo, float4 shadowColor, float3 normalWS, float3 vertexPosWS, float mainLightInfluence, float additionalLightInfluence,
                          Texture2D colorRampTex,
                          sampler colorRampTexSampler, Texture2D addColorRampTex, sampler addColorRampTexSampler)
{
    float3 mainLightColor = ComputeToonMainLight(shadowColor, normalWS, vertexPosWS, colorRampTex, colorRampTexSampler);
    float3 additionalLightColor = ComputeToonAddLights(vertexPosWS, addColorRampTex, addColorRampTexSampler);

    float3 lightColor = mainLightColor * mainLightInfluence + additionalLightColor * additionalLightInfluence + ambient;
    float4 result = albedo * float4(lightColor, 1);
    return result;
}
