Shader "Terrain Grid System/Unlit Surface Texture Ground" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "black" {}
		_Offset ("Depth Offset", Float) = -1
		_ZWrite ("ZWrite", Float) = 0
		_SrcBlend ("Src Blend", Float) = 5
		_DstBlend ("Dst Blend", Float) = 10
		_StencilComp ("Stencil Comp", Float) = 8
		_StencilRef ("Stencil Ref", Float) = 8
		_StencilOp ("Stencil Op", Float) = 0
		_AlphaTestThreshold ("Alpha Test", Range(0, 1)) = 0.5
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