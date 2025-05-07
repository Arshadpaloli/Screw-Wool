Shader "Custom/CellColorFromTexture"
{
    Properties
    {
        _ColorMap ("Color Map", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _ColorMap;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; // Use this to identify cell ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Assume each cell has unique UV for lookup
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_ColorMap, i.uv);
            }
            ENDCG
        }
    }
}
