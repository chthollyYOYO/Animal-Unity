Shader "Universal Render Pipeline/2D/Sprite-Unlit-InlineMaskProcess"
{
    Properties
    {
        _MaskTex ("Mask Texture", 2D) = "black" {}
        _OccluderMaskTex ("Occluder Mask Texture", 2D) = "black" {}
        _OutlineColor ("Outline Color", Color) = (1, 1, 0, 1)
        _OutlineWidth ("Outline Width", Range(0, 10)) = 2
        _Threshold ("Threshold", Range(0, 1)) = 0.01
        _Light ("Light", Range(1, 5)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            Name "InteractableOutlineComposite"

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D_X(_BlitTexture);
            SAMPLER(sampler_BlitTexture);

            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);
            float4 _MaskTex_TexelSize;

            TEXTURE2D(_OccluderMaskTex);
            SAMPLER(sampler_OccluderMaskTex);

            half4 _OutlineColor;
            half _OutlineWidth;
            half _Threshold;
            half _Light;

            Varyings Vert(Attributes input)
            {
                Varyings output;

                float2 uv = float2(
                    (input.vertexID << 1) & 2,
                    input.vertexID & 2
                );

                output.uv = uv;
                output.positionCS = float4(uv * 2.0 - 1.0, 0.0, 1.0);

#if UNITY_UV_STARTS_AT_TOP
                output.uv.y = 1.0 - output.uv.y;
#endif

                return output;
            }

            half4 Frag(Varyings i) : SV_Target
            {
                half4 sceneCol =
                    SAMPLE_TEXTURE2D_X(
                        _BlitTexture,
                        sampler_BlitTexture,
                        i.uv
                    );

                half center =
                    SAMPLE_TEXTURE2D(
                        _MaskTex,
                        sampler_MaskTex,
                        i.uv
                    ).a;

                float2 texel = _MaskTex_TexelSize.xy * _OutlineWidth;

                half around =
                    SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv + float2( texel.x, 0)).a +
                    SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv + float2(-texel.x, 0)).a +
                    SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv + float2(0,  texel.y)).a +
                    SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv + float2(0, -texel.y)).a +
                    SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv + float2( texel.x,  texel.y)).a +
                    SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv + float2( texel.x, -texel.y)).a +
                    SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv + float2(-texel.x,  texel.y)).a +
                    SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv + float2(-texel.x, -texel.y)).a;

                half expanded = step(_Threshold, around);

                half outline = saturate(expanded - center);

                half occluder =
                    SAMPLE_TEXTURE2D(
                        _OccluderMaskTex,
                        sampler_OccluderMaskTex,
                        i.uv
                    ).a;

                outline *= 1.0 - step(0.01, occluder);

                half4 outlineCol = _OutlineColor;
                outlineCol.rgb *= _Light;

                sceneCol.rgb =
                    lerp(
                        sceneCol.rgb,
                        outlineCol.rgb,
                        outline * outlineCol.a
                    );

                return sceneCol;
            }

            ENDHLSL
        }
    }
}