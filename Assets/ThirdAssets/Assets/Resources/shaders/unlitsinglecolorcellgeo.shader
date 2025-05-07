Shader "Terrain Grid System/Unlit Single Color Cell Geo" {
	Properties {
		_MainTex ("Texture", 2D) = "black" {}
		_Color ("Color", Vector) = (1,1,1,1)
		_Offset ("Depth Offset", Float) = -0.01
		_NearClip ("Near Clip", Range(0, 1000)) = 25
		_FallOff ("FallOff", Range(1, 1000)) = 50
		_FarFadeDistance ("Far Fade Distance", Float) = 10000
		_FarFadeFallOff ("Far Fade FallOff", Range(1, 1000)) = 50
		_CircularFadeDistance ("Circular Fade Distance", Float) = 250000
		_CircularFadeFallOff ("Circular Fade FallOff", Float) = 50
		_Thickness ("Thickness", Float) = 0.05
		_ZWrite ("ZWrite", Float) = 0
		_SrcBlend ("Src Blend", Float) = 5
		_DstBlend ("Dst Blend", Float) = 10
		_StencilComp ("Stencil Comp", Float) = 6
		_StencilOp ("Stencil Op", Float) = 2
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