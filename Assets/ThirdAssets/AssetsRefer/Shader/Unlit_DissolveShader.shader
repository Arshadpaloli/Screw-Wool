Shader "Unlit/DissolveShader" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_DissolveMap ("Dissolve Shape", 2D) = "black" {}
		_DissolveLevel ("Dissolve Value", Range(0, 1)) = 1
		_LineWidth ("Line Width", Range(0, 0.2)) = 0.1
		_LineColor ("Line Color", Vector) = (1,1,1,1)
		_MysteryColor ("Mystery Color", Vector) = (0.75,0.82,0.98,0.56)
		_MainColor ("Main Color", Vector) = (0.75,0.82,0.98,0.56)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}