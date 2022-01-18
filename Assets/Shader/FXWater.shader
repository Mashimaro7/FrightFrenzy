Shader "FX/Water" {
	Properties {
		_WaveScale ("Wave scale", Range(0.02, 0.15)) = 0.063
		_ReflDistort ("Reflection distort", Range(0, 1.5)) = 0.44
		_RefrDistort ("Refraction distort", Range(0, 1.5)) = 0.4
		_RefrColor ("Refraction color", Vector) = (0.34,0.85,0.92,1)
		[NoScaleOffset] _Fresnel ("Fresnel (A) ", 2D) = "gray" {}
		[NoScaleOffset] _BumpMap ("Normalmap ", 2D) = "bump" {}
		WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
		[NoScaleOffset] _ReflectiveColor ("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
		_HorizonColor ("Simple water horizon color", Vector) = (0.172,0.463,0.435,1)
		[HideInInspector] _ReflectionTex ("Internal Reflection", 2D) = "" {}
		[HideInInspector] _RefractionTex ("Internal Refraction", 2D) = "" {}
	}
	SubShader {
		Tags { "RenderType" = "Opaque" "WaterMode" = "Refractive" }
		Pass {
			Tags { "RenderType" = "Opaque" "WaterMode" = "Refractive" }
			GpuProgramID 32035
			Program "vp" {
				SubProgram "d3d11 " {
					Keywords { "WATER_REFRACTIVE" }
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
						vec4 unused_0_0[2];
						vec4 _WaveScale4;
						vec4 _WaveOffset;
						vec4 unused_0_3[2];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[4];
						vec3 _WorldSpaceCameraPos;
						vec4 _ProjectionParams;
						vec4 unused_1_3[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[7];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					out vec4 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
					 vec4 phase0_Output0_2;
					out vec2 vs_TEXCOORD2;
					out vec3 vs_TEXCOORD3;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					float u_xlat9;
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
					    u_xlat9 = u_xlat1.y * _ProjectionParams.x;
					    u_xlat2.w = u_xlat9 * 0.5;
					    u_xlat2.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD0.zw = u_xlat1.zw;
					    vs_TEXCOORD0.xy = u_xlat2.zz + u_xlat2.xw;
					    u_xlat1 = unity_ObjectToWorld[3].xzzx * in_POSITION0.wwww + u_xlat0.xzzx;
					    u_xlat0.xyz = unity_ObjectToWorld[3].xzy * in_POSITION0.www + u_xlat0.xzy;
					    vs_TEXCOORD3.xyz = (-u_xlat0.xyz) + _WorldSpaceCameraPos.xzy;
					    phase0_Output0_2 = u_xlat1 * _WaveScale4.xywz + _WaveOffset.xywz;
					vs_TEXCOORD1 = phase0_Output0_2.xy;
					vs_TEXCOORD2 = phase0_Output0_2.zw;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "WATER_REFLECTIVE" }
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
						vec4 unused_0_0[2];
						vec4 _WaveScale4;
						vec4 _WaveOffset;
						vec4 unused_0_3;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[4];
						vec3 _WorldSpaceCameraPos;
						vec4 _ProjectionParams;
						vec4 unused_1_3[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[7];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					out vec4 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
					 vec4 phase0_Output0_2;
					out vec2 vs_TEXCOORD2;
					out vec3 vs_TEXCOORD3;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					float u_xlat9;
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
					    u_xlat9 = u_xlat1.y * _ProjectionParams.x;
					    u_xlat2.w = u_xlat9 * 0.5;
					    u_xlat2.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD0.zw = u_xlat1.zw;
					    vs_TEXCOORD0.xy = u_xlat2.zz + u_xlat2.xw;
					    u_xlat1 = unity_ObjectToWorld[3].xzzx * in_POSITION0.wwww + u_xlat0.xzzx;
					    u_xlat0.xyz = unity_ObjectToWorld[3].xzy * in_POSITION0.www + u_xlat0.xzy;
					    vs_TEXCOORD3.xyz = (-u_xlat0.xyz) + _WorldSpaceCameraPos.xzy;
					    phase0_Output0_2 = u_xlat1 * _WaveScale4.xywz + _WaveOffset.xywz;
					vs_TEXCOORD1 = phase0_Output0_2.xy;
					vs_TEXCOORD2 = phase0_Output0_2.zw;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "WATER_SIMPLE" }
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
						vec4 unused_0_0[2];
						vec4 _WaveScale4;
						vec4 _WaveOffset;
						vec4 unused_0_3;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[4];
						vec3 _WorldSpaceCameraPos;
						vec4 unused_1_2[4];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[7];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					out vec2 vs_TEXCOORD0;
					 vec4 phase0_Output0_1;
					out vec2 vs_TEXCOORD1;
					out vec3 vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    gl_Position = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    u_xlat1 = unity_ObjectToWorld[3].xzzx * in_POSITION0.wwww + u_xlat0.xzzx;
					    u_xlat0.xyz = unity_ObjectToWorld[3].xzy * in_POSITION0.www + u_xlat0.xzy;
					    vs_TEXCOORD2.xyz = (-u_xlat0.xyz) + _WorldSpaceCameraPos.xzy;
					    phase0_Output0_1 = u_xlat1 * _WaveScale4.xywz + _WaveOffset.xywz;
					vs_TEXCOORD0 = phase0_Output0_1.xy;
					vs_TEXCOORD1 = phase0_Output0_1.zw;
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					Keywords { "WATER_REFRACTIVE" }
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
						float _ReflDistort;
						float _RefrDistort;
						vec4 _RefrColor;
					};
					uniform  sampler2D _BumpMap;
					uniform  sampler2D _ReflectionTex;
					uniform  sampler2D _RefractionTex;
					uniform  sampler2D _Fresnel;
					in  vec4 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec2 vs_TEXCOORD2;
					in  vec3 vs_TEXCOORD3;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					float u_xlat12;
					void main()
					{
					    u_xlat0 = texture(_BumpMap, vs_TEXCOORD1.xy);
					    u_xlat0.x = u_xlat0.w * u_xlat0.x;
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
					    u_xlat12 = dot(u_xlat0.xy, u_xlat0.xy);
					    u_xlat12 = min(u_xlat12, 1.0);
					    u_xlat12 = (-u_xlat12) + 1.0;
					    u_xlat0.z = sqrt(u_xlat12);
					    u_xlat1 = texture(_BumpMap, vs_TEXCOORD2.xy);
					    u_xlat1.x = u_xlat1.w * u_xlat1.x;
					    u_xlat1.xy = u_xlat1.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
					    u_xlat12 = dot(u_xlat1.xy, u_xlat1.xy);
					    u_xlat12 = min(u_xlat12, 1.0);
					    u_xlat12 = (-u_xlat12) + 1.0;
					    u_xlat1.z = sqrt(u_xlat12);
					    u_xlat0.xyz = u_xlat0.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * vec3(0.5, 0.5, 0.5);
					    u_xlat1.xy = u_xlat0.xy * vec2(_ReflDistort) + vs_TEXCOORD0.xy;
					    u_xlat1.xy = u_xlat1.xy / vs_TEXCOORD0.ww;
					    u_xlat1 = texture(_ReflectionTex, u_xlat1.xy);
					    u_xlat2.xy = (-u_xlat0.xy) * vec2(vec2(_RefrDistort, _RefrDistort)) + vs_TEXCOORD0.xy;
					    u_xlat2.xy = u_xlat2.xy / vs_TEXCOORD0.ww;
					    u_xlat2 = texture(_RefractionTex, u_xlat2.xy);
					    u_xlat1 = (-u_xlat2) * _RefrColor + u_xlat1;
					    u_xlat2 = u_xlat2 * _RefrColor;
					    u_xlat12 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat3.xyz = vec3(u_xlat12) * vs_TEXCOORD3.xyz;
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat0.xyz);
					    u_xlat0 = texture(_Fresnel, u_xlat0.xx);
					    SV_Target0 = u_xlat0.wwww * u_xlat1 + u_xlat2;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "WATER_REFLECTIVE" }
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
						float _ReflDistort;
					};
					uniform  sampler2D _BumpMap;
					uniform  sampler2D _ReflectionTex;
					uniform  sampler2D _ReflectiveColor;
					in  vec4 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec2 vs_TEXCOORD2;
					in  vec3 vs_TEXCOORD3;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					float u_xlat9;
					void main()
					{
					    u_xlat0 = texture(_BumpMap, vs_TEXCOORD1.xy);
					    u_xlat0.x = u_xlat0.w * u_xlat0.x;
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
					    u_xlat9 = dot(u_xlat0.xy, u_xlat0.xy);
					    u_xlat9 = min(u_xlat9, 1.0);
					    u_xlat9 = (-u_xlat9) + 1.0;
					    u_xlat0.z = sqrt(u_xlat9);
					    u_xlat1 = texture(_BumpMap, vs_TEXCOORD2.xy);
					    u_xlat1.x = u_xlat1.w * u_xlat1.x;
					    u_xlat1.xy = u_xlat1.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
					    u_xlat9 = dot(u_xlat1.xy, u_xlat1.xy);
					    u_xlat9 = min(u_xlat9, 1.0);
					    u_xlat9 = (-u_xlat9) + 1.0;
					    u_xlat1.z = sqrt(u_xlat9);
					    u_xlat0.xyz = u_xlat0.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * vec3(0.5, 0.5, 0.5);
					    u_xlat1.xy = u_xlat0.xy * vec2(_ReflDistort) + vs_TEXCOORD0.xy;
					    u_xlat1.xy = u_xlat1.xy / vs_TEXCOORD0.ww;
					    u_xlat1 = texture(_ReflectionTex, u_xlat1.xy);
					    u_xlat9 = dot(vs_TEXCOORD3.xyz, vs_TEXCOORD3.xyz);
					    u_xlat9 = inversesqrt(u_xlat9);
					    u_xlat2.xyz = vec3(u_xlat9) * vs_TEXCOORD3.xyz;
					    u_xlat0.x = dot(u_xlat2.xyz, u_xlat0.xyz);
					    u_xlat0 = texture(_ReflectiveColor, u_xlat0.xx);
					    u_xlat1.xyz = (-u_xlat0.xyz) + u_xlat1.xyz;
					    SV_Target0.w = u_xlat0.w * u_xlat1.w;
					    SV_Target0.xyz = u_xlat0.www * u_xlat1.xyz + u_xlat0.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "WATER_SIMPLE" }
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
						vec4 _HorizonColor;
					};
					uniform  sampler2D _BumpMap;
					uniform  sampler2D _ReflectiveColor;
					in  vec2 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec3 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat6;
					void main()
					{
					    u_xlat0 = texture(_BumpMap, vs_TEXCOORD0.xy);
					    u_xlat0.x = u_xlat0.w * u_xlat0.x;
					    u_xlat0.xy = u_xlat0.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
					    u_xlat6 = dot(u_xlat0.xy, u_xlat0.xy);
					    u_xlat6 = min(u_xlat6, 1.0);
					    u_xlat6 = (-u_xlat6) + 1.0;
					    u_xlat0.z = sqrt(u_xlat6);
					    u_xlat1 = texture(_BumpMap, vs_TEXCOORD1.xy);
					    u_xlat1.x = u_xlat1.w * u_xlat1.x;
					    u_xlat1.xy = u_xlat1.xy * vec2(2.0, 2.0) + vec2(-1.0, -1.0);
					    u_xlat6 = dot(u_xlat1.xy, u_xlat1.xy);
					    u_xlat6 = min(u_xlat6, 1.0);
					    u_xlat6 = (-u_xlat6) + 1.0;
					    u_xlat1.z = sqrt(u_xlat6);
					    u_xlat0.xyz = u_xlat0.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * vec3(0.5, 0.5, 0.5);
					    u_xlat6 = dot(vs_TEXCOORD2.xyz, vs_TEXCOORD2.xyz);
					    u_xlat6 = inversesqrt(u_xlat6);
					    u_xlat1.xyz = vec3(u_xlat6) * vs_TEXCOORD2.xyz;
					    u_xlat0.x = dot(u_xlat1.xyz, u_xlat0.xyz);
					    u_xlat0 = texture(_ReflectiveColor, u_xlat0.xx);
					    u_xlat1.xyz = (-u_xlat0.xyz) + _HorizonColor.xyz;
					    SV_Target0.xyz = u_xlat0.www * u_xlat1.xyz + u_xlat0.xyz;
					    SV_Target0.w = _HorizonColor.w;
					    return;
					}"
				}
			}
		}
	}
}