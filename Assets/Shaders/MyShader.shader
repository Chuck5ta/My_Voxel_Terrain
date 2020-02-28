/*
Test shader
only use for testing
 */
Shader "Custom/Texture Blend" 
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Blend("Texture Blend", Range(0,1)) = 0.0
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _MainTex2("Albedo 2 (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MainTex2;

        struct Input {
            float2 uv_MainTex;
            float2 uv_MainTex2;
        };

        half _Blend;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        struct appdata
        {
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
        };

        struct v2f
        {
            float4 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        v2f vert(inout appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord;
            return o;
        }

        void surf(Input IN, inout SurfaceOutputStandard o) 
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = lerp(tex2D(_MainTex, IN.uv_MainTex), tex2D(_MainTex2, IN.uv_MainTex2), _Blend) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}