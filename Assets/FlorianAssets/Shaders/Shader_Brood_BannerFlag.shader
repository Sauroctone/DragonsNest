// Upgrade NOTE: upgraded instancing buffer 'Shader_BannerFlag' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader_BannerFlag"
{
	Properties
	{
		_Diffuse("Diffuse", 2D) = "white" {}
		_T_Wind_01("T_Wind_01", 2D) = "white" {}
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
		_FakeMetalMask("FakeMetal Mask", 2D) = "white" {}
		_FakeMetalMaskBlending("FakeMetal Mask Blending", Range( 0 , 1)) = 0
		_GradientPosition("GradientPosition", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
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
			float3 worldPos;
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _T_Wind_01;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform sampler2D _TopReflectionMap;
		uniform sampler2D _FakeMetalMask;
		uniform float4 _FakeMetalMask_ST;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform sampler2D _Metallic_Smoothness_Mask;
		uniform float4 _Metallic_Smoothness_Mask_ST;

		UNITY_INSTANCING_BUFFER_START(Shader_BannerFlag)
			UNITY_DEFINE_INSTANCED_PROP(float4, _TopReflectionColor2)
#define _TopReflectionColor2_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float4, _TopReflectionColor1)
#define _TopReflectionColor1_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float, _EmissivePower)
#define _EmissivePower_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float, _FakeMetalMaskBlending)
#define _FakeMetalMaskBlending_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float, _SmoothnessPower)
#define _SmoothnessPower_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float, _MetallicPower)
#define _MetallicPower_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float, _OffsetY)
#define _OffsetY_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float, _ScaleX)
#define _ScaleX_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float, _GradientPosition)
#define _GradientPosition_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float, _OffsetX)
#define _OffsetX_arr Shader_BannerFlag
			UNITY_DEFINE_INSTANCED_PROP(float, _ScaleY)
#define _ScaleY_arr Shader_BannerFlag
		UNITY_INSTANCING_BUFFER_END(Shader_BannerFlag)


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
			float4 _Vector1 = float4(0,1,0,1);
			float3 normalizeResult5_g363 = normalize( (_Vector1).xyz );
			float3 temp_cast_0 = (3.0).xxx;
			float3 temp_output_9_0_g363 = ( normalizeResult5_g363 * ( (_Vector1).w * ( ( _Time.y * 1.0 ) * -0.5 ) ) );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 temp_output_47_0_g363 = abs( ( ( frac( ( ( temp_output_9_0_g363 + ( ase_worldPos / 1024.0 ) ) + 0.5 ) ) * 2.0 ) + -1.0 ) );
			float dotResult52_g363 = dot( normalizeResult5_g363 , ( ( ( temp_cast_0 - ( temp_output_47_0_g363 * 2.0 ) ) * temp_output_47_0_g363 ) * temp_output_47_0_g363 ) );
			float3 temp_cast_1 = (3.0).xxx;
			float3 temp_output_31_0_g363 = abs( ( ( frac( ( ( temp_output_9_0_g363 + ( ase_worldPos / 200.0 ) ) + 0.5 ) ) * 2.0 ) + -1.0 ) );
			float3 temp_cast_2 = (0.0).xxx;
			float2 panner8_g361 = ( 1.0 * _Time.y * (float3( 1,1,100 )).xz + (( ase_worldPos / 1000.0 )).xz);
			float3 temp_output_61_0_g363 = ( tex2Dlod( _T_Wind_01, float4( panner8_g361, 0, 0.0) ) * float4( float3( 0.2,0.2,0.2 ) , 0.0 ) ).rgb;
			float3 rotatedValue62_g363 = RotateAroundAxis( ( temp_output_61_0_g363 + float3(0,0,-10) ), temp_output_61_0_g363, cross( normalizeResult5_g363 , float3(0,0,1) ), ( dotResult52_g363 + distance( ( ( ( temp_cast_1 - ( temp_output_31_0_g363 * 2.0 ) ) * temp_output_31_0_g363 ) * temp_output_31_0_g363 ) , temp_cast_2 ) ) );
			float _GradientPosition_Instance = UNITY_ACCESS_INSTANCED_PROP(_GradientPosition_arr, _GradientPosition);
			float temp_output_31_0_g361 = pow( ( 1.0 - v.texcoord.xy.y ) , _GradientPosition_Instance );
			float3 lerpResult35_g361 = lerp( float3( 0,0,0 ) , ( ( ( rotatedValue62_g363 * -0.2 ) * 0.3 ) + temp_output_61_0_g363 ) , temp_output_31_0_g361);
			v.vertex.xyz += lerpResult35_g361;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float4 _TopReflectionColor1_Instance = UNITY_ACCESS_INSTANCED_PROP(_TopReflectionColor1_arr, _TopReflectionColor1);
			float3 temp_output_10_0_g334 = _TopReflectionColor1_Instance.rgb;
			float4 _TopReflectionColor2_Instance = UNITY_ACCESS_INSTANCED_PROP(_TopReflectionColor2_arr, _TopReflectionColor2);
			float3 temp_output_11_0_g334 = _TopReflectionColor2_Instance.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 ase_tanViewDir = mul( ase_worldToTangent, ase_worldViewDir );
			float3 worldToObjDir16_g333 = mul( unity_WorldToObject, float4( ( ase_tanViewDir - float3(0,1,0) ), 0 ) ).xyz;
			float2 appendResult9_g333 = (float2(worldToObjDir16_g333.x , ( worldToObjDir16_g333.y * -1.0 )));
			float _ScaleX_Instance = UNITY_ACCESS_INSTANCED_PROP(_ScaleX_arr, _ScaleX);
			float4 temp_cast_2 = (_ScaleX_Instance).xxxx;
			float _ScaleY_Instance = UNITY_ACCESS_INSTANCED_PROP(_ScaleY_arr, _ScaleY);
			float4 temp_cast_4 = (_ScaleY_Instance).xxxx;
			float2 appendResult23_g333 = (float2(temp_cast_2.x , temp_cast_4.x));
			float _OffsetX_Instance = UNITY_ACCESS_INSTANCED_PROP(_OffsetX_arr, _OffsetX);
			float4 temp_cast_6 = (_OffsetX_Instance).xxxx;
			float _OffsetY_Instance = UNITY_ACCESS_INSTANCED_PROP(_OffsetY_arr, _OffsetY);
			float4 temp_cast_8 = (_OffsetY_Instance).xxxx;
			float2 appendResult25_g333 = (float2(temp_cast_6.x , temp_cast_8.x));
			float3 clampResult2_g334 = clamp( ( 1.0 - tex2D( _TopReflectionMap, ( ( ( ( appendResult9_g333 * 0.5 ) + 0.5 ) * appendResult23_g333 ) + appendResult25_g333 ) ) ).rgb , float3( 0,0,0 ) , float3( 1,0,0 ) );
			float3 desaturateInitialColor4_g338 = clampResult2_g334;
			float desaturateDot4_g338 = dot( desaturateInitialColor4_g338, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar4_g338 = lerp( desaturateInitialColor4_g338, desaturateDot4_g338.xxx, 0.0 );
			float temp_output_2_0_g338 = 0.0;
			float3 temp_cast_11 = (temp_output_2_0_g338).xxx;
			float temp_output_3_0_g338 = 0.5;
			float3 temp_cast_12 = (temp_output_3_0_g338).xxx;
			float3 clampResult5_g338 = clamp( desaturateVar4_g338 , temp_cast_11 , temp_cast_12 );
			float3 temp_cast_13 = (temp_output_2_0_g338).xxx;
			float3 lerpResult9_g334 = lerp( temp_output_10_0_g334 , temp_output_11_0_g334 , ( ( clampResult5_g338 - temp_cast_13 ) / ( temp_output_3_0_g338 - temp_output_2_0_g338 ) ).x);
			float3 temp_output_22_0_g334 = float4(0,0,0,1).rgb;
			float3 desaturateInitialColor4_g336 = clampResult2_g334;
			float desaturateDot4_g336 = dot( desaturateInitialColor4_g336, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar4_g336 = lerp( desaturateInitialColor4_g336, desaturateDot4_g336.xxx, 0.0 );
			float temp_output_2_0_g336 = 0.5;
			float3 temp_cast_16 = (temp_output_2_0_g336).xxx;
			float temp_output_3_0_g336 = 1.0;
			float3 temp_cast_17 = (temp_output_3_0_g336).xxx;
			float3 clampResult5_g336 = clamp( desaturateVar4_g336 , temp_cast_16 , temp_cast_17 );
			float3 temp_cast_18 = (temp_output_2_0_g336).xxx;
			float3 lerpResult23_g334 = lerp( lerpResult9_g334 , temp_output_22_0_g334 , ( ( clampResult5_g336 - temp_cast_18 ) / ( temp_output_3_0_g336 - temp_output_2_0_g336 ) ).x);
			float2 uv_FakeMetalMask = i.uv_texcoord * _FakeMetalMask_ST.xy + _FakeMetalMask_ST.zw;
			float _FakeMetalMaskBlending_Instance = UNITY_ACCESS_INSTANCED_PROP(_FakeMetalMaskBlending_arr, _FakeMetalMaskBlending);
			float4 lerpResult108 = lerp( tex2D( _FakeMetalMask, uv_FakeMetalMask ) , float4( 1,1,1,0 ) , _FakeMetalMaskBlending_Instance);
			float4 lerpResult83 = lerp( tex2D( _Diffuse, uv_Diffuse ) , float4( lerpResult23_g334 , 0.0 ) , lerpResult108.r);
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
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				vertexDataFunc( v, customInputData );
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
482.4;100;726;994;-161.5869;-986.0424;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;84;-953.4968,345.5578;Float;False;776;954;Custom Reflexion;8;48;66;67;62;64;61;65;94;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-815.2809,552.0577;Float;False;InstancedProperty;_ScaleX;ScaleX;16;0;Create;True;0;0;False;0;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-810.7809,395.5578;Float;False;InstancedProperty;_OffsetX;OffsetX;14;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;67;-852.7809,1094.558;Float;False;InstancedProperty;_TopReflectionColor2;TopReflectionColor2;11;0;Create;True;0;0;False;0;0.4039216,0.1843137,0,1;0.4039215,0.1843136,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;66;-859.7809,926.5576;Float;False;InstancedProperty;_TopReflectionColor1;TopReflectionColor1;10;0;Create;True;0;0;False;0;0.6627451,0.4470589,0.1647059,1;0.6627451,0.4470589,0.1647058,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;48;-903.4968,724.1258;Float;True;Property;_TopReflectionMap;TopReflectionMap;9;0;Create;True;0;0;False;0;4e7dff65b6d456c4384a0fdac3a9e8d9;4e7dff65b6d456c4384a0fdac3a9e8d9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-814.2809,633.0577;Float;False;InstancedProperty;_ScaleY;ScaleY;17;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-809.7809,476.5578;Float;False;InstancedProperty;_OffsetY;OffsetY;15;0;Create;True;0;0;False;0;0.7;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;94;-535.4971,436.1259;Float;False;SF_StylizedReflective;12;;331;1dcbbf1d6e399ed41acf5d9ca5864a50;0;8;60;FLOAT;0;False;62;FLOAT;0;False;52;FLOAT;0;False;53;FLOAT;0;False;38;SAMPLER2D;;False;35;SAMPLER2D;;False;2;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;93;-177.1329,449.3443;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;96;39.20993,-305.4144;Float;True;Property;_FakeMetalMask;FakeMetal Mask;18;0;Create;True;0;0;False;0;420b7f08fdd56094faded847f16348f9;420b7f08fdd56094faded847f16348f9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;91;-186.019,-316.947;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;109;163.815,-112.4643;Float;False;InstancedProperty;_FakeMetalMaskBlending;FakeMetal Mask Blending;19;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;90;-156.9866,-340.3441;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-22.70836,275.1226;Float;False;InstancedProperty;_EmissivePower;EmissivePower;5;0;Create;True;0;0;False;0;0;0;0;5000;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;74;-44.24223,82.32854;Float;True;Property;_Emissive;Emissive;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;75;169.7787,833.193;Float;False;InstancedProperty;_SmoothnessPower;SmoothnessPower;8;0;Create;True;0;0;False;0;1;1.2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;73;3.236621,476.0197;Float;True;Property;_Metallic_Smoothness_Mask;Metallic_Smoothness_Mask;6;0;Create;True;0;0;False;0;None;14a425d1382a31a4d8f1d60840b7e449;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;82;-1.234792,-589.2222;Float;True;Property;_Diffuse;Diffuse;0;0;Create;True;0;0;False;0;None;4ece7407f3fbc2346842010123bdf3b0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;77;162.163,662.0538;Float;False;InstancedProperty;_MetallicPower;MetallicPower;7;0;Create;True;0;0;False;0;1;1.2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;108;370.3154,-299.9641;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;110;479.0561,1514.361;Float;False;InstancedProperty;_GradientPosition;GradientPosition;20;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;78;-47.17083,-106.4097;Float;True;Property;_Normal;Normal;3;1;[Normal];Create;True;0;0;False;0;None;6c33609dbd86a3d4f970e996ad648cd7;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;105;672.707,1507.069;Float;True;SF_BannerWind;1;;361;7164ce9717420df43b429e32fe46db4b;0;7;40;FLOAT3;1,1,100;False;32;FLOAT;2;False;21;FLOAT;-0.2;False;23;FLOAT;0.3;False;17;SAMPLER2D;;False;19;FLOAT3;0.2,0.2,0.2;False;4;FLOAT;1000;False;2;FLOAT;37;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;651.281,470.5787;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;612.5349,87.58688;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;83;695.7778,-473.5957;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;646.0974,682.9177;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;966.236,-205.0552;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Shader_BannerFlag;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;94;60;62;0
WireConnection;94;62;61;0
WireConnection;94;52;64;0
WireConnection;94;53;65;0
WireConnection;94;35;48;0
WireConnection;94;2;66;0
WireConnection;94;6;67;0
WireConnection;93;0;94;0
WireConnection;91;0;93;0
WireConnection;90;0;91;0
WireConnection;108;0;96;0
WireConnection;108;2;109;0
WireConnection;105;32;110;0
WireConnection;79;0;73;1
WireConnection;79;1;77;0
WireConnection;80;0;74;0
WireConnection;80;1;76;0
WireConnection;83;0;82;0
WireConnection;83;1;90;0
WireConnection;83;2;108;0
WireConnection;81;0;73;2
WireConnection;81;1;75;0
WireConnection;0;0;83;0
WireConnection;0;1;78;0
WireConnection;0;2;80;0
WireConnection;0;3;79;0
WireConnection;0;4;81;0
WireConnection;0;11;105;0
ASEEND*/
//CHKSM=0C84A61CA08F6F9C2DF3ED9D24D3F5D2387929BF