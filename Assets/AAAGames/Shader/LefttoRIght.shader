Shader "Custom/LitTextureRevealRightToLeft"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Cutoff ("Reveal Amount", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        LOD 200
        Cull Off
        ZWrite On

        CGPROGRAM
        #pragma surface surf Standard alpha:clip

        sampler2D _MainTex;
        float4 _Color;
        float _Cutoff;
        float3 _ObjectBoundsMin;
        float3 _ObjectBoundsMax;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float revealStart = _ObjectBoundsMin.x;
            float revealEnd = _ObjectBoundsMax.x;
            float revealRange = revealEnd - revealStart;
            float xProgress = saturate((IN.worldPos.x - revealStart) / revealRange);

            // Perform reveal
            clip((_Cutoff - xProgress) * 100); // Sharp cutoff

            // Sample texture and apply tint color
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            tex *= _Color;

            o.Albedo = tex.rgb;
            o.Alpha = 1;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
