Shader "Custom/Final Shader"
{
    Properties
    {
        _SceneTex ("Scene Texture", 2D) = "white" {}
        _UITex ("UI Texture", 2D) = "black" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _SceneTex;
            sampler2D _UITex;

            v2f vert (float4 vertex : POSITION, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(vertex);
                o.uv = uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Récupérer les pixels des deux textures
                half4 sceneColor = tex2D(_SceneTex, i.uv);
                half4 uiColor = tex2D(_UITex, i.uv);

                // Si le pixel de l'UI est noir, rendre transparent
                if (length(uiColor.rgb - half3(0.0, 0.0, 0.0)) < 0.1)
                {
                    uiColor.a = 0.0;
                }

                // L'UI est rendue avec alpha, on fait une fusion
                return lerp(sceneColor, uiColor, uiColor.a);
            }
            ENDHLSL
        }
    }
}
