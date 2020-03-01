/*
Gradually blend 2 textures from one to the other horizontally
 */
Shader "Custom/GradualBlendHorizontal"
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
        LOD 200

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
                // texcoord.x used for horizonal blend
                fixed4 c = lerp(texture01, texture02, i.texcoord.x) * _Color;
                c.a = 1; // alpha set to 1 - totally opaque
                return c;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}