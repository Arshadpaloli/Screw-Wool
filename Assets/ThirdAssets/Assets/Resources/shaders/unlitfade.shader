Shader "Terrain Grid System/Unlit Fade" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_Offset ("Depth Offset", Float) = -1
		_NearClip ("Near Clip", Range(0, 1000)) = 25
		_FallOff ("FallOff", Range(1, 1000)) = 50
		_FarFadeDistance ("Far Fade Distance", Float) = 10000
		_FarFadeFallOff ("Far Fade FallOff", Range(1, 1000)) = 50
		_CircularFadeDistance ("Circular Fade Distance", Float) = 250000
		_CircularFadeFallOff ("Circular Fade FallOff", Float) = 50
		_ZTest ("ZTest", Float) = 4
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
}