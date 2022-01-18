Shader "Oculus/OVRMRCameraFrameLit" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DepthTex ("Depth (cm)", 2D) = "black" {}
		_InconfidenceTex ("Inconfidence (0-100)", 2D) = "black" {}
		_Visible ("Visible", Range(0, 1)) = 1
	}
	SubShader {
		LOD 200
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Name "FORWARD"
			LOD 200
			Tags { "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ColorMask RGB -1
			ZWrite Off
			GpuProgramID 18302
			Program "vp" {
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_1_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD7;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat0.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    vs_TEXCOORD0.w = u_xlat0.x;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat3.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat0.x = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.w = u_xlat0.y;
					    vs_TEXCOORD2.w = u_xlat0.z;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
					    u_xlat0.w = u_xlat0.x * 0.5;
					    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD3.zw = u_xlat1.zw;
					    vs_TEXCOORD3.xy = u_xlat0.zz + u_xlat0.xw;
					    vs_TEXCOORD7 = vec4(0.0, 0.0, 0.0, 0.0);
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityLighting {
						vec4 unused_1_0[42];
						vec4 unity_SHBr;
						vec4 unity_SHBg;
						vec4 unity_SHBb;
						vec4 unity_SHC;
						vec4 unused_1_5[2];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_2_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_2_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec3 vs_TEXCOORD4;
					out vec4 vs_TEXCOORD7;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat4;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat0.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    vs_TEXCOORD0.w = u_xlat0.x;
					    u_xlat2.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat2.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat2.xyz;
					    u_xlat2.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat2.xyz;
					    u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat2.xyz;
					    vs_TEXCOORD0.x = u_xlat2.z;
					    u_xlat3.x = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat3.y = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat3.z = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat3 = u_xlat0.xxxx * u_xlat3.xyzz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.wxy;
					    u_xlat4.xyz = u_xlat3.ywx * u_xlat2.yzx + (-u_xlat4.xyz);
					    u_xlat0.x = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.z = u_xlat3.x;
					    vs_TEXCOORD1.x = u_xlat2.x;
					    vs_TEXCOORD2.x = u_xlat2.y;
					    vs_TEXCOORD1.w = u_xlat0.y;
					    vs_TEXCOORD2.w = u_xlat0.z;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    vs_TEXCOORD1.z = u_xlat3.y;
					    vs_TEXCOORD2.z = u_xlat3.w;
					    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
					    u_xlat0.w = u_xlat0.x * 0.5;
					    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD3.zw = u_xlat1.zw;
					    vs_TEXCOORD3.xy = u_xlat0.zz + u_xlat0.xw;
					    u_xlat0.x = u_xlat3.y * u_xlat3.y;
					    u_xlat0.x = u_xlat3.x * u_xlat3.x + (-u_xlat0.x);
					    u_xlat1 = u_xlat3.ywzx * u_xlat3;
					    u_xlat2.x = dot(unity_SHBr, u_xlat1);
					    u_xlat2.y = dot(unity_SHBg, u_xlat1);
					    u_xlat2.z = dot(unity_SHBb, u_xlat1);
					    vs_TEXCOORD4.xyz = unity_SHC.xyz * u_xlat0.xxx + u_xlat2.xyz;
					    vs_TEXCOORD7 = vec4(0.0, 0.0, 0.0, 0.0);
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "DIRLIGHTMAP_COMBINED" "DYNAMICLIGHTMAP_ON" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_1_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					layout(std140) uniform UnityLightmaps {
						vec4 unused_3_0;
						vec4 unity_DynamicLightmapST;
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD2;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD7;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat0.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    vs_TEXCOORD0.w = u_xlat0.x;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat3.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat0.x = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.w = u_xlat0.y;
					    vs_TEXCOORD2.w = u_xlat0.z;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
					    u_xlat0.w = u_xlat0.x * 0.5;
					    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD3.zw = u_xlat1.zw;
					    vs_TEXCOORD3.xy = u_xlat0.zz + u_xlat0.xw;
					    vs_TEXCOORD7.zw = in_TEXCOORD2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
					    vs_TEXCOORD7.xy = vec2(0.0, 0.0);
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "DIRLIGHTMAP_COMBINED" "LIGHTMAP_ON" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_1_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					layout(std140) uniform UnityLightmaps {
						vec4 unity_LightmapST;
						vec4 unused_3_1;
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD1;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD4;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat0.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    vs_TEXCOORD0.w = u_xlat0.x;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat3.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat0.x = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.w = u_xlat0.y;
					    vs_TEXCOORD2.w = u_xlat0.z;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
					    u_xlat0.w = u_xlat0.x * 0.5;
					    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD3.zw = u_xlat1.zw;
					    vs_TEXCOORD3.xy = u_xlat0.zz + u_xlat0.xw;
					    vs_TEXCOORD4.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					    vs_TEXCOORD4.zw = vec2(0.0, 0.0);
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "DIRLIGHTMAP_COMBINED" "LIGHTMAP_ON" "LIGHTPROBE_SH" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_1_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					layout(std140) uniform UnityLightmaps {
						vec4 unity_LightmapST;
						vec4 unused_3_1;
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD1;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD4;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat0.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    vs_TEXCOORD0.w = u_xlat0.x;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat3.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat0.x = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.w = u_xlat0.y;
					    vs_TEXCOORD2.w = u_xlat0.z;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
					    u_xlat0.w = u_xlat0.x * 0.5;
					    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD3.zw = u_xlat1.zw;
					    vs_TEXCOORD3.xy = u_xlat0.zz + u_xlat0.xw;
					    vs_TEXCOORD4.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					    vs_TEXCOORD4.zw = vec2(0.0, 0.0);
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "DIRLIGHTMAP_COMBINED" "DYNAMICLIGHTMAP_ON" "LIGHTMAP_ON" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_1_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					layout(std140) uniform UnityLightmaps {
						vec4 unity_LightmapST;
						vec4 unity_DynamicLightmapST;
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD1;
					in  vec4 in_TEXCOORD2;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD4;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat0.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    vs_TEXCOORD0.w = u_xlat0.x;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat3.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat0.x = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.w = u_xlat0.y;
					    vs_TEXCOORD2.w = u_xlat0.z;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
					    u_xlat0.w = u_xlat0.x * 0.5;
					    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD3.zw = u_xlat1.zw;
					    vs_TEXCOORD3.xy = u_xlat0.zz + u_xlat0.xw;
					    vs_TEXCOORD4.zw = in_TEXCOORD2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
					    vs_TEXCOORD4.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityLighting {
						vec4 unused_1_0[3];
						vec4 unity_4LightPosX0;
						vec4 unity_4LightPosY0;
						vec4 unity_4LightPosZ0;
						vec4 unity_4LightAtten0;
						vec4 unity_LightColor[8];
						vec4 unused_1_6[34];
						vec4 unity_SHBr;
						vec4 unity_SHBg;
						vec4 unity_SHBb;
						vec4 unity_SHC;
						vec4 unused_1_11[2];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_2_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_2_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec3 vs_TEXCOORD4;
					out vec4 vs_TEXCOORD7;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					float u_xlat15;
					float u_xlat17;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat0.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    u_xlat2.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat2.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat2.xyz;
					    u_xlat2.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat2.xyz;
					    u_xlat15 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat15 = inversesqrt(u_xlat15);
					    u_xlat2.xyz = vec3(u_xlat15) * u_xlat2.xyz;
					    vs_TEXCOORD0.x = u_xlat2.z;
					    u_xlat15 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat3.x = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat3.y = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat3.z = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat17 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat17 = inversesqrt(u_xlat17);
					    u_xlat3 = vec4(u_xlat17) * u_xlat3.xyzz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.wxy;
					    u_xlat4.xyz = u_xlat3.ywx * u_xlat2.yzx + (-u_xlat4.xyz);
					    u_xlat4.xyz = vec3(u_xlat15) * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.w = u_xlat0.x;
					    vs_TEXCOORD0.z = u_xlat3.x;
					    vs_TEXCOORD1.x = u_xlat2.x;
					    vs_TEXCOORD2.x = u_xlat2.y;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    vs_TEXCOORD1.w = u_xlat0.y;
					    vs_TEXCOORD1.z = u_xlat3.y;
					    vs_TEXCOORD2.w = u_xlat0.z;
					    vs_TEXCOORD2.z = u_xlat3.w;
					    u_xlat15 = u_xlat1.y * _ProjectionParams.x;
					    u_xlat2.w = u_xlat15 * 0.5;
					    u_xlat2.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD3.zw = u_xlat1.zw;
					    vs_TEXCOORD3.xy = u_xlat2.zz + u_xlat2.xw;
					    u_xlat1 = (-u_xlat0.xxxx) + unity_4LightPosX0;
					    u_xlat2 = (-u_xlat0.yyyy) + unity_4LightPosY0;
					    u_xlat0 = (-u_xlat0.zzzz) + unity_4LightPosZ0;
					    u_xlat4 = u_xlat3.yyyy * u_xlat2;
					    u_xlat2 = u_xlat2 * u_xlat2;
					    u_xlat2 = u_xlat1 * u_xlat1 + u_xlat2;
					    u_xlat1 = u_xlat1 * u_xlat3.xxxx + u_xlat4;
					    u_xlat1 = u_xlat0 * u_xlat3.wwzw + u_xlat1;
					    u_xlat0 = u_xlat0 * u_xlat0 + u_xlat2;
					    u_xlat0 = max(u_xlat0, vec4(9.99999997e-07, 9.99999997e-07, 9.99999997e-07, 9.99999997e-07));
					    u_xlat2 = inversesqrt(u_xlat0);
					    u_xlat0 = u_xlat0 * unity_4LightAtten0 + vec4(1.0, 1.0, 1.0, 1.0);
					    u_xlat0 = vec4(1.0, 1.0, 1.0, 1.0) / u_xlat0;
					    u_xlat1 = u_xlat1 * u_xlat2;
					    u_xlat1 = max(u_xlat1, vec4(0.0, 0.0, 0.0, 0.0));
					    u_xlat0 = u_xlat0 * u_xlat1;
					    u_xlat1.xyz = u_xlat0.yyy * unity_LightColor[1].xyz;
					    u_xlat1.xyz = unity_LightColor[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
					    u_xlat0.xyz = unity_LightColor[2].xyz * u_xlat0.zzz + u_xlat1.xyz;
					    u_xlat0.xyz = unity_LightColor[3].xyz * u_xlat0.www + u_xlat0.xyz;
					    u_xlat1.xyz = u_xlat0.xyz * vec3(0.305306017, 0.305306017, 0.305306017) + vec3(0.682171106, 0.682171106, 0.682171106);
					    u_xlat1.xyz = u_xlat0.xyz * u_xlat1.xyz + vec3(0.0125228781, 0.0125228781, 0.0125228781);
					    u_xlat15 = u_xlat3.y * u_xlat3.y;
					    u_xlat15 = u_xlat3.x * u_xlat3.x + (-u_xlat15);
					    u_xlat2 = u_xlat3.ywzx * u_xlat3;
					    u_xlat3.x = dot(unity_SHBr, u_xlat2);
					    u_xlat3.y = dot(unity_SHBg, u_xlat2);
					    u_xlat3.z = dot(unity_SHBb, u_xlat2);
					    u_xlat2.xyz = unity_SHC.xyz * vec3(u_xlat15) + u_xlat3.xyz;
					    vs_TEXCOORD4.xyz = u_xlat0.xyz * u_xlat1.xyz + u_xlat2.xyz;
					    vs_TEXCOORD7 = vec4(0.0, 0.0, 0.0, 0.0);
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "DIRLIGHTMAP_COMBINED" "DYNAMICLIGHTMAP_ON" "VERTEXLIGHT_ON" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_1_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					layout(std140) uniform UnityLightmaps {
						vec4 unused_3_0;
						vec4 unity_DynamicLightmapST;
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD2;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD7;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat0.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    vs_TEXCOORD0.w = u_xlat0.x;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat3.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat0.x = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.w = u_xlat0.y;
					    vs_TEXCOORD2.w = u_xlat0.z;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
					    u_xlat0.w = u_xlat0.x * 0.5;
					    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD3.zw = u_xlat1.zw;
					    vs_TEXCOORD3.xy = u_xlat0.zz + u_xlat0.xw;
					    vs_TEXCOORD7.zw = in_TEXCOORD2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
					    vs_TEXCOORD7.xy = vec2(0.0, 0.0);
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _LightColor0;
						vec4 unused_0_2;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[45];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_1_3;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec4 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					vec3 u_xlat10;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					float u_xlat19;
					int u_xlati19;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD3.yx / vs_TEXCOORD3.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlatb18 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb18){
					        u_xlatb18 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat3.xyz = vs_TEXCOORD1.www * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD0.www + u_xlat3.xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD2.www + u_xlat3.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat10.x = vs_TEXCOORD0.w;
					        u_xlat10.y = vs_TEXCOORD1.w;
					        u_xlat10.z = vs_TEXCOORD2.w;
					        u_xlat3.xyz = (bool(u_xlatb18)) ? u_xlat3.xyz : u_xlat10.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat3.yzw = u_xlat3.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat18 = u_xlat3.y * 0.25 + 0.75;
					        u_xlat19 = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat3.x = max(u_xlat18, u_xlat19);
					        u_xlat3 = texture(unity_ProbeVolumeSH, u_xlat3.xzw);
					    } else {
					        u_xlat3.x = float(1.0);
					        u_xlat3.y = float(1.0);
					        u_xlat3.z = float(1.0);
					        u_xlat3.w = float(1.0);
					    }
					    u_xlat18 = dot(u_xlat3, unity_OcclusionMaskSelector);
					    u_xlat18 = clamp(u_xlat18, 0.0, 1.0);
					    u_xlat3.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat3.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat3.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat3.xyz = vec3(u_xlat18) * _LightColor0.xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat6.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _LightColor0;
						vec4 unused_0_2;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[38];
						vec4 unity_SHAr;
						vec4 unity_SHAg;
						vec4 unity_SHAb;
						vec4 unused_1_5[4];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_1_7;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec4 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					in  vec3 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec4 u_xlat6;
					float u_xlat7;
					vec2 u_xlat9;
					float u_xlat10;
					vec3 u_xlat11;
					vec3 u_xlat12;
					vec2 u_xlat14;
					bvec2 u_xlatb14;
					float u_xlat15;
					bool u_xlatb15;
					vec2 u_xlat16;
					bool u_xlatb16;
					float u_xlat21;
					int u_xlati21;
					bool u_xlatb21;
					float u_xlat22;
					int u_xlati22;
					bool u_xlatb22;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD3.yx / vs_TEXCOORD3.ww;
					    u_xlatb14.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb14.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat14.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb14.y) ? u_xlat14.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb14.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb14.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat14.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat14.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat14.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat14.x;
					    u_xlat14.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat14.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat14.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat14.x;
					    u_xlat14.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat15 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat15 + -1.0;
					        u_xlat15 = u_xlat14.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat16.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat16.x + -1.0;
					            u_xlat9.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat9.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat9.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat16.x = u_xlat3.y * 0.336089998;
					            u_xlat16.x = u_xlat3.x * -0.0999099985 + (-u_xlat16.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat16.x;
					            u_xlat16.x = u_xlat3.y * 0.558610022;
					            u_xlat16.x = u_xlat3.w * 0.61500001 + (-u_xlat16.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat16.x;
					            u_xlat16.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat16.x = dot(u_xlat16.xy, u_xlat16.xy);
					            u_xlat16.x = sqrt(u_xlat16.x);
					            u_xlat9.x = u_xlat9.x + -0.899999976;
					            u_xlat9.x = clamp(u_xlat9.x, 0.0, 1.0);
					            u_xlat9.x = u_xlat9.x + u_xlat16.x;
					            u_xlat15 = u_xlat15 + u_xlat9.x;
					        }
					        u_xlat14.x = u_xlat15;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat21 = u_xlat1.x * 0.00999999978;
					    u_xlatb21 = _CullingDistance<u_xlat21;
					    if(((int(u_xlatb21) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat14.x = u_xlat14.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat14.xy = u_xlat14.xx * u_xlat3.xy;
					    u_xlat14.xy = clamp(u_xlat14.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat14.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat14.xy = u_xlat14.xy * u_xlat14.xy;
					    u_xlat14.xy = u_xlat14.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat14.xy * u_xlat14.xy;
					    u_xlat14.x = u_xlat14.y * u_xlat3.y;
					    u_xlat21 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat21));
					    u_xlat1.xyz = u_xlat14.xxx * u_xlat1.xyz + vec3(u_xlat21);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat7 = u_xlat4.x * 0.00999999978;
					    u_xlat7 = u_xlat2.x * 0.00999999978 + (-u_xlat7);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat7, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat21 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat21 = inversesqrt(u_xlat21);
					    u_xlat0.xyz = vec3(u_xlat21) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlatb21 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb21){
					        u_xlatb22 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat3.xyz = vs_TEXCOORD1.www * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD0.www + u_xlat3.xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD2.www + u_xlat3.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat11.x = vs_TEXCOORD0.w;
					        u_xlat11.y = vs_TEXCOORD1.w;
					        u_xlat11.z = vs_TEXCOORD2.w;
					        u_xlat3.xyz = (bool(u_xlatb22)) ? u_xlat3.xyz : u_xlat11.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat3.yzw = u_xlat3.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat22 = u_xlat3.y * 0.25 + 0.75;
					        u_xlat10 = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat3.x = max(u_xlat22, u_xlat10);
					        u_xlat3 = texture(unity_ProbeVolumeSH, u_xlat3.xzw);
					    } else {
					        u_xlat3.x = float(1.0);
					        u_xlat3.y = float(1.0);
					        u_xlat3.z = float(1.0);
					        u_xlat3.w = float(1.0);
					    }
					    u_xlat22 = dot(u_xlat3, unity_OcclusionMaskSelector);
					    u_xlat22 = clamp(u_xlat22, 0.0, 1.0);
					    u_xlat3.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat3.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat3.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat3.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat0.xyz = vec3(u_xlat22) * _LightColor0.xyz;
					    if(u_xlatb21){
					        u_xlatb21 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat4.xyz = vs_TEXCOORD1.www * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat4.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD0.www + u_xlat4.xyz;
					        u_xlat4.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD2.www + u_xlat4.xyz;
					        u_xlat4.xyz = u_xlat4.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat12.x = vs_TEXCOORD0.w;
					        u_xlat12.y = vs_TEXCOORD1.w;
					        u_xlat12.z = vs_TEXCOORD2.w;
					        u_xlat4.xyz = (bool(u_xlatb21)) ? u_xlat4.xyz : u_xlat12.xyz;
					        u_xlat4.xyz = u_xlat4.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat4.yzw = u_xlat4.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat21 = u_xlat4.y * 0.25;
					        u_xlat22 = unity_ProbeVolumeParams.z * 0.5;
					        u_xlat11.x = (-unity_ProbeVolumeParams.z) * 0.5 + 0.25;
					        u_xlat21 = max(u_xlat21, u_xlat22);
					        u_xlat4.x = min(u_xlat11.x, u_xlat21);
					        u_xlat5 = texture(unity_ProbeVolumeSH, u_xlat4.xzw);
					        u_xlat6.xyz = u_xlat4.xzw + vec3(0.25, 0.0, 0.0);
					        u_xlat6 = texture(unity_ProbeVolumeSH, u_xlat6.xyz);
					        u_xlat4.xyz = u_xlat4.xzw + vec3(0.5, 0.0, 0.0);
					        u_xlat4 = texture(unity_ProbeVolumeSH, u_xlat4.xyz);
					        u_xlat3.w = 1.0;
					        u_xlat5.x = dot(u_xlat5, u_xlat3);
					        u_xlat5.y = dot(u_xlat6, u_xlat3);
					        u_xlat5.z = dot(u_xlat4, u_xlat3);
					    } else {
					        u_xlat3.w = 1.0;
					        u_xlat5.x = dot(unity_SHAr, u_xlat3);
					        u_xlat5.y = dot(unity_SHAg, u_xlat3);
					        u_xlat5.z = dot(unity_SHAb, u_xlat3);
					    }
					    u_xlat4.xyz = u_xlat5.xyz + vs_TEXCOORD4.xyz;
					    u_xlat4.xyz = max(u_xlat4.xyz, vec3(0.0, 0.0, 0.0));
					    u_xlat4.xyz = log2(u_xlat4.xyz);
					    u_xlat4.xyz = u_xlat4.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
					    u_xlat4.xyz = exp2(u_xlat4.xyz);
					    u_xlat4.xyz = u_xlat4.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
					    u_xlat4.xyz = max(u_xlat4.xyz, vec3(0.0, 0.0, 0.0));
					    u_xlat21 = dot(u_xlat3.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat21 = max(u_xlat21, 0.0);
					    u_xlat0.xyz = u_xlat0.xyz * u_xlat1.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * u_xlat4.xyz;
					    u_xlat2.xyz = u_xlat0.xyz * vec3(u_xlat21) + u_xlat1.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "DIRLIGHTMAP_COMBINED" "DYNAMICLIGHTMAP_ON" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0;
						vec4 unity_DynamicLightmap_HDR;
						vec4 _LightColor0;
						vec4 unused_0_3;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[45];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_1_3;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler2D unity_DynamicLightmap;
					uniform  sampler2D unity_DynamicDirectionality;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec4 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD7;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					vec3 u_xlat10;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					float u_xlat19;
					int u_xlati19;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD3.yx / vs_TEXCOORD3.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlatb18 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb18){
					        u_xlatb18 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat3.xyz = vs_TEXCOORD1.www * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD0.www + u_xlat3.xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD2.www + u_xlat3.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat10.x = vs_TEXCOORD0.w;
					        u_xlat10.y = vs_TEXCOORD1.w;
					        u_xlat10.z = vs_TEXCOORD2.w;
					        u_xlat3.xyz = (bool(u_xlatb18)) ? u_xlat3.xyz : u_xlat10.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat3.yzw = u_xlat3.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat18 = u_xlat3.y * 0.25 + 0.75;
					        u_xlat19 = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat3.x = max(u_xlat18, u_xlat19);
					        u_xlat3 = texture(unity_ProbeVolumeSH, u_xlat3.xzw);
					    } else {
					        u_xlat3.x = float(1.0);
					        u_xlat3.y = float(1.0);
					        u_xlat3.z = float(1.0);
					        u_xlat3.w = float(1.0);
					    }
					    u_xlat18 = dot(u_xlat3, unity_OcclusionMaskSelector);
					    u_xlat18 = clamp(u_xlat18, 0.0, 1.0);
					    u_xlat3.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat3.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat3.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat3.xyz = vec3(u_xlat18) * _LightColor0.xyz;
					    u_xlat4 = texture(unity_DynamicLightmap, vs_TEXCOORD7.zw);
					    u_xlat18 = u_xlat4.w * unity_DynamicLightmap_HDR.x;
					    u_xlat4.xyz = u_xlat4.xyz * vec3(u_xlat18);
					    u_xlat4.xyz = log2(u_xlat4.xyz);
					    u_xlat4.xyz = u_xlat4.xyz * unity_DynamicLightmap_HDR.yyy;
					    u_xlat4.xyz = exp2(u_xlat4.xyz);
					    u_xlat5 = texture(unity_DynamicDirectionality, vs_TEXCOORD7.zw);
					    u_xlat5.xyz = u_xlat5.xyz + vec3(-0.5, -0.5, -0.5);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat5.xyz);
					    u_xlat18 = u_xlat18 + 0.5;
					    u_xlat4.xyz = vec3(u_xlat18) * u_xlat4.xyz;
					    u_xlat18 = max(u_xlat5.w, 9.99999975e-05);
					    u_xlat4.xyz = u_xlat4.xyz / vec3(u_xlat18);
					    u_xlat0.x = dot(u_xlat0.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * u_xlat4.xyz;
					    u_xlat2.xyz = u_xlat6.xyz * u_xlat0.xxx + u_xlat1.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "DIRLIGHTMAP_COMBINED" "LIGHTMAP_ON" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unity_Lightmap_HDR;
						vec4 unused_0_1;
						vec4 _LightColor0;
						vec4 unused_0_3;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[47];
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler2D unity_Lightmap;
					uniform  sampler2D unity_LightmapInd;
					in  vec4 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					int u_xlati19;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD3.yx / vs_TEXCOORD3.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlat3.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat3.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat3.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat3 = texture(unity_Lightmap, vs_TEXCOORD4.xy);
					    u_xlat18 = u_xlat3.w * unity_Lightmap_HDR.x;
					    u_xlat3.xyz = u_xlat3.xyz * vec3(u_xlat18);
					    u_xlat4 = texture(unity_LightmapInd, vs_TEXCOORD4.xy);
					    u_xlat4.xyz = u_xlat4.xyz + vec3(-0.5, -0.5, -0.5);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat4.xyz);
					    u_xlat18 = u_xlat18 + 0.5;
					    u_xlat3.xyz = vec3(u_xlat18) * u_xlat3.xyz;
					    u_xlat18 = max(u_xlat4.w, 9.99999975e-05);
					    u_xlat3.xyz = u_xlat3.xyz / vec3(u_xlat18);
					    u_xlat0.x = dot(u_xlat0.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * _LightColor0.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat2.xyz = u_xlat6.xyz * u_xlat0.xxx + u_xlat1.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "DIRLIGHTMAP_COMBINED" "LIGHTMAP_ON" "LIGHTPROBE_SH" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unity_Lightmap_HDR;
						vec4 unused_0_1;
						vec4 _LightColor0;
						vec4 unused_0_3;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[38];
						vec4 unity_SHAr;
						vec4 unity_SHAg;
						vec4 unity_SHAb;
						vec4 unity_SHBr;
						vec4 unity_SHBg;
						vec4 unity_SHBb;
						vec4 unity_SHC;
						vec4 unused_1_9[2];
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler2D unity_Lightmap;
					uniform  sampler2D unity_LightmapInd;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec4 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					float u_xlat9;
					vec3 u_xlat10;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					float u_xlat19;
					int u_xlati19;
					bool u_xlatb19;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD3.yx / vs_TEXCOORD3.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlat3.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat3.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat3.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlatb19 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb19){
					        u_xlatb19 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat3.xyz = vs_TEXCOORD1.www * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD0.www + u_xlat3.xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD2.www + u_xlat3.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat10.x = vs_TEXCOORD0.w;
					        u_xlat10.y = vs_TEXCOORD1.w;
					        u_xlat10.z = vs_TEXCOORD2.w;
					        u_xlat3.xyz = (bool(u_xlatb19)) ? u_xlat3.xyz : u_xlat10.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat3.yzw = u_xlat3.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat19 = u_xlat3.y * 0.25;
					        u_xlat9 = unity_ProbeVolumeParams.z * 0.5;
					        u_xlat4.x = (-unity_ProbeVolumeParams.z) * 0.5 + 0.25;
					        u_xlat19 = max(u_xlat19, u_xlat9);
					        u_xlat3.x = min(u_xlat4.x, u_xlat19);
					        u_xlat4 = texture(unity_ProbeVolumeSH, u_xlat3.xzw);
					        u_xlat5.xyz = u_xlat3.xzw + vec3(0.25, 0.0, 0.0);
					        u_xlat5 = texture(unity_ProbeVolumeSH, u_xlat5.xyz);
					        u_xlat3.xyz = u_xlat3.xzw + vec3(0.5, 0.0, 0.0);
					        u_xlat3 = texture(unity_ProbeVolumeSH, u_xlat3.xyz);
					        u_xlat0.w = 1.0;
					        u_xlat4.x = dot(u_xlat4, u_xlat0);
					        u_xlat4.y = dot(u_xlat5, u_xlat0);
					        u_xlat4.z = dot(u_xlat3, u_xlat0);
					    } else {
					        u_xlat0.w = 1.0;
					        u_xlat4.x = dot(unity_SHAr, u_xlat0);
					        u_xlat4.y = dot(unity_SHAg, u_xlat0);
					        u_xlat4.z = dot(unity_SHAb, u_xlat0);
					    }
					    u_xlat3 = u_xlat0.yzzx * u_xlat0.xyzz;
					    u_xlat5.x = dot(unity_SHBr, u_xlat3);
					    u_xlat5.y = dot(unity_SHBg, u_xlat3);
					    u_xlat5.z = dot(unity_SHBb, u_xlat3);
					    u_xlat18 = u_xlat0.y * u_xlat0.y;
					    u_xlat18 = u_xlat0.x * u_xlat0.x + (-u_xlat18);
					    u_xlat3.xyz = unity_SHC.xyz * vec3(u_xlat18) + u_xlat5.xyz;
					    u_xlat3.xyz = u_xlat3.xyz + u_xlat4.xyz;
					    u_xlat3.xyz = max(u_xlat3.xyz, vec3(0.0, 0.0, 0.0));
					    u_xlat3.xyz = log2(u_xlat3.xyz);
					    u_xlat3.xyz = u_xlat3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
					    u_xlat3.xyz = exp2(u_xlat3.xyz);
					    u_xlat3.xyz = u_xlat3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
					    u_xlat3.xyz = max(u_xlat3.xyz, vec3(0.0, 0.0, 0.0));
					    u_xlat4 = texture(unity_Lightmap, vs_TEXCOORD4.xy);
					    u_xlat18 = u_xlat4.w * unity_Lightmap_HDR.x;
					    u_xlat4.xyz = u_xlat4.xyz * vec3(u_xlat18);
					    u_xlat5 = texture(unity_LightmapInd, vs_TEXCOORD4.xy);
					    u_xlat5.xyz = u_xlat5.xyz + vec3(-0.5, -0.5, -0.5);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat5.xyz);
					    u_xlat18 = u_xlat18 + 0.5;
					    u_xlat4.xyz = vec3(u_xlat18) * u_xlat4.xyz;
					    u_xlat18 = max(u_xlat5.w, 9.99999975e-05);
					    u_xlat4.xyz = u_xlat4.xyz / vec3(u_xlat18);
					    u_xlat3.xyz = u_xlat3.xyz + u_xlat4.xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * _LightColor0.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat2.xyz = u_xlat6.xyz * u_xlat0.xxx + u_xlat1.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "DIRLIGHTMAP_COMBINED" "DYNAMICLIGHTMAP_ON" "LIGHTMAP_ON" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unity_Lightmap_HDR;
						vec4 unity_DynamicLightmap_HDR;
						vec4 _LightColor0;
						vec4 unused_0_3;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[47];
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler2D unity_Lightmap;
					uniform  sampler2D unity_LightmapInd;
					uniform  sampler2D unity_DynamicLightmap;
					uniform  sampler2D unity_DynamicDirectionality;
					in  vec4 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					int u_xlati19;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD3.yx / vs_TEXCOORD3.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlat3.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat3.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat3.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat3 = texture(unity_Lightmap, vs_TEXCOORD4.xy);
					    u_xlat18 = u_xlat3.w * unity_Lightmap_HDR.x;
					    u_xlat3.xyz = u_xlat3.xyz * vec3(u_xlat18);
					    u_xlat4 = texture(unity_LightmapInd, vs_TEXCOORD4.xy);
					    u_xlat4.xyz = u_xlat4.xyz + vec3(-0.5, -0.5, -0.5);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat4.xyz);
					    u_xlat18 = u_xlat18 + 0.5;
					    u_xlat3.xyz = vec3(u_xlat18) * u_xlat3.xyz;
					    u_xlat18 = max(u_xlat4.w, 9.99999975e-05);
					    u_xlat3.xyz = u_xlat3.xyz / vec3(u_xlat18);
					    u_xlat4 = texture(unity_DynamicLightmap, vs_TEXCOORD4.zw);
					    u_xlat18 = u_xlat4.w * unity_DynamicLightmap_HDR.x;
					    u_xlat4.xyz = u_xlat4.xyz * vec3(u_xlat18);
					    u_xlat4.xyz = log2(u_xlat4.xyz);
					    u_xlat4.xyz = u_xlat4.xyz * unity_DynamicLightmap_HDR.yyy;
					    u_xlat4.xyz = exp2(u_xlat4.xyz);
					    u_xlat5 = texture(unity_DynamicDirectionality, vs_TEXCOORD4.zw);
					    u_xlat5.xyz = u_xlat5.xyz + vec3(-0.5, -0.5, -0.5);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat5.xyz);
					    u_xlat18 = u_xlat18 + 0.5;
					    u_xlat4.xyz = vec3(u_xlat18) * u_xlat4.xyz;
					    u_xlat18 = max(u_xlat5.w, 9.99999975e-05);
					    u_xlat4.xyz = u_xlat4.xyz / vec3(u_xlat18);
					    u_xlat3.xyz = u_xlat3.xyz + u_xlat4.xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * _LightColor0.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat2.xyz = u_xlat6.xyz * u_xlat0.xxx + u_xlat1.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
			}
		}
		Pass {
			Name "FORWARD"
			LOD 200
			Tags { "LIGHTMODE" = "FORWARDADD" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha One, SrcAlpha One
			ColorMask RGB -1
			ZWrite Off
			GpuProgramID 91498
			Program "vp" {
				SubProgram "d3d11 " {
					Keywords { "POINT" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[4];
						mat4x4 unity_WorldToLight;
						vec4 unused_0_2[8];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_2_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_2_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					out vec3 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD1;
					out vec3 vs_TEXCOORD2;
					out vec3 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD4;
					out vec3 vs_TEXCOORD5;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					float u_xlat17;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat17 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat17 = inversesqrt(u_xlat17);
					    u_xlat2.xyz = vec3(u_xlat17) * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat17 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat17 = inversesqrt(u_xlat17);
					    u_xlat3.xyz = vec3(u_xlat17) * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat17 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = vec3(u_xlat17) * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    vs_TEXCOORD3.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = unity_ObjectToWorld[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat1.y = u_xlat1.y * _ProjectionParams.x;
					    u_xlat2.xzw = u_xlat1.xwy * vec3(0.5, 0.5, 0.5);
					    vs_TEXCOORD4.zw = u_xlat1.zw;
					    vs_TEXCOORD4.xy = u_xlat2.zz + u_xlat2.xw;
					    u_xlat1.xyz = u_xlat0.yyy * unity_WorldToLight[1].xyz;
					    u_xlat1.xyz = unity_WorldToLight[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
					    u_xlat0.xyz = unity_WorldToLight[2].xyz * u_xlat0.zzz + u_xlat1.xyz;
					    vs_TEXCOORD5.xyz = unity_WorldToLight[3].xyz * u_xlat0.www + u_xlat0.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_1_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					out vec3 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD1;
					out vec3 vs_TEXCOORD2;
					out vec3 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD4;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					float u_xlat13;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    vs_TEXCOORD3.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat0 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    u_xlat0 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat0;
					    gl_Position = u_xlat0;
					    u_xlat1.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat1.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat1.xyz = vec3(u_xlat13) * u_xlat1.xyz;
					    u_xlat2.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat2.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat2.xyz;
					    u_xlat2.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat2.xyz;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat3.xyz = u_xlat1.xyz * u_xlat2.xyz;
					    u_xlat3.xyz = u_xlat1.zxy * u_xlat2.yzx + (-u_xlat3.xyz);
					    u_xlat13 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat3.xyz = vec3(u_xlat13) * u_xlat3.xyz;
					    vs_TEXCOORD0.y = u_xlat3.x;
					    vs_TEXCOORD0.x = u_xlat2.z;
					    vs_TEXCOORD0.z = u_xlat1.y;
					    vs_TEXCOORD1.x = u_xlat2.x;
					    vs_TEXCOORD2.x = u_xlat2.y;
					    vs_TEXCOORD1.z = u_xlat1.z;
					    vs_TEXCOORD2.z = u_xlat1.x;
					    vs_TEXCOORD1.y = u_xlat3.y;
					    vs_TEXCOORD2.y = u_xlat3.z;
					    u_xlat0.y = u_xlat0.y * _ProjectionParams.x;
					    u_xlat1.xzw = u_xlat0.xwy * vec3(0.5, 0.5, 0.5);
					    vs_TEXCOORD4.zw = u_xlat0.zw;
					    vs_TEXCOORD4.xy = u_xlat1.zz + u_xlat1.xw;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "SPOT" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[4];
						mat4x4 unity_WorldToLight;
						vec4 unused_0_2[8];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_2_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_2_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					out vec3 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD1;
					out vec3 vs_TEXCOORD2;
					out vec3 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD4;
					out vec4 vs_TEXCOORD5;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					float u_xlat17;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat17 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat17 = inversesqrt(u_xlat17);
					    u_xlat2.xyz = vec3(u_xlat17) * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat17 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat17 = inversesqrt(u_xlat17);
					    u_xlat3.xyz = vec3(u_xlat17) * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat17 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = vec3(u_xlat17) * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    vs_TEXCOORD3.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = unity_ObjectToWorld[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat1.y = u_xlat1.y * _ProjectionParams.x;
					    u_xlat2.xzw = u_xlat1.xwy * vec3(0.5, 0.5, 0.5);
					    vs_TEXCOORD4.zw = u_xlat1.zw;
					    vs_TEXCOORD4.xy = u_xlat2.zz + u_xlat2.xw;
					    u_xlat1 = u_xlat0.yyyy * unity_WorldToLight[1];
					    u_xlat1 = unity_WorldToLight[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_WorldToLight[2] * u_xlat0.zzzz + u_xlat1;
					    vs_TEXCOORD5 = unity_WorldToLight[3] * u_xlat0.wwww + u_xlat1;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "POINT_COOKIE" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[4];
						mat4x4 unity_WorldToLight;
						vec4 unused_0_2[8];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_2_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_2_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					out vec3 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD1;
					out vec3 vs_TEXCOORD2;
					out vec3 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD4;
					out vec3 vs_TEXCOORD5;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					float u_xlat17;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat17 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat17 = inversesqrt(u_xlat17);
					    u_xlat2.xyz = vec3(u_xlat17) * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat17 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat17 = inversesqrt(u_xlat17);
					    u_xlat3.xyz = vec3(u_xlat17) * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat17 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = vec3(u_xlat17) * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    vs_TEXCOORD3.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = unity_ObjectToWorld[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat1.y = u_xlat1.y * _ProjectionParams.x;
					    u_xlat2.xzw = u_xlat1.xwy * vec3(0.5, 0.5, 0.5);
					    vs_TEXCOORD4.zw = u_xlat1.zw;
					    vs_TEXCOORD4.xy = u_xlat2.zz + u_xlat2.xw;
					    u_xlat1.xyz = u_xlat0.yyy * unity_WorldToLight[1].xyz;
					    u_xlat1.xyz = unity_WorldToLight[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
					    u_xlat0.xyz = unity_WorldToLight[2].xyz * u_xlat0.zzz + u_xlat1.xyz;
					    vs_TEXCOORD5.xyz = unity_WorldToLight[3].xyz * u_xlat0.www + u_xlat0.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL_COOKIE" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[4];
						mat4x4 unity_WorldToLight;
						vec4 unused_0_2[8];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_2_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_2_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					out vec3 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD1;
					out vec3 vs_TEXCOORD2;
					out vec3 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD4;
					out vec2 vs_TEXCOORD5;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					float u_xlat17;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    u_xlat2.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat2.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat2.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat17 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat17 = inversesqrt(u_xlat17);
					    u_xlat2.xyz = vec3(u_xlat17) * u_xlat2.xyz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat17 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat17 = inversesqrt(u_xlat17);
					    u_xlat3.xyz = vec3(u_xlat17) * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat2.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat17 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = vec3(u_xlat17) * u_xlat4.xyz;
					    vs_TEXCOORD0.y = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat3.z;
					    vs_TEXCOORD0.z = u_xlat2.y;
					    vs_TEXCOORD1.x = u_xlat3.x;
					    vs_TEXCOORD2.x = u_xlat3.y;
					    vs_TEXCOORD1.z = u_xlat2.z;
					    vs_TEXCOORD2.z = u_xlat2.x;
					    vs_TEXCOORD1.y = u_xlat4.y;
					    vs_TEXCOORD2.y = u_xlat4.z;
					    vs_TEXCOORD3.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = unity_ObjectToWorld[3] * in_POSITION0.wwww + u_xlat0;
					    u_xlat1.y = u_xlat1.y * _ProjectionParams.x;
					    u_xlat2.xzw = u_xlat1.xwy * vec3(0.5, 0.5, 0.5);
					    vs_TEXCOORD4.zw = u_xlat1.zw;
					    vs_TEXCOORD4.xy = u_xlat2.zz + u_xlat2.xw;
					    u_xlat1.xy = u_xlat0.yy * unity_WorldToLight[1].xy;
					    u_xlat0.xy = unity_WorldToLight[0].xy * u_xlat0.xx + u_xlat1.xy;
					    u_xlat0.xy = unity_WorldToLight[2].xy * u_xlat0.zz + u_xlat0.xy;
					    vs_TEXCOORD5.xy = unity_WorldToLight[3].xy * u_xlat0.ww + u_xlat0.xy;
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					Keywords { "POINT" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _LightColor0;
						vec4 unused_0_2;
						mat4x4 unity_WorldToLight;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[45];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_1_3;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler2D _LightTexture0;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec3 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD1;
					in  vec3 vs_TEXCOORD2;
					in  vec3 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					float u_xlat19;
					int u_xlati19;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD4.yx / vs_TEXCOORD4.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlat3.xyz = (-vs_TEXCOORD3.xyz) + _WorldSpaceLightPos0.xyz;
					    u_xlat18 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat3.xyz = vec3(u_xlat18) * u_xlat3.xyz;
					    u_xlat4.xyz = vs_TEXCOORD3.yyy * unity_WorldToLight[1].xyz;
					    u_xlat4.xyz = unity_WorldToLight[0].xyz * vs_TEXCOORD3.xxx + u_xlat4.xyz;
					    u_xlat4.xyz = unity_WorldToLight[2].xyz * vs_TEXCOORD3.zzz + u_xlat4.xyz;
					    u_xlat4.xyz = u_xlat4.xyz + unity_WorldToLight[3].xyz;
					    u_xlatb18 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb18){
					        u_xlatb18 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat5.xyz = vs_TEXCOORD3.yyy * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat5.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD3.xxx + u_xlat5.xyz;
					        u_xlat5.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD3.zzz + u_xlat5.xyz;
					        u_xlat5.xyz = u_xlat5.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat5.xyz = (bool(u_xlatb18)) ? u_xlat5.xyz : vs_TEXCOORD3.xyz;
					        u_xlat5.xyz = u_xlat5.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat5.yzw = u_xlat5.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat18 = u_xlat5.y * 0.25 + 0.75;
					        u_xlat19 = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat5.x = max(u_xlat18, u_xlat19);
					        u_xlat5 = texture(unity_ProbeVolumeSH, u_xlat5.xzw);
					    } else {
					        u_xlat5.x = float(1.0);
					        u_xlat5.y = float(1.0);
					        u_xlat5.z = float(1.0);
					        u_xlat5.w = float(1.0);
					    }
					    u_xlat18 = dot(u_xlat5, unity_OcclusionMaskSelector);
					    u_xlat18 = clamp(u_xlat18, 0.0, 1.0);
					    u_xlat19 = dot(u_xlat4.xyz, u_xlat4.xyz);
					    u_xlat4 = texture(_LightTexture0, vec2(u_xlat19));
					    u_xlat18 = u_xlat18 * u_xlat4.x;
					    u_xlat4.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat4.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat4.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat4.xyz, u_xlat4.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    u_xlat4.xyz = vec3(u_xlat18) * _LightColor0.xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, u_xlat3.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * u_xlat4.xyz;
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat6.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _LightColor0;
						vec4 unused_0_2;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[45];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_1_3;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec3 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD1;
					in  vec3 vs_TEXCOORD2;
					in  vec3 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					float u_xlat19;
					int u_xlati19;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD4.yx / vs_TEXCOORD4.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlatb18 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb18){
					        u_xlatb18 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat3.xyz = vs_TEXCOORD3.yyy * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD3.xxx + u_xlat3.xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD3.zzz + u_xlat3.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat3.xyz = (bool(u_xlatb18)) ? u_xlat3.xyz : vs_TEXCOORD3.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat3.yzw = u_xlat3.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat18 = u_xlat3.y * 0.25 + 0.75;
					        u_xlat19 = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat3.x = max(u_xlat18, u_xlat19);
					        u_xlat3 = texture(unity_ProbeVolumeSH, u_xlat3.xzw);
					    } else {
					        u_xlat3.x = float(1.0);
					        u_xlat3.y = float(1.0);
					        u_xlat3.z = float(1.0);
					        u_xlat3.w = float(1.0);
					    }
					    u_xlat18 = dot(u_xlat3, unity_OcclusionMaskSelector);
					    u_xlat18 = clamp(u_xlat18, 0.0, 1.0);
					    u_xlat3.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat3.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat3.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat3.xyz = vec3(u_xlat18) * _LightColor0.xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat6.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "SPOT" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _LightColor0;
						vec4 unused_0_2;
						mat4x4 unity_WorldToLight;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[45];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_1_3;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler2D _LightTexture0;
					uniform  sampler2D _LightTextureB0;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec3 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD1;
					in  vec3 vs_TEXCOORD2;
					in  vec3 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					float u_xlat19;
					int u_xlati19;
					bool u_xlatb19;
					float u_xlat21;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD4.yx / vs_TEXCOORD4.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlat3.xyz = (-vs_TEXCOORD3.xyz) + _WorldSpaceLightPos0.xyz;
					    u_xlat18 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat3.xyz = vec3(u_xlat18) * u_xlat3.xyz;
					    u_xlat4 = vs_TEXCOORD3.yyyy * unity_WorldToLight[1];
					    u_xlat4 = unity_WorldToLight[0] * vs_TEXCOORD3.xxxx + u_xlat4;
					    u_xlat4 = unity_WorldToLight[2] * vs_TEXCOORD3.zzzz + u_xlat4;
					    u_xlat4 = u_xlat4 + unity_WorldToLight[3];
					    u_xlatb18 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb18){
					        u_xlatb18 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat5.xyz = vs_TEXCOORD3.yyy * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat5.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD3.xxx + u_xlat5.xyz;
					        u_xlat5.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD3.zzz + u_xlat5.xyz;
					        u_xlat5.xyz = u_xlat5.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat5.xyz = (bool(u_xlatb18)) ? u_xlat5.xyz : vs_TEXCOORD3.xyz;
					        u_xlat5.xyz = u_xlat5.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat5.yzw = u_xlat5.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat18 = u_xlat5.y * 0.25 + 0.75;
					        u_xlat19 = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat5.x = max(u_xlat18, u_xlat19);
					        u_xlat5 = texture(unity_ProbeVolumeSH, u_xlat5.xzw);
					    } else {
					        u_xlat5.x = float(1.0);
					        u_xlat5.y = float(1.0);
					        u_xlat5.z = float(1.0);
					        u_xlat5.w = float(1.0);
					    }
					    u_xlat18 = dot(u_xlat5, unity_OcclusionMaskSelector);
					    u_xlat18 = clamp(u_xlat18, 0.0, 1.0);
					    u_xlatb19 = 0.0<u_xlat4.z;
					    u_xlat19 = u_xlatb19 ? 1.0 : float(0.0);
					    u_xlat5.xy = u_xlat4.xy / u_xlat4.ww;
					    u_xlat5.xy = u_xlat5.xy + vec2(0.5, 0.5);
					    u_xlat5 = texture(_LightTexture0, u_xlat5.xy);
					    u_xlat19 = u_xlat19 * u_xlat5.w;
					    u_xlat21 = dot(u_xlat4.xyz, u_xlat4.xyz);
					    u_xlat4 = texture(_LightTextureB0, vec2(u_xlat21));
					    u_xlat19 = u_xlat19 * u_xlat4.x;
					    u_xlat18 = u_xlat18 * u_xlat19;
					    u_xlat4.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat4.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat4.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat4.xyz, u_xlat4.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    u_xlat4.xyz = vec3(u_xlat18) * _LightColor0.xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, u_xlat3.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * u_xlat4.xyz;
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat6.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "POINT_COOKIE" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _LightColor0;
						vec4 unused_0_2;
						mat4x4 unity_WorldToLight;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[45];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_1_3;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler2D _LightTextureB0;
					uniform  samplerCube _LightTexture0;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec3 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD1;
					in  vec3 vs_TEXCOORD2;
					in  vec3 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					float u_xlat19;
					int u_xlati19;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD4.yx / vs_TEXCOORD4.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlat3.xyz = (-vs_TEXCOORD3.xyz) + _WorldSpaceLightPos0.xyz;
					    u_xlat18 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat3.xyz = vec3(u_xlat18) * u_xlat3.xyz;
					    u_xlat4.xyz = vs_TEXCOORD3.yyy * unity_WorldToLight[1].xyz;
					    u_xlat4.xyz = unity_WorldToLight[0].xyz * vs_TEXCOORD3.xxx + u_xlat4.xyz;
					    u_xlat4.xyz = unity_WorldToLight[2].xyz * vs_TEXCOORD3.zzz + u_xlat4.xyz;
					    u_xlat4.xyz = u_xlat4.xyz + unity_WorldToLight[3].xyz;
					    u_xlatb18 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb18){
					        u_xlatb18 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat5.xyz = vs_TEXCOORD3.yyy * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat5.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD3.xxx + u_xlat5.xyz;
					        u_xlat5.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD3.zzz + u_xlat5.xyz;
					        u_xlat5.xyz = u_xlat5.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat5.xyz = (bool(u_xlatb18)) ? u_xlat5.xyz : vs_TEXCOORD3.xyz;
					        u_xlat5.xyz = u_xlat5.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat5.yzw = u_xlat5.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat18 = u_xlat5.y * 0.25 + 0.75;
					        u_xlat19 = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat5.x = max(u_xlat18, u_xlat19);
					        u_xlat5 = texture(unity_ProbeVolumeSH, u_xlat5.xzw);
					    } else {
					        u_xlat5.x = float(1.0);
					        u_xlat5.y = float(1.0);
					        u_xlat5.z = float(1.0);
					        u_xlat5.w = float(1.0);
					    }
					    u_xlat18 = dot(u_xlat5, unity_OcclusionMaskSelector);
					    u_xlat18 = clamp(u_xlat18, 0.0, 1.0);
					    u_xlat19 = dot(u_xlat4.xyz, u_xlat4.xyz);
					    u_xlat5 = texture(_LightTextureB0, vec2(u_xlat19));
					    u_xlat4 = texture(_LightTexture0, u_xlat4.xyz);
					    u_xlat19 = u_xlat4.w * u_xlat5.x;
					    u_xlat18 = u_xlat18 * u_xlat19;
					    u_xlat4.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat4.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat4.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat4.xyz, u_xlat4.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    u_xlat4.xyz = vec3(u_xlat18) * _LightColor0.xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, u_xlat3.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * u_xlat4.xyz;
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat6.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL_COOKIE" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _LightColor0;
						vec4 unused_0_2;
						mat4x4 unity_WorldToLight;
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 _TextureWorldSize;
						float _SmoothFactor;
						float _DepthVariationClamp;
						float _CullingDistance;
						vec4 _Color;
						float _Visible;
						vec4 _FlipParams;
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_1_1[45];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_1_3;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					uniform  sampler2D _LightTexture0;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec3 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD1;
					in  vec3 vs_TEXCOORD2;
					in  vec3 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec2 u_xlat8;
					vec2 u_xlat12;
					bvec2 u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					vec2 u_xlat14;
					bool u_xlatb14;
					float u_xlat18;
					int u_xlati18;
					bool u_xlatb18;
					float u_xlat19;
					int u_xlati19;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD4.yx / vs_TEXCOORD4.ww;
					    u_xlatb12.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb12.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat12.x = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb12.y) ? u_xlat12.x : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb12.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb12.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat12.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat12.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat12.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat12.x;
					    u_xlat12.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat12.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat12.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat12.x;
					    u_xlat12.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat13 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat13 + -1.0;
					        u_xlat13 = u_xlat12.x;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat14.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat14.x + -1.0;
					            u_xlat8.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat8.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat8.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat14.x = u_xlat3.y * 0.336089998;
					            u_xlat14.x = u_xlat3.x * -0.0999099985 + (-u_xlat14.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat14.x;
					            u_xlat14.x = u_xlat3.y * 0.558610022;
					            u_xlat14.x = u_xlat3.w * 0.61500001 + (-u_xlat14.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat14.x;
					            u_xlat14.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat14.x = dot(u_xlat14.xy, u_xlat14.xy);
					            u_xlat14.x = sqrt(u_xlat14.x);
					            u_xlat8.x = u_xlat8.x + -0.899999976;
					            u_xlat8.x = clamp(u_xlat8.x, 0.0, 1.0);
					            u_xlat8.x = u_xlat8.x + u_xlat14.x;
					            u_xlat13 = u_xlat13 + u_xlat8.x;
					        }
					        u_xlat12.x = u_xlat13;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat18 = u_xlat1.x * 0.00999999978;
					    u_xlatb18 = _CullingDistance<u_xlat18;
					    if(((int(u_xlatb18) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat2.xyz = u_xlat1.xyz * _Color.xyz;
					    u_xlat12.x = u_xlat12.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat3.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat12.xy = u_xlat12.xx * u_xlat3.xy;
					    u_xlat12.xy = clamp(u_xlat12.xy, 0.0, 1.0);
					    u_xlat3.xy = u_xlat12.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat12.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.xy = u_xlat12.xy * u_xlat3.xy;
					    u_xlat3.xy = u_xlat12.xy * u_xlat12.xy;
					    u_xlat12.x = u_xlat12.y * u_xlat3.y;
					    u_xlat18 = dot(u_xlat2.xyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-vec3(u_xlat18));
					    u_xlat1.xyz = u_xlat12.xxx * u_xlat1.xyz + vec3(u_xlat18);
					    u_xlat2.xw = _TextureDimension.zw;
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat4 = u_xlat0.xyxy + u_xlat2;
					    u_xlat5 = texture(_DepthTex, u_xlat4.xy);
					    u_xlat0 = u_xlat0.xyxy + (-u_xlat2);
					    u_xlat2 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat0.x = u_xlat2.x * 0.00999999978;
					    u_xlat0.x = u_xlat5.x * 0.00999999978 + (-u_xlat0.x);
					    u_xlat2 = texture(_DepthTex, u_xlat4.zw);
					    u_xlat4 = texture(_DepthTex, u_xlat0.zw);
					    u_xlat6.x = u_xlat4.x * 0.00999999978;
					    u_xlat6.x = u_xlat2.x * 0.00999999978 + (-u_xlat6.x);
					    u_xlat0.x = max(u_xlat0.x, (-_DepthVariationClamp));
					    u_xlat2.y = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.x = max(u_xlat6.x, (-_DepthVariationClamp));
					    u_xlat2.x = min(u_xlat0.x, _DepthVariationClamp);
					    u_xlat0.xy = _TextureDimension.zw * _TextureWorldSize.xy;
					    u_xlat4.z = dot(u_xlat0.xx, vec2(_SmoothFactor));
					    u_xlat2.z = dot(u_xlat0.yy, vec2(_SmoothFactor));
					    u_xlat4.x = float(0.0);
					    u_xlat4.y = float(0.0);
					    u_xlat2.w = u_xlat4.z;
					    u_xlat0.xy = u_xlat2.zx * u_xlat2.yw;
					    u_xlat0.z = 0.0;
					    u_xlat0.xyz = u_xlat4.xyz * u_xlat2.xyz + (-u_xlat0.xyz);
					    u_xlat18 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat18 = inversesqrt(u_xlat18);
					    u_xlat0.xyz = vec3(u_xlat18) * u_xlat0.xyz;
					    u_xlat2.w = u_xlat3.x * _Visible;
					    u_xlat3.xy = vs_TEXCOORD3.yy * unity_WorldToLight[1].xy;
					    u_xlat3.xy = unity_WorldToLight[0].xy * vs_TEXCOORD3.xx + u_xlat3.xy;
					    u_xlat3.xy = unity_WorldToLight[2].xy * vs_TEXCOORD3.zz + u_xlat3.xy;
					    u_xlat3.xy = u_xlat3.xy + unity_WorldToLight[3].xy;
					    u_xlatb18 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb18){
					        u_xlatb18 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat4.xyz = vs_TEXCOORD3.yyy * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat4.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD3.xxx + u_xlat4.xyz;
					        u_xlat4.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD3.zzz + u_xlat4.xyz;
					        u_xlat4.xyz = u_xlat4.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat4.xyz = (bool(u_xlatb18)) ? u_xlat4.xyz : vs_TEXCOORD3.xyz;
					        u_xlat4.xyz = u_xlat4.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat4.yzw = u_xlat4.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat18 = u_xlat4.y * 0.25 + 0.75;
					        u_xlat19 = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat4.x = max(u_xlat18, u_xlat19);
					        u_xlat4 = texture(unity_ProbeVolumeSH, u_xlat4.xzw);
					    } else {
					        u_xlat4.x = float(1.0);
					        u_xlat4.y = float(1.0);
					        u_xlat4.z = float(1.0);
					        u_xlat4.w = float(1.0);
					    }
					    u_xlat18 = dot(u_xlat4, unity_OcclusionMaskSelector);
					    u_xlat18 = clamp(u_xlat18, 0.0, 1.0);
					    u_xlat3 = texture(_LightTexture0, u_xlat3.xy);
					    u_xlat18 = u_xlat18 * u_xlat3.w;
					    u_xlat3.x = dot(vs_TEXCOORD0.xyz, u_xlat0.xyz);
					    u_xlat3.y = dot(vs_TEXCOORD1.xyz, u_xlat0.xyz);
					    u_xlat3.z = dot(vs_TEXCOORD2.xyz, u_xlat0.xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    u_xlat3.xyz = vec3(u_xlat18) * _LightColor0.xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat0.x = max(u_xlat0.x, 0.0);
					    u_xlat6.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat6.xyz;
					    SV_Target0 = u_xlat2;
					    return;
					}"
				}
			}
		}
		Pass {
			Name "Meta"
			LOD 200
			Tags { "LIGHTMODE" = "META" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			ColorMask RGB -1
			ZWrite Off
			Cull Off
			GpuProgramID 141602
			Program "vp" {
				SubProgram "d3d11 " {
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform UnityPerCamera {
						vec4 unused_0_0[5];
						vec4 _ProjectionParams;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_1_2;
						vec4 unity_WorldTransformParams;
						vec4 unused_1_4;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					layout(std140) uniform UnityLightmaps {
						vec4 unity_LightmapST;
						vec4 unity_DynamicLightmapST;
					};
					layout(std140) uniform UnityMetaPass {
						bvec4 unity_MetaVertexControl;
						vec4 unused_4_1[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD1;
					in  vec4 in_TEXCOORD2;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					vec3 u_xlat4;
					bool u_xlatb15;
					float u_xlat16;
					void main()
					{
					    u_xlatb0 = 0.0<in_POSITION0.z;
					    u_xlat0.z = u_xlatb0 ? 9.99999975e-05 : float(0.0);
					    u_xlat0.xy = in_TEXCOORD1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					    u_xlat0.xyz = (unity_MetaVertexControl.x) ? u_xlat0.xyz : in_POSITION0.xyz;
					    u_xlatb15 = 0.0<u_xlat0.z;
					    u_xlat1.z = u_xlatb15 ? 9.99999975e-05 : float(0.0);
					    u_xlat1.xy = in_TEXCOORD2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
					    u_xlat0.xyz = (unity_MetaVertexControl.y) ? u_xlat1.xyz : u_xlat0.xyz;
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat0 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = u_xlat0 + unity_MatrixVP[3];
					    gl_Position = u_xlat0;
					    u_xlat1.y = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat1.x = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat16 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat16 = inversesqrt(u_xlat16);
					    u_xlat1.xyz = vec3(u_xlat16) * u_xlat1.xyz;
					    u_xlat2.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat2.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat2.xyz;
					    u_xlat2.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat2.xyz;
					    u_xlat16 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat16 = inversesqrt(u_xlat16);
					    u_xlat2.xyz = vec3(u_xlat16) * u_xlat2.xyz;
					    u_xlat3.xyz = u_xlat1.xyz * u_xlat2.xyz;
					    u_xlat3.xyz = u_xlat1.zxy * u_xlat2.yzx + (-u_xlat3.xyz);
					    u_xlat16 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat3.xyz = vec3(u_xlat16) * u_xlat3.xyz;
					    vs_TEXCOORD0.y = u_xlat3.x;
					    u_xlat4.xyz = in_POSITION0.yyy * unity_ObjectToWorld[1].xyz;
					    u_xlat4.xyz = unity_ObjectToWorld[0].xyz * in_POSITION0.xxx + u_xlat4.xyz;
					    u_xlat4.xyz = unity_ObjectToWorld[2].xyz * in_POSITION0.zzz + u_xlat4.xyz;
					    u_xlat4.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat4.xyz;
					    vs_TEXCOORD0.w = u_xlat4.x;
					    vs_TEXCOORD0.x = u_xlat2.z;
					    vs_TEXCOORD0.z = u_xlat1.y;
					    vs_TEXCOORD1.x = u_xlat2.x;
					    vs_TEXCOORD2.x = u_xlat2.y;
					    vs_TEXCOORD1.z = u_xlat1.z;
					    vs_TEXCOORD2.z = u_xlat1.x;
					    vs_TEXCOORD1.w = u_xlat4.y;
					    vs_TEXCOORD2.w = u_xlat4.z;
					    vs_TEXCOORD1.y = u_xlat3.y;
					    vs_TEXCOORD2.y = u_xlat3.z;
					    u_xlat0.y = u_xlat0.y * _ProjectionParams.x;
					    u_xlat1.xzw = u_xlat0.xwy * vec3(0.5, 0.5, 0.5);
					    vs_TEXCOORD3.zw = u_xlat0.zw;
					    vs_TEXCOORD3.xy = u_xlat1.zz + u_xlat1.xw;
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[4];
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySpillRange;
						vec4 _TextureDimension;
						vec4 unused_0_5;
						float _CullingDistance;
						vec4 _Color;
						vec4 unused_0_8;
						vec4 _FlipParams;
						float unity_OneOverOutputBoost;
						float unity_MaxOutputValue;
					};
					layout(std140) uniform UnityMetaPass {
						vec4 unused_1_0;
						bvec4 unity_MetaFragmentControl;
						vec4 unused_1_2;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					uniform  sampler2D _DepthTex;
					in  vec4 vs_TEXCOORD3;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec2 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					vec2 u_xlat7;
					float u_xlat10;
					bvec2 u_xlatb10;
					float u_xlat11;
					bool u_xlatb11;
					vec2 u_xlat12;
					bool u_xlatb12;
					float u_xlat15;
					int u_xlati15;
					bool u_xlatb15;
					float u_xlat16;
					int u_xlati16;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD3.yx / vs_TEXCOORD3.ww;
					    u_xlatb10.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxy).xy;
					    u_xlat1.xy = (-u_xlat0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb10.x) ? u_xlat1.y : u_xlat0.y;
					    u_xlat10 = (-u_xlat1.x) + 1.0;
					    u_xlat0.y = (u_xlatb10.y) ? u_xlat10 : u_xlat1.x;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xy);
					    u_xlatb10.x = u_xlat1.x==0.0;
					    if(((int(u_xlatb10.x) * int(0xffffffffu)))!=0){discard;}
					    u_xlat10 = _ChromaKeyColor.y * 0.336089998;
					    u_xlat10 = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat10);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat10;
					    u_xlat10 = _ChromaKeyColor.y * 0.558610022;
					    u_xlat10 = _ChromaKeyColor.x * 0.61500001 + (-u_xlat10);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat10;
					    u_xlat10 = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat11 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat11 + -1.0;
					        u_xlat11 = u_xlat10;
					        for(int u_xlati_loop_2 = 0 ; u_xlati_loop_2<3 ; u_xlati_loop_2++)
					        {
					            u_xlat12.x = float(u_xlati_loop_2);
					            u_xlat2.y = u_xlat12.x + -1.0;
					            u_xlat7.xy = u_xlat2.xy * _TextureDimension.zw + u_xlat0.xy;
					            u_xlat3 = texture(_MainTex, u_xlat7.xy);
					            u_xlat3 = u_xlat3.xyzx * _Color.xyzx;
					            u_xlat7.x = dot(u_xlat3.wyz, vec3(0.212599993, 0.715200007, 0.0722000003));
					            u_xlat12.x = u_xlat3.y * 0.336089998;
					            u_xlat12.x = u_xlat3.x * -0.0999099985 + (-u_xlat12.x);
					            u_xlat4.x = u_xlat3.z * 0.43599999 + u_xlat12.x;
					            u_xlat12.x = u_xlat3.y * 0.558610022;
					            u_xlat12.x = u_xlat3.w * 0.61500001 + (-u_xlat12.x);
					            u_xlat4.y = (-u_xlat3.z) * 0.0563899986 + u_xlat12.x;
					            u_xlat12.xy = (-u_xlat1.xy) + u_xlat4.xy;
					            u_xlat12.x = dot(u_xlat12.xy, u_xlat12.xy);
					            u_xlat12.x = sqrt(u_xlat12.x);
					            u_xlat7.x = u_xlat7.x + -0.899999976;
					            u_xlat7.x = clamp(u_xlat7.x, 0.0, 1.0);
					            u_xlat7.x = u_xlat7.x + u_xlat12.x;
					            u_xlat11 = u_xlat11 + u_xlat7.x;
					        }
					        u_xlat10 = u_xlat11;
					    }
					    u_xlat1 = texture(_DepthTex, u_xlat0.xy);
					    u_xlat15 = u_xlat1.x * 0.00999999978;
					    u_xlatb15 = _CullingDistance<u_xlat15;
					    if(((int(u_xlatb15) * int(0xffffffffu)))!=0){discard;}
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat0.xyw = u_xlat1.xyz * _Color.xyz;
					    u_xlat10 = u_xlat10 * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat16 = float(1.0) / _ChromaKeySpillRange;
					    u_xlat10 = u_xlat10 * u_xlat16;
					    u_xlat10 = clamp(u_xlat10, 0.0, 1.0);
					    u_xlat16 = u_xlat10 * -2.0 + 3.0;
					    u_xlat10 = u_xlat10 * u_xlat10;
					    u_xlat10 = u_xlat10 * u_xlat16;
					    u_xlat16 = u_xlat10 * u_xlat10;
					    u_xlat10 = u_xlat10 * u_xlat16;
					    u_xlat0.x = dot(u_xlat0.xyw, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-u_xlat0.xxx);
					    u_xlat0.xyz = vec3(u_xlat10) * u_xlat1.xyz + u_xlat0.xxx;
					    u_xlat15 = unity_OneOverOutputBoost;
					    u_xlat15 = clamp(u_xlat15, 0.0, 1.0);
					    u_xlat0.xyz = log2(u_xlat0.xyz);
					    u_xlat0.xyz = u_xlat0.xyz * vec3(u_xlat15);
					    u_xlat0.xyz = exp2(u_xlat0.xyz);
					    u_xlat0.xyz = min(u_xlat0.xyz, vec3(vec3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue)));
					    u_xlat0.w = 1.0;
					    u_xlat0 = (unity_MetaFragmentControl.x) ? u_xlat0 : vec4(0.0, 0.0, 0.0, 0.0);
					    SV_Target0 = (unity_MetaFragmentControl.y) ? vec4(0.0, 0.0, 0.0, 1.0) : u_xlat0;
					    return;
					}"
				}
			}
		}
	}
	Fallback "Alpha-Diffuse"
}