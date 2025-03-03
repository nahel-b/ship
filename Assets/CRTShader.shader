Shader "UI/CRT_PostProcess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.5
        _Distortion ("Distortion", Range(0,2)) = 0.1
        _VerticalDistortion ("Vertical Distortion", Range(0,2)) = 0.1
        _distortionCompensation ("distortionCompensation", Range(0.1,1)) = 1
        _VignetteStrength ("Vignette Strength", Range(0,1)) = 0.3
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            Name "CRT"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _ScanlineIntensity;
            float _Distortion;
            float _VerticalDistortion;
            float _distortionCompensation;
            float _VignetteStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                // Centrer les coordonnées UV
                float2 centeredUV = uv - 0.5;
                
                // Pré-étirer les coordonnées UV
                centeredUV *= _distortionCompensation;
                
                // Recalculer r2 avec les UV étirés
                float r2 = dot(centeredUV, centeredUV);
                
                // Calculer le rapport hauteur/largeur de l'écran
                float aspectRatio = _ScreenParams.y / _ScreenParams.x;
                
                // Calculer la distorsion verticale en fonction du rapport hauteur/largeur avec une montée moins violente
                float verticalDistortion = _VerticalDistortion * sqrt(aspectRatio);
                
                // Appliquer la distorsion horizontale et verticale
                float2 distortedUV = centeredUV * (1.0 + r2 * float2(_Distortion, verticalDistortion)) + 0.5;
                
                // Vérifier si les coordonnées sont dans les limites
                if (distortedUV.x < 0.0 || distortedUV.x > 1.0 || 
                    distortedUV.y < 0.0 || distortedUV.y > 1.0) {
                    return half4(0, 0, 0, 1); // Noir pour les pixels hors limites
                }
                
                // Scanlines (effet lignes horizontales)
                float scanline = sin(distortedUV.y * _ScreenParams.y * 3.1415) * _ScanlineIntensity;
                
                // Effet vignette (basé sur les UV centrés originaux)
                float vignette = 1.0 - smoothstep(0.5, 1.0, dot(uv - 0.5, uv - 0.5));
                vignette = lerp(1.0, vignette, _VignetteStrength);
                
                // Appliquer l'effet CRT
                half4 color = tex2D(_MainTex, distortedUV);
                color.rgb *= 1.0 + scanline;
                color.rgb *= vignette;
                
                return color;
            }
            ENDHLSL
        }
    }
}
