Shader "Oculus/OVRMRCameraFrame" {
	Properties {
		_Color ("Main Color", Vector) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_Visible ("Visible", Range(0, 1)) = 1
		_ChromaAlphaCutoff ("ChromaAlphaCutoff", Range(0, 1)) = 0.01
		_ChromaToleranceA ("ChromaToleranceA", Range(0, 50)) = 20
		_ChromaToleranceB ("ChromaToleranceB", Range(0, 50)) = 15
		_ChromaShadows ("ChromaShadows", Range(0, 1)) = 0.02
	}
	SubShader {
		LOD 100
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			LOD 100
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			Cull Off
			Fog {
				Mode Off
			}
			GpuProgramID 34540
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
						vec4 unused_0_0[4];
						vec4 _MainTex_ST;
						vec4 unused_0_2[2];
						float _Visible;
						vec4 unused_0_4;
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
					out vec2 vs_TEXCOORD0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    gl_Position = u_xlat0 * vec4(_Visible);
					    u_xlat0.xy = in_TEXCOORD0.xy * vec2(1.0, -1.0) + vec2(0.0, 1.0);
					    vs_TEXCOORD0.xy = u_xlat0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
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
						vec4 unused_0_0[2];
						vec4 _ChromaKeyColor;
						float _ChromaKeySimilarity;
						float _ChromaKeySmoothRange;
						float _ChromaKeySpillRange;
						vec4 unused_0_5;
						vec4 _TextureDimension;
						vec4 _Color;
						vec4 unused_0_8;
						vec4 _FlipParams;
					};
					uniform  sampler2D _MaskTex;
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bvec2 u_xlatb0;
					vec4 u_xlat1;
					vec2 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					vec2 u_xlat7;
					vec2 u_xlat10;
					bool u_xlatb10;
					float u_xlat11;
					bool u_xlatb11;
					vec2 u_xlat12;
					bool u_xlatb12;
					int u_xlati15;
					int u_xlati16;
					void main()
					{
					    u_xlatb0.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), _FlipParams.xyxx).xy;
					    u_xlat10.xy = (-vs_TEXCOORD0.xy) + vec2(1.0, 1.0);
					    u_xlat0.x = (u_xlatb0.x) ? u_xlat10.x : vs_TEXCOORD0.x;
					    u_xlat0.y = (u_xlatb0.y) ? u_xlat10.y : vs_TEXCOORD0.y;
					    u_xlat0.z = (-u_xlat0.y) + 1.0;
					    u_xlat1 = texture(_MaskTex, u_xlat0.xz);
					    u_xlatb10 = u_xlat1.x==0.0;
					    if(((int(u_xlatb10) * int(0xffffffffu)))!=0){discard;}
					    u_xlat10.x = _ChromaKeyColor.y * 0.336089998;
					    u_xlat10.x = _ChromaKeyColor.x * -0.0999099985 + (-u_xlat10.x);
					    u_xlat1.x = _ChromaKeyColor.z * 0.43599999 + u_xlat10.x;
					    u_xlat10.x = _ChromaKeyColor.y * 0.558610022;
					    u_xlat10.x = _ChromaKeyColor.x * 0.61500001 + (-u_xlat10.x);
					    u_xlat1.y = (-_ChromaKeyColor.z) * 0.0563899986 + u_xlat10.x;
					    u_xlat10.x = float(0.0);
					    for(int u_xlati_loop_1 = int(0) ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat11 = float(u_xlati_loop_1);
					        u_xlat2.x = u_xlat11 + -1.0;
					        u_xlat11 = u_xlat10.x;
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
					        u_xlat10.x = u_xlat11;
					    }
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat0.xyw = u_xlat1.xyz * _Color.xyz;
					    u_xlat10.x = u_xlat10.x * 0.111111112 + (-_ChromaKeySimilarity);
					    u_xlat2.xy = vec2(1.0, 1.0) / vec2(_ChromaKeySmoothRange, _ChromaKeySpillRange);
					    u_xlat2.xy = u_xlat10.xx * u_xlat2.xy;
					    u_xlat2.xy = clamp(u_xlat2.xy, 0.0, 1.0);
					    u_xlat12.xy = u_xlat2.xy * vec2(-2.0, -2.0) + vec2(3.0, 3.0);
					    u_xlat2.xy = u_xlat2.xy * u_xlat2.xy;
					    u_xlat2.xy = u_xlat2.xy * u_xlat12.xy;
					    SV_Target0.w = u_xlat2.x * u_xlat2.x;
					    u_xlat10.x = u_xlat2.y * u_xlat2.y;
					    u_xlat10.x = u_xlat2.y * u_xlat10.x;
					    u_xlat0.x = dot(u_xlat0.xyw, vec3(0.212599993, 0.715200007, 0.0722000003));
					    u_xlat1.xyz = u_xlat1.xyz * _Color.xyz + (-u_xlat0.xxx);
					    SV_Target0.xyz = u_xlat10.xxx * u_xlat1.xyz + u_xlat0.xxx;
					    return;
					}"
				}
			}
		}
	}
}