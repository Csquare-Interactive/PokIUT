Shader "Custom/TransparentShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Texture principale
        _Color ("Color", Color) = (1,1,1,1)  // Couleur avec alpha
        _Opacity ("Opacity", Range(0, 1)) = 8.0 // Contrôle de l'opacité
    }
    SubShader
    {
        Tags { "Queue"="Transparent+10" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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
            float4 _Color;
            float _Opacity; // Opacité globale

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                texColor.a *= _Color.a * _Opacity;
                return texColor * _Color;
            }
            ENDCG
        }
    }
}
