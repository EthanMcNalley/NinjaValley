Shader "ColorCorrection"
{
    Properties
    {
        _MainTex ("Scene Texture", 2D) = "white" {}
        _LUT ("LUT Texture", 2D) = "white" {}
        _MaskTex ("Mask Sprite", 2D) = "white" {}  // Your sprite mask
        _Contribution ("Contribution", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #define COLORS 32.0

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionHCS : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_LUT);
            SAMPLER(sampler_LUT);

            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);

            float4 _LUT_TexelSize;
            float _Contribution;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample scene
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // Sample LUT
                float maxColor = COLORS - 1.0;
                float halfColX = 0.5 / _LUT_TexelSize.z;
                float halfColY = 0.5 / _LUT_TexelSize.w;
                float threshold = maxColor / COLORS;

                float xOffset = halfColX + col.r * threshold / COLORS;
                float yOffset = halfColY + col.g * threshold;
                float cell = floor(col.b * maxColor);

                float2 lutPos = float2(cell / COLORS + xOffset, yOffset);
                lutPos = clamp(lutPos, 0.0, 1.0);

                half4 gradedCol = SAMPLE_TEXTURE2D(_LUT, sampler_LUT, lutPos);

                // Sample mask (sprite)
                half maskAlpha = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, IN.uv).a;

                // Apply LUT only where mask is visible
                half3 finalCol = lerp(col.rgb, gradedCol.rgb, _Contribution * maskAlpha);

                return half4(finalCol, col.a);
            }

            ENDHLSL
        }
    }
}
