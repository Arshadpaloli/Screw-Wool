Shader "Unlit/LidDissolveShader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_EdgeColour1 ("Edge colour 1", Vector) = (1,1,1,1)
		_EdgeColour2 ("Edge colour 2", Vector) = (1,1,1,1)
		_DissolveLevel ("Dissolution level", Range(-0.2, 1)) = -0.2
		_Edges ("Edge width", Range(0, 1)) = 0.1
		_MysteryColor ("Mystery Color", Vector) = (0.75,0.82,0.98,0.56)
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