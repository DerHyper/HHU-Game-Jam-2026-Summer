Shader "Custom/Blur"
{
    Properties
    {
        _Blur("Blur strength (filter radius)", Integer) = 1
        _Scale("Scale", Range(0.1, 10)) = 1.1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                // float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                // float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                int _Blur;
                float _Scale;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz); // Position of vertex on screen.
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 OUT = 0.0;

                // ComputeScreenPos() needs perspective divide
                half2 pos = IN.screenPos.xy / IN.screenPos.w;

                half2 texel = _Scale * (1.0 / _ScreenParams.xy); // Size of one texel in screen space, multiplied by scale factor.

                // Blur is at least 1, so we always sample the center pixel.
                int blur_size = _Blur > 0 ? _Blur : 1;

                // Iterate over a square of pixels around the center pixel
                for (int x_off = -blur_size; x_off <= blur_size; x_off++) {
                    for (int y_off = -blur_size; y_off <= blur_size; y_off++) {
                        // Sum pixel RGB values of surrounding pixels
                        OUT += SAMPLE_TEXTURE2D(
                            _MainTex,
                            sampler_MainTex,
                            pos + (half2(x_off, y_off) * texel)
                        );
                    }
                }

                // Normalise brightness
                OUT = OUT / ((2 * blur_size + 1) * (2 * blur_size + 1));

                return half4(OUT.rgb, 1.0);
            }
            ENDHLSL
        }
    }
}
