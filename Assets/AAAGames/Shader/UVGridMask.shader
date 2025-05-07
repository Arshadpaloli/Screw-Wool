Shader"Custom/UVGridMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GridSize ("Grid Size", Vector) = (1,1,0,0)
        _GridIndex ("Grid Index", Vector) = (0,0,0,0)
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

sampler2D _MainTex;
float4 _MainTex_ST;

float2 _GridSize; // x = cols, y = rows
float2 _GridIndex; // x = colIndex, y = rowIndex

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float2 gridUV = i.uv / _GridSize + _GridIndex / _GridSize;
    return tex2D(_MainTex, gridUV);
}
            ENDCG
        }
    }
}
