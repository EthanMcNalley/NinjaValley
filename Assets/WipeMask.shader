Shader "Hidden/WipeStencilMask"
{
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "WipeStencilMask"
            ZWrite Off
            ZTest Always
            ColorMask 0   // write NO color
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float2 _WipeOriginPoint;
            float _WipeSize;
            float _NoiseScale;
            float _NoiseStrength;

            // --- noise functions (same as your wipe shader) ---
            inline float randomValue(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            inline float perlinLerp(float a, float b, float t)
            {
                return (1.0 - t) * a + t * b;
            }

            inline float valueNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                float2 c0 = i + float2(0,0);
                float2 c1 = i + float2(1,0);
                float2 c2 = i + float2(0,1);
                float2 c3 = i + float2(1,1);

                float r0 = randomValue(c0);
                float r1 = randomValue(c1);
                float r2 = randomValue(c2);
                float r3 = randomValue(c3);

                float bottom = perlinLerp(r0, r1, f.x);
                float top    = perlinLerp(r2, r3, f.x);

                return perlinLerp(bottom, top, f.y);
            }

            float perlinNoise(float2 uv, float scale)
            {
                float t = 0.0;
                float2 scaledUV = uv * scale;

                t += valueNoise(scaledUV / 1.0) * 0.25;
                t += valueNoise(scaledUV / 2.0) * 0.5;
                t += valueNoise(scaledUV / 4.0) * 1.0;

                return t;
            }

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 screenPos  : TEXCOORD0;
            };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.screenPos = ComputeScreenPos(o.positionCS);
                return o;
            }

            float4 Frag(Varyings i) : SV_Target
            {
                float2 uv = i.screenPos.xy / i.screenPos.w;

                float2 offset = uv - _WipeOriginPoint;
                offset.x *= _ScreenParams.x / _ScreenParams.y;

                float noise = perlinNoise(offset, _NoiseScale);
                float distance = length(offset) + noise * _NoiseStrength;

                float inside = 1.0 - step(_WipeSize, distance);

                // Only write stencil where inside == 1
                clip(inside - 0.5);

                return 0;
            }

            ENDHLSL
        }
    }
}