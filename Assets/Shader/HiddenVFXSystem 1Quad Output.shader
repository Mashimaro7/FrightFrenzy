Shader "Hidden/VFX/System 1/Quad Output" {
	Properties {
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 59476
			Program "vp" {
				SubProgram "d3d11 " {
					"vs_5_0
					
					#version 430
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
					precise vec4 u_xlat_precise_vec4;
					precise ivec4 u_xlat_precise_ivec4;
					precise bvec4 u_xlat_precise_bvec4;
					precise uvec4 u_xlat_precise_uvec4;
					vec4 ImmCB_0_0_0[4];
					UNITY_BINDING(0) uniform parameters {
						vec4 Size_b;
						float Color_c;
					};
					UNITY_BINDING(1) uniform outputParams {
						float nbMax;
					};
					UNITY_BINDING(2) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[7];
					};
					UNITY_BINDING(3) uniform UnityPerFrame {
						vec4 unused_3_0[9];
						mat4x4 unity_MatrixV;
						vec4 unused_3_2[4];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_4[2];
					};
					layout(std430, binding = 0) readonly buffer attributeBuffer {
						uint attributeBuffer_buf[];
					};
					 struct indirectBuffer_type {
						uint[1] value;
					};
					
					layout(std430, binding = 1) readonly buffer indirectBuffer {
						indirectBuffer_type indirectBuffer_buf[];
					};
					layout(std430, binding = 2) readonly buffer deadListCount {
						uint deadListCount_buf[];
					};
					UNITY_LOCATION(0) uniform  sampler2D bakedTexture;
					layout(location = 0) out vec2 vs_TEXCOORD0;
					layout(location = 1) out vec4 vs_COLOR0;
					vec4 u_xlat0;
					int u_xlati0;
					uint u_xlatu0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec3 u_xlat5;
					float u_xlat6;
					ivec2 u_xlati6;
					uint u_xlatu6;
					bool u_xlatb6;
					vec3 u_xlat7;
					vec2 u_xlat12;
					uint u_xlatu12;
					vec2 u_xlat13;
					int u_xlati13;
					void main()
					{
						ImmCB_0_0_0[0] = vec4(1.0, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[1] = vec4(0.0, 1.0, 0.0, 0.0);
						ImmCB_0_0_0[2] = vec4(0.0, 0.0, 1.0, 0.0);
						ImmCB_0_0_0[3] = vec4(0.0, 0.0, 0.0, 1.0);
					    u_xlatu0 = uint(gl_VertexID) >> 2u;
					    u_xlati6.x = gl_InstanceID << 11;
					    u_xlatu0 = uint(u_xlati6.x) + u_xlatu0;
					    u_xlat6 = uintBitsToFloat(deadListCount_buf[(0 >> 2) + 0]);
					    u_xlatu6 = (-floatBitsToUint(u_xlat6)) + floatBitsToUint(nbMax);
					    u_xlatb6 = u_xlatu0>=u_xlatu6;
					    if(u_xlatb6){
					        gl_Position = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_COLOR0 = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_TEXCOORD0.xy = vec2(0.0, 0.0);
					        return;
					    }
					    u_xlati0 = int(indirectBuffer_buf[u_xlatu0].value[(0 >> 2) + 0]);
					    u_xlati0 = u_xlati0 << 1;
					    u_xlati6.xy = ivec2(u_xlati0) << ivec2(1, 2);
					    u_xlati6.xy = u_xlati6.xy + ivec2(192, 432);
					    u_xlat6 = uintBitsToFloat(attributeBuffer_buf[(u_xlati6.x >> 2) + 0]);
					    u_xlat12.xy = vec2(uintBitsToFloat(attributeBuffer_buf[(u_xlati6.y >> 2) + 0]), uintBitsToFloat(attributeBuffer_buf[(u_xlati6.y >> 2) + 1]));
					    u_xlat6 = u_xlat12.x / u_xlat6;
					    u_xlat1.x = u_xlat6 * Size_b.x + Size_b.y;
					    u_xlatu12 = floatBitsToUint(Size_b.w) >> 2u;
					    switch(int(u_xlatu12)){
					        case 1:
					            u_xlat12.x = min(u_xlat1.x, 1.0);
					            u_xlat12.x = fract(u_xlat12.x);
					            u_xlat1.x = u_xlat12.x * 0.9921875 + 0.00390625;
					            break;
					        case 2:
					            u_xlat12.x = max(u_xlat1.x, 0.0);
					            u_xlat12.x = fract(u_xlat12.x);
					            u_xlat1.x = u_xlat12.x * 0.9921875 + 0.00390625;
					            break;
					        case 3:
					            u_xlat1.x = u_xlat1.x;
					            u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
					            u_xlat1.x = u_xlat1.x * 0.9921875 + 0.00390625;
					            break;
					        default:
					            break;
					    }
					    if(floatBitsToUint(u_xlat12.y) == uint(0)) {
					        gl_Position = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_COLOR0 = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_TEXCOORD0.xy = vec2(0.0, 0.0);
					        return;
					    }
					    u_xlati0 = u_xlati0 << 3;
					    u_xlati0 = u_xlati0 + 240;
					    u_xlat0.xzw = vec3(uintBitsToFloat(attributeBuffer_buf[(u_xlati0 >> 2) + 0]), uintBitsToFloat(attributeBuffer_buf[(u_xlati0 >> 2) + 1]), uintBitsToFloat(attributeBuffer_buf[(u_xlati0 >> 2) + 2]));
					    u_xlat13.xy = unity_ObjectToWorld[0].yy * unity_MatrixV[1].xy;
					    u_xlat13.xy = unity_MatrixV[0].xy * unity_ObjectToWorld[0].xx + u_xlat13.xy;
					    u_xlat2.xy = unity_MatrixV[2].xy * unity_ObjectToWorld[0].zz + u_xlat13.xy;
					    u_xlat13.xy = unity_ObjectToWorld[1].yy * unity_MatrixV[1].xy;
					    u_xlat13.xy = unity_MatrixV[0].xy * unity_ObjectToWorld[1].xx + u_xlat13.xy;
					    u_xlat3.xy = unity_MatrixV[2].xy * unity_ObjectToWorld[1].zz + u_xlat13.xy;
					    u_xlat13.xy = unity_ObjectToWorld[2].yy * unity_MatrixV[1].xy;
					    u_xlat13.xy = unity_MatrixV[0].xy * unity_ObjectToWorld[2].xx + u_xlat13.xy;
					    u_xlat13.xy = unity_MatrixV[2].xy * unity_ObjectToWorld[2].zz + u_xlat13.xy;
					    u_xlat2.z = u_xlat3.x;
					    u_xlat2.w = u_xlat13.x;
					    u_xlat13.x = dot(u_xlat2.xzw, u_xlat2.xzw);
					    u_xlat13.x = inversesqrt(u_xlat13.x);
					    u_xlat2.xzw = u_xlat13.xxx * u_xlat2.xzw;
					    u_xlat3.z = u_xlat2.y;
					    u_xlat3.w = u_xlat13.y;
					    u_xlat13.x = dot(u_xlat3.yzw, u_xlat3.yzw);
					    u_xlat13.x = inversesqrt(u_xlat13.x);
					    u_xlat3.xyz = u_xlat13.xxx * u_xlat3.zyw;
					    u_xlati13 = int(floatBitsToUint(Size_b.w) & 3u);
					    u_xlat1.y = Size_b.z;
					    u_xlat4 = textureLod(bakedTexture, u_xlat1.xy, 0.0);
					    u_xlat1.x = dot(u_xlat4, ImmCB_0_0_0[u_xlati13]);
					    u_xlat6 = u_xlat6;
					    u_xlat6 = clamp(u_xlat6, 0.0, 1.0);
					    u_xlat4.x = u_xlat6 * 0.9921875 + 0.00390625;
					    u_xlat4.y = Color_c;
					    vs_COLOR0 = textureLod(bakedTexture, u_xlat4.xy, 0.0);
					    u_xlatu6 = uint(gl_VertexID) & 1u;
					    u_xlat4.x = float(u_xlatu6);
					    u_xlatu6 = bitfieldExtract(uint(gl_VertexID), 1, 1);
					    u_xlat4.y = float(u_xlatu6);
					    u_xlat5.xy = u_xlat4.xy + vec2(-0.5, -0.5);
					    u_xlat7.xyz = u_xlat1.xxx * u_xlat2.xzw;
					    u_xlat2.xyz = u_xlat1.xxx * u_xlat3.xyz;
					    u_xlat3.x = u_xlat7.x;
					    u_xlat3.y = u_xlat2.x;
					    u_xlat3.z = u_xlat0.x;
					    u_xlat5.z = 1.0;
					    u_xlat6 = dot(u_xlat3.xyz, u_xlat5.xyz);
					    u_xlat1.x = u_xlat7.y;
					    u_xlat1.y = u_xlat2.y;
					    u_xlat1.z = u_xlat0.z;
					    u_xlat1.x = dot(u_xlat1.xyz, u_xlat5.xyz);
					    u_xlat0.x = u_xlat7.z;
					    u_xlat0.z = u_xlat2.z;
					    u_xlat0.x = dot(u_xlat0.xzw, u_xlat5.xyz);
					    u_xlat1 = u_xlat1.xxxx * unity_ObjectToWorld[1];
					    u_xlat1 = unity_ObjectToWorld[0] * vec4(u_xlat6) + u_xlat1;
					    u_xlat0 = unity_ObjectToWorld[2] * u_xlat0.xxxx + u_xlat1;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_TEXCOORD0.xy = u_xlat4.xy;
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					"ps_5_0
					
					#version 430
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					precise vec4 u_xlat_precise_vec4;
					precise ivec4 u_xlat_precise_ivec4;
					precise bvec4 u_xlat_precise_bvec4;
					precise uvec4 u_xlat_precise_uvec4;
					UNITY_LOCATION(0) uniform  sampler2D mainTexture;
					layout(location = 0) in  vec2 vs_TEXCOORD0;
					layout(location = 1) flat in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = texture(mainTexture, vs_TEXCOORD0.xy);
					    u_xlat0 = u_xlat0 * vs_COLOR0;
					    SV_Target0.w = u_xlat0.w;
					    SV_Target0.w = clamp(SV_Target0.w, 0.0, 1.0);
					    SV_Target0.xyz = u_xlat0.xyz;
					    return;
					}"
				}
			}
		}
	}
}