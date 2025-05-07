Shader "lcl/OutLine/Outline_NormalExpansion2" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,1)
		_OutlinePower ("line power", Range(0, 0.2)) = 0.05
		_LineColor ("lineColor", Vector) = (1,1,1,1)
		_OffsetFactor ("Offset Factor", Range(0, 200)) = 0
		_OffsetUnits ("Offset Units", Range(0, 200)) = 0
		[Toggle(_USE_SMOOTH_NORMAL_ON)] _USE_SMOOTH_NORMAL ("Use Smooth Normal", Float) = 0
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