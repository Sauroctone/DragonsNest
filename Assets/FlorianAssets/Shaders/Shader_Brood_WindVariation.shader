// Upgrade NOTE: upgraded instancing buffer 'Shader_Brood_Wind' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader_Brood_Wind"
{
	Properties
	{
		_Diffuse("Diffuse", 2D) = "white" {}
		_T_Wind_01("T_Wind_01", 2D) = "white" {}
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
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _T_Wind_01;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform sampler2D _Metallic_Smothness_AO;
		uniform float4 _Metallic_Smothness_AO_ST;

		UNITY_INSTANCING_BUFFER_START(Shader_Brood_Wind)
			UNITY_DEFINE_INSTANCED_PROP(float, _SmoothnessPower)
#define _SmoothnessPower_arr Shader_Brood_Wind
			UNITY_DEFINE_INSTANCED_PROP(float, _MetallicPower)
#define _MetallicPower_arr Shader_Brood_Wind
			UNITY_DEFINE_INSTANCED_PROP(float, _EmissivePower)
#define _EmissivePower_arr Shader_Brood_Wind
		UNITY_INSTANCING_BUFFER_END(Shader_Brood_Wind)


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float4 _Vector1 = float4(0,1,0,1);
			float3 normalizeResult5_g6 = normalize( (_Vector1).xyz );
			float3 temp_cast_0 = (3.0).xxx;
			float3 temp_output_9_0_g6 = ( normalizeResult5_g6 * ( (_Vector1).w * ( ( _Time.y * 1.0 ) * -0.5 ) ) );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 temp_output_47_0_g6 = abs( ( ( frac( ( ( temp_output_9_0_g6 + ( ase_worldPos / 1024.0 ) ) + 0.5 ) ) * 2.0 ) + -1.0 ) );
			float dotResult52_g6 = dot( normalizeResult5_g6 , ( ( ( temp_cast_0 - ( temp_output_47_0_g6 * 2.0 ) ) * temp_output_47_0_g6 ) * temp_output_47_0_g6 ) );
			float3 temp_cast_1 = (3.0).xxx;
			float3 temp_output_31_0_g6 = abs( ( ( frac( ( ( temp_output_9_0_g6 + ( ase_worldPos / 200.0 ) ) + 0.5 ) ) * 2.0 ) + -1.0 ) );
			float3 temp_cast_2 = (0.0).xxx;
			float3 temp_cast_3 = (0.001).xxx;
			float2 panner8_g1 = ( 1.0 * _Time.y * (temp_cast_3).xz + (( ase_worldPos / 0.1 )).xz);
			float3 temp_cast_4 = (-0.13).xxx;
			float3 temp_output_61_0_g6 = ( tex2Dlod( _T_Wind_01, float4( panner8_g1, 0, 0.0) ) * float4( temp_cast_4 , 0.0 ) ).rgb;
			float3 rotatedValue62_g6 = RotateAroundAxis( ( temp_output_61_0_g6 + float3(0,0,-10) ), temp_output_61_0_g6, cross( normalizeResult5_g6 , float3(0,0,1) ), ( dotResult52_g6 + distance( ( ( ( temp_cast_1 - ( temp_output_31_0_g6 * 2.0 ) ) * temp_output_31_0_g6 ) * temp_output_31_0_g6 ) , temp_cast_2 ) ) );
			float temp_output_31_0_g1 = pow( ( 1.0 - v.texcoord.xy.y ) , 0.0 );
			float3 lerpResult35_g1 = lerp( ase_vertexNormal , ( ( ( rotatedValue62_g6 * 0.05 ) * 0.04 ) + temp_output_61_0_g6 ) , temp_output_31_0_g1);
			v.vertex.xyz += lerpResult35_g1;
		}

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
1927;23;1266;964;285.9283;105.2641;1;True;True
Node;AmplifyShaderEditor.SamplerNode;85;-92.92649,-230.3021;Float;True;Property;_Metallic_Smothness_AO;Metallic_Smothness_AO;6;0;Create;True;0;0;False;0;None;b1488ca1a58a4794a9e9e026223a472d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;89;79.07623,126.8713;Float;False;InstancedProperty;_SmoothnessPower;SmoothnessPower;8;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;177.3337,637.0499;Float;False;Constant;_WindOffset;WindOffset;10;0;Create;True;0;0;False;0;-0.13;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;218.7399,285.9783;Float;False;Constant;_WindWaveSpeed;Wind Wave Speed;9;0;Create;True;0;0;False;0;0.001;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;173.3337,735.0499;Float;False;Constant;_Wind_Line_Size;Wind_Line_Size;11;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-87.78934,-420.1559;Float;False;InstancedProperty;_EmissivePower;EmissivePower;5;0;Create;True;0;0;False;0;0;0;0;5000;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;94;181.3192,434.0097;Float;False;Constant;_Wind_Weight;Wind_Weight;11;0;Create;True;0;0;False;0;0.05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;181.3192,356.0097;Float;False;Constant;_GradientPower;GradientPower;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-109.3232,-608.8534;Float;True;Property;_Emissive;Emissive;4;0;Create;True;0;0;False;0;None;e3841bdb6607a914188ca6270b974615;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;87;71.46069,-44.26764;Float;False;InstancedProperty;_MetallicPower;MetallicPower;7;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;167.3192,504.0097;Float;False;Constant;_Wind_Intensity;Wind_Intensity;9;0;Create;True;0;0;False;0;0.04;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;555.3941,-23.40369;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;560.5778,-235.743;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;91;449.8492,338.9849;Float;True;SF_GrassWind;1;;1;1a4cb010744b7544f8d1ab89ba3fe301;0;7;40;FLOAT3;-0.2,-0.1,0.1;False;32;FLOAT;2;False;21;FLOAT;0.2;False;23;FLOAT;0.6;False;17;SAMPLER2D;;False;19;FLOAT3;50,20,30;False;4;FLOAT;2000;False;2;FLOAT;37;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;41;-112.2518,-801.6869;Float;True;Property;_Normal;Normal;3;1;[Normal];Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;417.8208,-598.3649;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;40;-99.40285,-1024.213;Float;True;Property;_Diffuse;Diffuse;0;0;Create;True;0;0;False;0;None;39cee1e6888e132428f3db162cb50f95;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1000.483,-771.8052;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Shader_Brood_Wind;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;90;0;85;2
WireConnection;90;1;89;0
WireConnection;86;0;85;1
WireConnection;86;1;87;0
WireConnection;91;40;92;0
WireConnection;91;32;93;0
WireConnection;91;21;94;0
WireConnection;91;23;95;0
WireConnection;91;19;96;0
WireConnection;91;4;97;0
WireConnection;43;0;36;0
WireConnection;43;1;88;0
WireConnection;0;0;40;0
WireConnection;0;1;41;0
WireConnection;0;2;43;0
WireConnection;0;3;86;0
WireConnection;0;4;90;0
WireConnection;0;5;85;3
WireConnection;0;11;91;0
ASEEND*/
//CHKSM=0BEC5C5C558F041CE3E7EFC55120B965FB9EADDF