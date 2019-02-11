// Upgrade NOTE: upgraded instancing buffer 'Shader_FresnelTest' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader_FresnelTest"
{
	Properties
	{
		_Diffuse("Diffuse", 2D) = "white" {}
		[Normal]_Normal("Normal", 2D) = "bump" {}
		_Emissive("Emissive", 2D) = "white" {}
		_EmissivePower("EmissivePower", Range( 0 , 5000)) = 0
		_Metallic_Smoothness_Mask("Metallic_Smoothness_Mask", 2D) = "white" {}
		_MetallicPower("MetallicPower", Range( 0 , 2)) = 1
		_SmoothnessPower("SmoothnessPower", Range( 0 , 2)) = 1
		_TopReflectionMap("TopReflectionMap", 2D) = "white" {}
		_TopReflectionColor1("TopReflectionColor1", Color) = (0.6627451,0.4470589,0.1647059,1)
		_TopReflectionColor2("TopReflectionColor2", Color) = (0.4039216,0.1843137,0,1)
		_OffsetX("OffsetX", Float) = 1
		_OffsetY("OffsetY", Float) = 0.7
		_ScaleX("ScaleX", Float) = 0.5
		_ScaleY("ScaleY", Float) = 1
		_BlendingPower("BlendingPower", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform sampler2D _TopReflectionMap;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform sampler2D _Metallic_Smoothness_Mask;
		uniform float4 _Metallic_Smoothness_Mask_ST;

		UNITY_INSTANCING_BUFFER_START(Shader_FresnelTest)
			UNITY_DEFINE_INSTANCED_PROP(float4, _TopReflectionColor2)
#define _TopReflectionColor2_arr Shader_FresnelTest
			UNITY_DEFINE_INSTANCED_PROP(float4, _TopReflectionColor1)
#define _TopReflectionColor1_arr Shader_FresnelTest
			UNITY_DEFINE_INSTANCED_PROP(float, _EmissivePower)
#define _EmissivePower_arr Shader_FresnelTest
			UNITY_DEFINE_INSTANCED_PROP(float, _BlendingPower)
#define _BlendingPower_arr Shader_FresnelTest
			UNITY_DEFINE_INSTANCED_PROP(float, _SmoothnessPower)
#define _SmoothnessPower_arr Shader_FresnelTest
			UNITY_DEFINE_INSTANCED_PROP(float, _MetallicPower)
#define _MetallicPower_arr Shader_FresnelTest
			UNITY_DEFINE_INSTANCED_PROP(float, _ScaleY)
#define _ScaleY_arr Shader_FresnelTest
			UNITY_DEFINE_INSTANCED_PROP(float, _ScaleX)
#define _ScaleX_arr Shader_FresnelTest
			UNITY_DEFINE_INSTANCED_PROP(float, _OffsetY)
#define _OffsetY_arr Shader_FresnelTest
			UNITY_DEFINE_INSTANCED_PROP(float, _OffsetX)
#define _OffsetX_arr Shader_FresnelTest
		UNITY_INSTANCING_BUFFER_END(Shader_FresnelTest)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float4 _TopReflectionColor1_Instance = UNITY_ACCESS_INSTANCED_PROP(_TopReflectionColor1_arr, _TopReflectionColor1);
			float3 temp_output_10_0_g323 = _TopReflectionColor1_Instance.rgb;
			float4 _TopReflectionColor2_Instance = UNITY_ACCESS_INSTANCED_PROP(_TopReflectionColor2_arr, _TopReflectionColor2);
			float3 temp_output_11_0_g323 = _TopReflectionColor2_Instance.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 ase_tanViewDir = mul( ase_worldToTangent, ase_worldViewDir );
			float3 worldToObjDir16_g330 = mul( unity_WorldToObject, float4( ( ase_tanViewDir - float3(0,1,0) ), 0 ) ).xyz;
			float2 appendResult9_g330 = (float2(worldToObjDir16_g330.x , ( worldToObjDir16_g330.y * -1.0 )));
			float _ScaleX_Instance = UNITY_ACCESS_INSTANCED_PROP(_ScaleX_arr, _ScaleX);
			float4 temp_cast_2 = (_ScaleX_Instance).xxxx;
			float _ScaleY_Instance = UNITY_ACCESS_INSTANCED_PROP(_ScaleY_arr, _ScaleY);
			float4 temp_cast_4 = (_ScaleY_Instance).xxxx;
			float2 appendResult23_g330 = (float2(temp_cast_2.x , temp_cast_4.x));
			float _OffsetX_Instance = UNITY_ACCESS_INSTANCED_PROP(_OffsetX_arr, _OffsetX);
			float4 temp_cast_6 = (_OffsetX_Instance).xxxx;
			float _OffsetY_Instance = UNITY_ACCESS_INSTANCED_PROP(_OffsetY_arr, _OffsetY);
			float4 temp_cast_8 = (_OffsetY_Instance).xxxx;
			float2 appendResult25_g330 = (float2(temp_cast_6.x , temp_cast_8.x));
			float3 clampResult2_g323 = clamp( ( 1.0 - tex2D( _TopReflectionMap, ( ( ( ( appendResult9_g330 * 0.5 ) + 0.5 ) * appendResult23_g330 ) + appendResult25_g330 ) ) ).rgb , float3( 0,0,0 ) , float3( 1,0,0 ) );
			float3 desaturateInitialColor4_g327 = clampResult2_g323;
			float desaturateDot4_g327 = dot( desaturateInitialColor4_g327, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar4_g327 = lerp( desaturateInitialColor4_g327, desaturateDot4_g327.xxx, 0.0 );
			float temp_output_2_0_g327 = 0.0;
			float3 temp_cast_11 = (temp_output_2_0_g327).xxx;
			float temp_output_3_0_g327 = 0.5;
			float3 temp_cast_12 = (temp_output_3_0_g327).xxx;
			float3 clampResult5_g327 = clamp( desaturateVar4_g327 , temp_cast_11 , temp_cast_12 );
			float3 temp_cast_13 = (temp_output_2_0_g327).xxx;
			float3 lerpResult9_g323 = lerp( temp_output_10_0_g323 , temp_output_11_0_g323 , ( ( clampResult5_g327 - temp_cast_13 ) / ( temp_output_3_0_g327 - temp_output_2_0_g327 ) ).x);
			float3 temp_output_22_0_g323 = float4(0,0,0,1).rgb;
			float3 desaturateInitialColor4_g325 = clampResult2_g323;
			float desaturateDot4_g325 = dot( desaturateInitialColor4_g325, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar4_g325 = lerp( desaturateInitialColor4_g325, desaturateDot4_g325.xxx, 0.0 );
			float temp_output_2_0_g325 = 0.5;
			float3 temp_cast_16 = (temp_output_2_0_g325).xxx;
			float temp_output_3_0_g325 = 1.0;
			float3 temp_cast_17 = (temp_output_3_0_g325).xxx;
			float3 clampResult5_g325 = clamp( desaturateVar4_g325 , temp_cast_16 , temp_cast_17 );
			float3 temp_cast_18 = (temp_output_2_0_g325).xxx;
			float3 lerpResult23_g323 = lerp( lerpResult9_g323 , temp_output_22_0_g323 , ( ( clampResult5_g325 - temp_cast_18 ) / ( temp_output_3_0_g325 - temp_output_2_0_g325 ) ).x);
			float _BlendingPower_Instance = UNITY_ACCESS_INSTANCED_PROP(_BlendingPower_arr, _BlendingPower);
			float4 lerpResult83 = lerp( tex2D( _Diffuse, uv_Diffuse ) , float4( lerpResult23_g323 , 0.0 ) , _BlendingPower_Instance);
			o.Albedo = lerpResult83.rgb;
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			float _EmissivePower_Instance = UNITY_ACCESS_INSTANCED_PROP(_EmissivePower_arr, _EmissivePower);
			o.Emission = ( tex2D( _Emissive, uv_Emissive ) * _EmissivePower_Instance ).rgb;
			float2 uv_Metallic_Smoothness_Mask = i.uv_texcoord * _Metallic_Smoothness_Mask_ST.xy + _Metallic_Smoothness_Mask_ST.zw;
			float4 tex2DNode73 = tex2D( _Metallic_Smoothness_Mask, uv_Metallic_Smoothness_Mask );
			float _MetallicPower_Instance = UNITY_ACCESS_INSTANCED_PROP(_MetallicPower_arr, _MetallicPower);
			o.Metallic = ( tex2DNode73.r * _MetallicPower_Instance );
			float _SmoothnessPower_Instance = UNITY_ACCESS_INSTANCED_PROP(_SmoothnessPower_arr, _SmoothnessPower);
			o.Smoothness = ( tex2DNode73.g * _SmoothnessPower_Instance );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
382.4;92.8;1092;1007;349.9695;521.8081;1.73494;True;True
Node;AmplifyShaderEditor.CommentaryNode;84;-953.4968,345.5578;Float;False;776;954;Custom Reflexion;8;48;66;67;62;64;61;65;72;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;67;-852.7809,1094.558;Float;False;InstancedProperty;_TopReflectionColor2;TopReflectionColor2;9;0;Create;True;0;0;False;0;0.4039216,0.1843137,0,1;0.4039216,0.1843137,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;62;-810.7809,395.5578;Float;False;InstancedProperty;_OffsetX;OffsetX;12;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-815.2809,552.0577;Float;False;InstancedProperty;_ScaleX;ScaleX;14;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-814.2809,633.0577;Float;False;InstancedProperty;_ScaleY;ScaleY;15;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;48;-903.4968,724.1258;Float;True;Property;_TopReflectionMap;TopReflectionMap;7;0;Create;True;0;0;False;0;4e7dff65b6d456c4384a0fdac3a9e8d9;4e7dff65b6d456c4384a0fdac3a9e8d9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;66;-859.7809,926.5576;Float;False;InstancedProperty;_TopReflectionColor1;TopReflectionColor1;8;0;Create;True;0;0;False;0;0.6627451,0.4470589,0.1647059,1;0.6627451,0.4470589,0.1647059,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;61;-809.7809,476.5578;Float;False;InstancedProperty;_OffsetY;OffsetY;13;0;Create;True;0;0;False;0;0.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;72;-535.4971,436.1259;Float;False;SF_StylizedReflective;10;;322;1dcbbf1d6e399ed41acf5d9ca5864a50;0;8;60;FLOAT;0;False;62;FLOAT;0;False;52;FLOAT;0;False;53;FLOAT;0;False;38;SAMPLER2D;;False;35;SAMPLER2D;;False;2;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;91;-186.019,-316.947;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;75;169.7787,833.193;Float;False;InstancedProperty;_SmoothnessPower;SmoothnessPower;6;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;98.62701,-247.7219;Float;False;InstancedProperty;_BlendingPower;BlendingPower;16;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;82;-1.234792,-589.2222;Float;True;Property;_Diffuse;Diffuse;0;0;Create;True;0;0;False;0;None;39cee1e6888e132428f3db162cb50f95;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;77;162.163,662.0538;Float;False;InstancedProperty;_MetallicPower;MetallicPower;5;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;90;-156.9866,-340.3441;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-22.70836,275.1226;Float;False;InstancedProperty;_EmissivePower;EmissivePower;3;0;Create;True;0;0;False;0;0;0;0;5000;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;74;-44.24223,82.32854;Float;True;Property;_Emissive;Emissive;2;0;Create;True;0;0;False;0;None;e3841bdb6607a914188ca6270b974615;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;73;3.236621,476.0197;Float;True;Property;_Metallic_Smoothness_Mask;Metallic_Smoothness_Mask;4;0;Create;True;0;0;False;0;None;b1488ca1a58a4794a9e9e026223a472d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;83;526.7778,-406.5957;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;78;-47.17083,-106.4097;Float;True;Property;_Normal;Normal;1;1;[Normal];Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;612.5349,87.58688;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;646.0974,682.9177;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;651.281,470.5787;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;966.236,-205.0552;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Shader_FresnelTest;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;72;60;62;0
WireConnection;72;62;61;0
WireConnection;72;52;64;0
WireConnection;72;53;65;0
WireConnection;72;35;48;0
WireConnection;72;2;66;0
WireConnection;72;6;67;0
WireConnection;91;0;72;0
WireConnection;90;0;91;0
WireConnection;83;0;82;0
WireConnection;83;1;90;0
WireConnection;83;2;92;0
WireConnection;80;0;74;0
WireConnection;80;1;76;0
WireConnection;81;0;73;2
WireConnection;81;1;75;0
WireConnection;79;0;73;1
WireConnection;79;1;77;0
WireConnection;0;0;83;0
WireConnection;0;1;78;0
WireConnection;0;2;80;0
WireConnection;0;3;79;0
WireConnection;0;4;81;0
ASEEND*/
//CHKSM=71531A4BE04937E45E73B9370FCC5C0B825D527B