/*
Test shader
only used for testing
 */
Shader "Custom/Test Shader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Texture01("Texture 1", 2D) = "white" {}
        _Texture02("Texture 2", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 300

        ZWrite On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert  
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            sampler2D _Texture01;
            sampler2D _Texture02;

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
                fixed4 texture01 = tex2D(_Texture01, i.texcoord);
                fixed4 texture02 = tex2D(_Texture02, i.texcoord);
                fixed4 c = lerp(texture01, texture02, i.texcoord.y) * _Color;
                //          fixed4 c = lerp(t1, t2, i.texcoord.y / 0.25) * step(i.texcoord.y, 0.25) * _Color;
                //     c.a = 1;
                return c;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}