Shader "Universal Render Pipeline/2D/Sprite-Unlit-Mask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Tint",Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipline" = "UniversalPipline"
            "CanUseSpriteAtlas" = "True"
        }
        
        Cull off
        ZWrite off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Mask"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionOS : SV_POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            half4 _Color;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionOS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half a = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).a * IN.color.a;
                return half4(1,1,1,a);
            }

            ENDHLSL

        }
    }
}
