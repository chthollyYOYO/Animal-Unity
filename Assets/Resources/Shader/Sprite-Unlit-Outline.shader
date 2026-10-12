Shader "Universal Render Pipeline/2D/Sprite-Unlit-Outline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineWidth ("Outline Width", Range(0, 10)) = 1
        _AlphaThreshold ("Alpha Threshold", Range(0, 1)) = 0.01
        _Light ("Light", Range(1, 5)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "SpriteOutsideOutline"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;

            half4 _Color;
            half4 _OutlineColor;
            half _OutlineWidth;
            half _AlphaThreshold;
            half _Light;

            v2f vert(appdata IN)
            {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos(IN.vertex);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                half4 col = tex2D(_MainTex, IN.uv) * IN.color;

                half pointAlpha = col.a;

                float2 texelSize = _MainTex_TexelSize.xy;

                float2 offsetX = float2(_OutlineWidth, 0) * texelSize;
                float2 offsetY = float2(0, _OutlineWidth) * texelSize;

                half aroundAlpha =
                    tex2D(_MainTex, IN.uv + offsetY).a +
                    tex2D(_MainTex, IN.uv - offsetY).a +
                    tex2D(_MainTex, IN.uv - offsetX).a +
                    tex2D(_MainTex, IN.uv + offsetX).a +
                    tex2D(_MainTex, IN.uv + offsetX + offsetY).a +
                    tex2D(_MainTex, IN.uv + offsetX - offsetY).a +
                    tex2D(_MainTex, IN.uv - offsetX + offsetY).a +
                    tex2D(_MainTex, IN.uv - offsetX - offsetY).a;

                half aroundStep = step(0.01, aroundAlpha);
                half pointStep = step(_AlphaThreshold, pointAlpha);

                half4 outlineColor = _OutlineColor;
                outlineColor.rgb *= _Light;
                outlineColor.a *= aroundStep;

                half4 result = lerp(col, outlineColor, aroundStep);
                result = lerp(result, col, pointStep);

                return result;
            }

            ENDCG
        }
    }

    FallBack "Sprites/Default"
}