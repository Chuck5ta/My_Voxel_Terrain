/*
Test shader
only used for testing
 */
Shader "Custom/GradualBlendDiagonal"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Texture01("Texture 1", 2D) = "white" {}
        _Texture02("Texture 2", 2D) = "black" {}
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
                float4 position : SV_POSITION;
                float4 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texture01 = tex2D(_Texture01, i.texcoord);
                fixed4 texture02 = tex2D(_Texture02, i.texcoord);
                //      fixed4 c = lerp(texture01, texture02, i.texcoord.x) * _Color;

                // pythagorous's theorum to create a diagonal gradient
      //        fixed4 c = lerp(texture01, texture02, (i.texcoord.x + i.texcoord.y)) * _Color;
      //          fixed4 c = lerp(texture01, texture02, (i.texcoord.y + i.texcoord.x)) * _Color;

                // Small patch of Dirt or grass in TOP LEFT (facing positive Z) NO PINK - PERFECT COLOUR ALL THROUGH
               // fixed4 c = lerp(texture01, texture02, ((i.texcoord.y * i.texcoord.y) - (i.texcoord.x * i.texcoord.y))) * _Color;
                // Small patch of Dirt or grass in BOTTOM RIGHT (facing positive Z) NO PINK - PERFECT COLOUR ALL THROUGH
                // fixed4 c = lerp(texture01, texture02, ((i.texcoord.x * i.texcoord.x) - (i.texcoord.x * i.texcoord.y))) * _Color;

                // TOO BRIGHT!!!! Small patch of Dirt or grass in BOTTOM LEFT (facing positive Z)
        //        fixed4 c = lerp(texture01, texture02, ((i.texcoord.x * i.texcoord.x) + (i.texcoord.y * i.texcoord.y))) * _Color;

                // TOP RIGHT
                fixed4 c = lerp(texture01, texture02, (i.texcoord.x * i.texcoord.y)) * _Color;

                // BOTTOM RIGHT
                fixed4 c = lerp(texture01, texture02, ((i.texcoord.x * i.texcoord.x) - (i.texcoord.x * i.texcoord.y))) * _Color;

                // TOP LEFT
                fixed4 c = lerp(texture01, texture02, ((i.texcoord.y * i.texcoord.y) - (i.texcoord.x * i.texcoord.y))) * _Color;

                // BOTTOM LEFT 
                fixed4 c = lerp(texture01, texture02, (i.texcoord.x + i.texcoord.y) - (i.texcoord.x * i.texcoord.y)) * _Color;


                // Small patch of Dirt or grass in bottom left (facing positive Z)
                fixed4 c = lerp(texture01, texture02, (i.texcoord.x * i.texcoord.x + i.texcoord.y * i.texcoord.y)) * 0.99;
                c.a = 1; // alpha set to 1 - totally opaque
                return c;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}