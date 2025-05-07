Shader "Custom/GlassBottle"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0.5)
        _MainTex ("Main Texture", 2D) = "white" {}
        _ReflectionTex ("Reflection Cubemap", Cube) = "_Skybox" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.8
        _FresnelPower ("Fresnel Power", Range(0,5)) = 2.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            samplerCUBE _ReflectionTex;
            float4 _Color;
            float _Glossiness;
            float _FresnelPower;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float fresnel = pow(1.0 - saturate(dot(i.worldNormal, i.viewDir)), _FresnelPower);
                float4 reflection = texCUBE(_ReflectionTex, reflect(-i.viewDir, i.worldNormal)) * fresnel;
                float4 baseColor = tex2D(_MainTex, i.uv) * _Color;
                
                return float4(baseColor.rgb + reflection.rgb * _Glossiness, _Color.a);
            }
            ENDCG
        }
    }
}
