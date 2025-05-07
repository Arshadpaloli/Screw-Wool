Shader "Terrain Grid System/Unlit Highlight Ground Texture" {
	Properties {
		_Color ("Tint Color", Vector) = (1,1,1,0.5)
		_Color2 ("Color", Vector) = (0,1,0,0)
		_Offset ("Depth Offset", Float) = -1
		_MainTex ("Texture", 2D) = "black" {}
		_FadeAmount ("Fade Amount", Float) = 0
		_Scale ("Scale Min", Float) = 1
		_ZWrite ("ZWrite", Float) = 0
		_SrcBlend ("Src Blend", Float) = 5
		_DstBlend ("Dst Blend", Float) = 10
		_Cull ("Cull", Float) = 2
		_ZTest ("ZTest", Float) = 4
		_HighlightBorderColor ("Highlight Border Color", Vector) = (1,1,1,1)
		_HighlightBorderSize ("Highlight Border Size", Float) = 1
		_Mask ("Mask Texture", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}