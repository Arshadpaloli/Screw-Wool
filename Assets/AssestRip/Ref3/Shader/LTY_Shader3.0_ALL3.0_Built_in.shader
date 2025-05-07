Shader "LTY/Shader3.0/ALL3.0_Built_in" {
	Properties {
		[Enum(Add,1,Alpha,10)] _Dst ("BlendMode", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("CullMode", Float) = 0
		[Enum(Less or Equal,4,Always,8)] _ZTestMode ("ZTest", Float) = 4
		[Enum(ON,1,OFF,0)] _Zwrite ("Zwrite", Float) = 0
		_SoftParticle ("SoftParticle", Range(0, 10)) = 0
		[Header(Main_Tex)] _Main_Tex ("Main_Tex", 2D) = "white" {}
		[HDR] _Main_Tex_Color ("Main_Tex_Color", Vector) = (1,1,1,1)
		[Enum(R,0,A,1)] _Main_Tex_A_R ("Main_Tex_A_R", Float) = 0
		[Enum(Normal,0,Custom,1)] _Main_Tex_Custom_ZW ("Main_Tex_Custom_ZW", Float) = 0
		[Enum(Repeat,0,Clmap,1)] _Main_Tex_ClampSwitch ("Main_Tex_ClampSwitch", Float) = 0
		_Main_Tex_Rotator ("Main_Tex_Rotator", Range(0, 360)) = 0
		_Main_Tex_U_speed ("Main_Tex_U_speed", Float) = 0
		_Main_Tex_V_speed ("Main_Tex_V_speed", Float) = 0
		[Header(Remap_Tex)] _Remap_Tex ("Remap_Tex", 2D) = "white" {}
		[Enum(OFF,0,ON,1)] _Remap_Tex_Followl_Main_Tex ("Remap_Tex_Followl_Main_Tex", Float) = 0
		[Enum(R,0,A,1)] _Remap_Tex_A_R ("Remap_Tex_A_R", Float) = 0
		_Remap_Tex_Desaturate ("Remap_Tex_Desaturate", Range(0, 1)) = 0
		[Enum(Repeat,0,Clmap,1)] _Remap_Tex_ClampSwitch ("Remap_Tex_ClampSwitch", Float) = 0
		_Remap_Tex_Rotator ("Remap_Tex_Rotator", Range(0, 360)) = 0
		_Remap_Tex_U_speed ("Remap_Tex_U_speed", Float) = 0
		_Remap_Tex_V_speed ("Remap_Tex_V_speed", Float) = 0
		[Header(Mask_Tex)] _Mask_Tex ("Mask_Tex", 2D) = "white" {}
		[Enum(Repeat,0,Clmap,1)] _Mask_Tex_ClampSwitch ("Mask_Tex_ClampSwitch", Float) = 0
		[Enum(R,0,A,1)] _Mask_Tex_A_R ("Mask_Tex_A_R", Float) = 0
		_Mask_Tex_Rotator ("Mask_Tex_Rotator", Range(0, 360)) = 0
		_Mask_Tex_U_speed ("Mask_Tex_U_speed", Float) = 0
		_Mask_Tex_V_speed ("Mask_Tex_V_speed", Float) = 0
		[Header(Dissolve_Tex)] _Dissolve_Tex ("Dissolve_Tex", 2D) = "white" {}
		[Toggle(_DISSOLVE_SWITCH_ON)] _Dissolve_Switch ("Dissolve_Switch", Float) = 0
		[Enum(R,0,A,1)] _Dissolve_Tex_A_R ("Dissolve_Tex_A_R", Float) = 0
		[Enum(OFF,0,ON,1)] _Dissolve_Tex_Custom_X ("Dissolve_Tex_Custom_X", Float) = 0
		_Dissolve_Tex_Rotator ("Dissolve_Tex_Rotator", Range(0, 360)) = 0
		_Dissolve_Tex_smooth ("Dissolve_Tex_smooth", Range(0.5, 1)) = 0.5
		_Dissolve_Tex_power ("Dissolve_Tex_power", Range(0, 1)) = 0
		_Dissolve_Tex_U_speed ("Dissolve_Tex_U_speed", Float) = 0
		_Dissolve_Tex_V_speed ("Dissolve_Tex_V_speed", Float) = 0
		[Header(Distortion_Tex)] _Distortion_Tex ("Distortion_Tex", 2D) = "white" {}
		[Enum(OFF,0,ON,1)] _Distortion_Switch ("Distortion_Switch", Float) = 0
		_Distortion_Tex_Power ("Distortion_Tex_Power", Float) = 0
		_Distortion_Tex_U_speed ("Distortion_Tex_U_speed", Float) = 0
		_Distortion_Tex_V_speed ("Distortion_Tex_V_speed", Float) = 0
		[Header(Fresnel)] [HDR] _Fresnel_Color ("Fresnel_Color", Vector) = (1,1,1,1)
		[Toggle(_FRESNEL_SWITCH_ON)] _Fresnel_Switch ("Fresnel_Switch", Float) = 0
		[Enum(Fresnel,0,Bokeh,1)] _Fresnel_Bokeh ("Fresnel_Bokeh", Float) = 0
		_Fresnel_scale ("Fresnel_scale", Float) = 1
		_Fresnel_power ("Fresnel_power", Float) = 5
		[Header(Wpo_Tex)] _WPO_Tex ("WPO_Tex", 2D) = "white" {}
		[Toggle(_WPO_SWITCH_ON)] _WPO_Switch ("WPO_Switch", Float) = 0
		[Enum(Normal,0,Custom,1)] _WPO_CustomSwitch_Y ("WPO_CustomSwitch_Y", Float) = 0
		_WPO_tex_power ("WPO_tex_power", Float) = 0
		_WPO_Tex_Direction ("WPO_Tex_Direction", Vector) = (1,1,1,0)
		_WPO_Tex_U_speed ("WPO_Tex_U_speed", Float) = 0
		_WPO_Tex_V_speed ("WPO_Tex_V_speed", Float) = 0
		[HideInInspector] _texcoord2 ("", 2D) = "white" {}
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
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
	//CustomEditor "LTY_ShaderGUI_Built_in"
}