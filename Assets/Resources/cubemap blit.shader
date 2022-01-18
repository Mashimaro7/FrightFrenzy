Shader "Oculus/Cubemap Blit" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", Cube) = "white" {}
		_face ("Face", Float) = 0
		_linearToSrgb ("Perform linear-to-gamma conversion", Float) = 0
		_premultiply ("Cubemap Blit", Float) = 1
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			ZWrite Off
			GpuProgramID 46655
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
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[3];
						int _face;
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_1_1[7];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					out vec3 vs_TEXCOORD0;
					vec4 u_xlat0;
					int u_xlati0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					float u_xlat9;
					vec4 TempArray0[6];
					vec4 TempArray1[6];
					vec4 TempArray2[6];
					void main()
					{
					    TempArray0[0].xyz = vec3(1.0, -1.0, 1.0);
					    TempArray0[1].xyz = vec3(-1.0, -1.0, -1.0);
					    TempArray0[2].xyz = vec3(-1.0, 1.0, 1.0);
					    TempArray0[3].xyz = vec3(-1.0, -1.0, -1.0);
					    TempArray0[4].xyz = vec3(-1.0, -1.0, 1.0);
					    TempArray0[5].xyz = vec3(1.0, -1.0, -1.0);
					    TempArray1[0].xz = vec2(0.0, -1.0);
					    TempArray1[1].xz = vec2(0.0, 1.0);
					    TempArray1[2].xz = vec2(1.0, 0.0);
					    TempArray1[3].xz = vec2(1.0, 0.0);
					    TempArray1[4].xz = vec2(1.0, 0.0);
					    TempArray1[5].xz = vec2(-1.0, 0.0);
					    TempArray2[0].yz = vec2(1.0, 0.0);
					    TempArray2[1].yz = vec2(1.0, 0.0);
					    TempArray2[2].yz = vec2(0.0, -1.0);
					    TempArray2[3].yz = vec2(0.0, 1.0);
					    TempArray2[4].yz = vec2(1.0, 0.0);
					    TempArray2[5].yz = vec2(1.0, 0.0);
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    u_xlati0 = _face;
					    u_xlat3.xy = TempArray1[u_xlati0].xz;
					    u_xlat9 = in_TEXCOORD0.x * 2.0;
					    u_xlat1.xz = u_xlat3.xy * vec2(u_xlat9);
					    u_xlat3.xyz = TempArray0[u_xlati0].xyz;
					    u_xlat2.xy = TempArray2[u_xlati0].yz;
					    u_xlat1.y = 0.0;
					    u_xlat0.xyz = u_xlat1.xyz + u_xlat3.xyz;
					    u_xlat1.xyz = in_TEXCOORD0.yyy * vec3(-1.0, -2.0, -2.0) + vec3(1.0, 2.0, 2.0);
					    u_xlat2.z = 0.0;
					    vs_TEXCOORD0.xyz = u_xlat1.xyz * u_xlat2.zxy + u_xlat0.xyz;
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
						vec4 unused_0_0[3];
						int _linearToSrgb;
						int _premultiply;
					};
					uniform  samplerCube _MainTex;
					in  vec3 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec3 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xyz);
					    u_xlat1.xyz = sqrt(u_xlat0.xyz);
					    u_xlat2.xyz = sqrt(u_xlat1.xyz);
					    u_xlat3.xyz = u_xlat2.xyz * vec3(0.684122086, 0.684122086, 0.684122086);
					    u_xlat2.xyz = sqrt(u_xlat2.xyz);
					    u_xlat1.xyz = u_xlat1.xyz * vec3(0.662002683, 0.662002683, 0.662002683) + u_xlat3.xyz;
					    u_xlat1.xyz = (-u_xlat2.xyz) * vec3(0.323583603, 0.323583603, 0.323583603) + u_xlat1.xyz;
					    u_xlat1.xyz = (-u_xlat0.xyz) * vec3(0.0225411467, 0.0225411467, 0.0225411467) + u_xlat1.xyz;
					    u_xlat0.xyz = (int(_linearToSrgb) != 0) ? u_xlat1.xyz : u_xlat0.xyz;
					    u_xlat1.xyz = u_xlat0.www * u_xlat0.xyz;
					    SV_Target0.xyz = (int(_premultiply) != 0) ? u_xlat1.xyz : u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
			}
		}
	}
}