Shader "AllIn1Vfx/Noises/AllIn1VfxWorleyNoise" {
	Properties {
		_ScaleX ("Scale X", Range(0.1, 100)) = 10
		_ScaleY ("Scale Y", Range(0.1, 100)) = 10
		_Jitter ("Jitter", Range(0, 2)) = 1
		_NoiseType ("Noise Type", Range(0, 4)) = 0
		_Offset ("Offset", Range(-100, 100)) = 1
		_Contrast ("Contrast", Range(0, 10)) = 1
		_Brightness ("Brightness", Range(-1, 1)) = 0
		[MaterialToggle] _Invert ("Invert?", Float) = 0
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