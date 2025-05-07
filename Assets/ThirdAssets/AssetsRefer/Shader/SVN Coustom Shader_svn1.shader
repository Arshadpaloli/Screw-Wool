Shader "SVN Coustom Shader/svn1" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		[Header(Color)] [Space] _Color ("Main Color", Vector) = (1,1,1,1)
		_Brightness ("Main Color Brightness", Range(0, 1)) = 0.3
		[Space] [Space] [Header(Shadow Controls)] [Space] [Space] [MaterialToggle] _ReceiveShadows ("Receive Shadows?", Float) = 1
		[Header(Highlights and shadows)] [Space] [Space] _HColor ("Highlight Color", Vector) = (0.75,0.75,0.75,1)
		_Strength ("Highlight Strength", Range(0, 1)) = 0.5
		_SColor ("Shadow Color", Vector) = (0.2,0.2,0.2,1)
		[Space] [Header(Toon Effects)] [Space] _Detail ("Detail", Range(0, 1)) = 0.3
		_Smoothness ("Smoothness", Range(0, 1)) = 0.3
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