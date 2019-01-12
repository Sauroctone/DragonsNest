// Upgrade NOTE: upgraded instancing buffer 'Shader_Brood_Standard' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader_Brood_Standard"
{
	Properties
	{
		_Diffuse("Diffuse", 2D) = "white" {}
		[Normal]_Normal("Normal", 2D) = "bump" {}
		_Emissive("Emissive", 2D) = "white" {}
		_EmissivePower("EmissivePower", Range( 0 , 5000)) = 0
		_Metallic_Smothness_AO("Metallic_Smothness_AO", 2D) = "white" {}
		_MetallicPower("MetallicPower", Range( 0 , 2)) = 1
		_SmoothnessPower("SmoothnessPower", Range( 0 , 2)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform sampler2D _Metallic_Smothness_AO;
		uniform float4 _Metallic_Smothness_AO_ST;

		UNITY_INSTANCING_BUFFER_START(Shader_Brood_Standard)
			UNITY_DEFINE_INSTANCED_PROP(float, _SmoothnessPower)
#define _SmoothnessPower_arr Shader_Brood_Standard
			UNITY_DEFINE_INSTANCED_PROP(float, _MetallicPower)
#define _MetallicPower_arr Shader_Brood_Standard
			UNITY_DEFINE_INSTANCED_PROP(float, _EmissivePower)
#define _EmissivePower_arr Shader_Brood_Standard
		UNITY_INSTANCING_BUFFER_END(Shader_Brood_Standard)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			o.Albedo = tex2D( _Diffuse, uv_Diffuse ).rgb;
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			float _EmissivePower_Instance = UNITY_ACCESS_INSTANCED_PROP(_EmissivePower_arr, _EmissivePower);
			o.Emission = ( tex2D( _Emissive, uv_Emissive ) * _EmissivePower_Instance ).rgb;
			float2 uv_Metallic_Smothness_AO = i.uv_texcoord * _Metallic_Smothness_AO_ST.xy + _Metallic_Smothness_AO_ST.zw;
			float4 tex2DNode85 = tex2D( _Metallic_Smothness_AO, uv_Metallic_Smothness_AO );
			float _MetallicPower_Instance = UNITY_ACCESS_INSTANCED_PROP(_MetallicPower_arr, _MetallicPower);
			o.Metallic = ( tex2DNode85.r * _MetallicPower_Instance );
			float _SmoothnessPower_Instance = UNITY_ACCESS_INSTANCED_PROP(_SmoothnessPower_arr, _SmoothnessPower);
			o.Smoothness = ( tex2DNode85.g * _SmoothnessPower_Instance );
			o.Occlusion = tex2DNode85.b;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
382.4;92.8;1092;1007;320.1435;675.1778;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;89;79.07623,126.8713;Float;False;InstancedProperty;_SmoothnessPower;SmoothnessPower;6;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;71.46069,-44.26764;Float;False;InstancedProperty;_MetallicPower;MetallicPower;5;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-109.3232,-608.8534;Float;True;Property;_Emissive;Emissive;2;0;Create;True;0;0;False;0;None;e3841bdb6607a914188ca6270b974615;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;88;-87.78934,-420.1559;Float;False;InstancedProperty;_EmissivePower;EmissivePower;3;0;Create;True;0;0;False;0;0;0;0;5000;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;85;-92.92649,-230.3021;Float;True;Property;_Metallic_Smothness_AO;Metallic_Smothness_AO;4;0;Create;True;0;0;False;0;None;b1488ca1a58a4794a9e9e026223a472d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;40;-99.40285,-1024.213;Float;True;Property;_Diffuse;Diffuse;0;0;Create;True;0;0;False;0;None;39cee1e6888e132428f3db162cb50f95;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;560.5778,-235.743;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;555.3941,-23.40369;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;417.8208,-598.3649;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;41;-112.2518,-801.6869;Float;True;Property;_Normal;Normal;1;1;[Normal];Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1000.483,-771.8052;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Shader_Brood_Standard;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;86;0;85;1
WireConnection;86;1;87;0
WireConnection;90;0;85;2
WireConnection;90;1;89;0
WireConnection;43;0;36;0
WireConnection;43;1;88;0
WireConnection;0;0;40;0
WireConnection;0;1;41;0
WireConnection;0;2;43;0
WireConnection;0;3;86;0
WireConnection;0;4;90;0
WireConnection;0;5;85;3
ASEEND*/
//CHKSM=778DDD23E2D25F7CDB2D41203D465E0AD56115B9