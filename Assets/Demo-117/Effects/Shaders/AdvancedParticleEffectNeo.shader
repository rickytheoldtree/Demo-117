Shader "Custom/AdvancedParticleEffectNeo_URP"
{
    Properties
    {
        [HDR]_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _ScrollSpeed ("UV Scroll Speed", Vector) = (0,0,0,0)

        [Toggle]_UseMask ("Use Mask", Float) = 0
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _MaskScrollSpeed ("Mask UV Scroll Speed", Vector) = (0,0,0,0)

        [Toggle]_UseMask2 ("Use Mask 2", Float) = 0
        _MaskTex2 ("Mask Texture 2", 2D) = "white" {}
        _MaskScrollSpeed2 ("Mask 2 UV Scroll Speed", Vector) = (0,0,0,0)

        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend Mode", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend Mode", Float) = 10
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
        }

        Pass
        {
            Name "UNLIT"
            Tags { "LightMode"="SRPDefaultUnlit" }

            Blend [_SrcBlend] [_DstBlend]
            ZWrite Off
            ZTest LEqual
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // =========================
            // Textures & Samplers
            // =========================
            TEXTURE2D(_MainTex);   SAMPLER(sampler_MainTex);
            TEXTURE2D(_MaskTex);   SAMPLER(sampler_MaskTex);
            TEXTURE2D(_MaskTex2);  SAMPLER(sampler_MaskTex2);

            float4 _MainTex_ST;
            float4 _MaskTex_ST;
            float4 _MaskTex2_ST;

            // =========================
            // Params
            // =========================
            float4 _Color;

            float2 _ScrollSpeed;
            float2 _MaskScrollSpeed;
            float2 _MaskScrollSpeed2;

            float _UseMask;
            float _UseMask2;

            // =========================
            // Structs
            // =========================
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uvMain     : TEXCOORD0;
                float2 uvMask     : TEXCOORD1;
                float2 uvMask2    : TEXCOORD2;
                float4 color      : COLOR;
            };

            // =========================
            // Vertex
            // =========================
            Varyings vert (Attributes v)
            {
                Varyings o;

                VertexPositionInputs pos =
                    GetVertexPositionInputs(v.positionOS.xyz);

                o.positionCS = pos.positionCS;

                float t = _Time.y;

                // 主贴图 UV 流动
                o.uvMain =
                    TRANSFORM_TEX(v.uv, _MainTex) + _ScrollSpeed * t;

                // Mask 1 UV 流动
                o.uvMask =
                    TRANSFORM_TEX(v.uv, _MaskTex) + _MaskScrollSpeed * t;

                // Mask 2 UV 流动
                o.uvMask2 =
                    TRANSFORM_TEX(v.uv, _MaskTex2) + _MaskScrollSpeed2 * t;

                o.color = v.color;
                return o;
            }

            // =========================
            // Fragment
            // =========================
            half4 frag (Varyings i) : SV_Target
            {
                // 基础颜色（贴图 * HDR Color * 粒子顶点色）
                half4 col =
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uvMain)
                    * _Color * i.color;

                // Mask 叠乘
                half m = 1.0;

                if (_UseMask > 0.5)
                {
                    m *= SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uvMask).r;
                }

                if (_UseMask2 > 0.5)
                {
                    m *= SAMPLE_TEXTURE2D(_MaskTex2, sampler_MaskTex2, i.uvMask2).r;
                }

                col.a *= m;
                return col;
            }
            ENDHLSL
        }
    }

    FallBack Off
}