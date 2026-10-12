Shader "Universal Render Pipeline/2D/Sprite-Unlit-Inline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineWidth ("Outline Width", Range(0, 10)) = 0.05
        _Threshold ("Alpha Threshold", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            Name "SpriteInsideOutline"
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;

            half4 _Color;
            half4 _OutlineColor;
            half _OutlineWidth;
            half _Threshold;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;
                
                // 如果透明度低于阈值，直接返回透明
                if (color.a < _Threshold)
                {
                    return half4(0, 0, 0, 0);
                }
                
                // 获取纹素大小
                float2 texelSize = _MainTex_TexelSize.xy;
                
                // 定义8个方向的偏移
                float2 offsets[8] = {
                    float2(_OutlineWidth, 0) * texelSize,
                    float2(-_OutlineWidth, 0) * texelSize,
                    float2(0, _OutlineWidth) * texelSize,
                    float2(0, -_OutlineWidth) * texelSize,
                    float2(_OutlineWidth, _OutlineWidth) * texelSize,
                    float2(-_OutlineWidth, _OutlineWidth) * texelSize,
                    float2(_OutlineWidth, -_OutlineWidth) * texelSize,
                    float2(-_OutlineWidth, -_OutlineWidth) * texelSize
                };
                
                bool isEdge = false;
                
                // 检查周围像素
                [unroll]
                for (int i = 0; i < 8; i++)
                {
                    float2 sampleUV = IN.uv + offsets[i];
                    sampleUV = saturate(sampleUV);
                    
                    half4 sampleColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, sampleUV);
                    
                    // 如果采样点透明度低于阈值，说明在边缘
                    if (sampleColor.a < _Threshold)
                    {
                        isEdge = true;
                        break;
                    }
                }
                
                // 如果是边缘，使用描边颜色
                if (isEdge)
                {
                    color.rgb = _OutlineColor.rgb;
                }
                
                color.rgb *= color.a;
                return color;
            }
            ENDHLSL
        }
    }
}