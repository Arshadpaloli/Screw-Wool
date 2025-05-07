Shader "AllIn1Vfx/Noises/AllIn1VfxFractalNoise" {
	Properties {
		_ScaleX ("Scale X", Range(0.1, 100)) = 4
		_ScaleY ("Scale Y", Range(0.1, 100)) = 4
		_StartBand ("Start Band", Range(0.1, 10)) = 1
		_EndBand ("End Band", Range(0.1, 10)) = 8
		_Offset ("Offset", Range(-100, 100)) = 1
		_Contrast ("Contrast", Range(0, 10)) = 1
		_Brightness ("Brightness", Range(-1, 1)) = 0
		[MaterialToggle] _Invert ("Invert?", Float) = 0
		[MaterialToggle] _Fractal ("Fractal?", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
}