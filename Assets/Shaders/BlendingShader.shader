/*
Test shader
only use for testing
 */
Shader "Custom/Blending Shader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Texture 1", 2D) = "white" {}
        _MainTex2("Texture 2", 2D) = "white" {}
        _Middle("Middle", Range(0.001, 0.999)) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        ZWrite On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert  
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float  _Middle;

            sampler2D _MainTex;
            sampler2D _MainTex2;
            float2 uv_MainTex;
            float2 uv_MainTex2;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 t1 = tex2D(_MainTex, i.texcoord);
                fixed4 t2 = tex2D(_MainTex2, i.texcoord);
                //      o.Albedo = lerp(t1, t2, _Blend);
      //          fixed4 c = lerp(t1, t2, i.texcoord.y / 0.25) * step(i.texcoord.y, 0.25) * _Color;
                fixed4 c = lerp(t1, t2, i.texcoord.y) * _Color;
                //          fixed4 c = lerp(t2, t1, 0.3);
      //          c += lerp(t1, t2, (i.texcoord.y - 0.25) / (1 - 0.25)) * (1 - step(i.texcoord.y, 0.25)) * _Color;
      //          c += lerp(t1, t2, (i.texcoord.y - 0.5) / (1 - 0.5)) * (1 - step(i.texcoord.y, 0.5)) * _Color;
      //          c += lerp(t1, t2, (i.texcoord.y - 0.75) / (1 - 0.75)) * (1 - step(i.texcoord.y, 0.75)) * _Color;
                c.a = 1;
                return c;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}