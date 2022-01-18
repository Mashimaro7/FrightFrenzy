Shader "Hidden/VFX/System 3/Quad Output" {
	Properties {
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 22337
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
						vec4 Alpha_d;
						vec3 Color_c;
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
					vec3 u_xlat6;
					ivec2 u_xlati6;
					uint u_xlatu6;
					bool u_xlatb6;
					vec2 u_xlat12;
					int u_xlati12;
					float u_xlat13;
					uint u_xlatu13;
					float u_xlat18;
					uint u_xlatu18;
					void main()
					{
						ImmCB_0_0_0[0] = vec4(1.0, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[1] = vec4(0.0, 1.0, 0.0, 0.0);
						ImmCB_0_0_0[2] = vec4(0.0, 0.0, 1.0, 0.0);
						ImmCB_0_0_0[3] = vec4(0.0, 0.0, 0.0, 1.0);
					    u_xlatu0 = uint(gl_VertexID) >> 2u;
					    u_xlati6.x = gl_InstanceID << 11;
					    u_xlatu0 = uint(u_xlati6.x) + u_xlatu0;
					    u_xlat6.x = uintBitsToFloat(deadListCount_buf[(0 >> 2) + 0]);
					    u_xlatu6 = (-floatBitsToUint(u_xlat6.x)) + floatBitsToUint(nbMax);
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
					    u_xlati6.xy = u_xlati6.xy + ivec2(1664, 1872);
					    u_xlat6.x = uintBitsToFloat(attributeBuffer_buf[(u_xlati6.x >> 2) + 0]);
					    u_xlat12.xy = vec2(uintBitsToFloat(attributeBuffer_buf[(u_xlati6.y >> 2) + 0]), uintBitsToFloat(attributeBuffer_buf[(u_xlati6.y >> 2) + 1]));
					    u_xlat6.x = u_xlat12.y / u_xlat6.x;
					    u_xlat1.x = u_xlat6.x * Size_b.x + Size_b.y;
					    u_xlatu18 = floatBitsToUint(Size_b.w) >> 2u;
					    switch(int(u_xlatu18)){
					        case 1:
					            u_xlat18 = min(u_xlat1.x, 1.0);
					            u_xlat18 = fract(u_xlat18);
					            u_xlat1.x = u_xlat18 * 0.9921875 + 0.00390625;
					            break;
					        case 2:
					            u_xlat18 = max(u_xlat1.x, 0.0);
					            u_xlat18 = fract(u_xlat18);
					            u_xlat1.x = u_xlat18 * 0.9921875 + 0.00390625;
					            break;
					        case 3:
					            u_xlat1.x = u_xlat1.x;
					            u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
					            u_xlat1.x = u_xlat1.x * 0.9921875 + 0.00390625;
					            break;
					        default:
					            break;
					    }
					    u_xlat6.x = u_xlat6.x * Alpha_d.x + Alpha_d.y;
					    u_xlatu13 = floatBitsToUint(Alpha_d.w) >> 2u;
					    switch(int(u_xlatu13)){
					        case 1:
					            u_xlat13 = min(u_xlat6.x, 1.0);
					            u_xlat13 = fract(u_xlat13);
					            u_xlat6.x = u_xlat13 * 0.9921875 + 0.00390625;
					            break;
					        case 2:
					            u_xlat13 = max(u_xlat6.x, 0.0);
					            u_xlat13 = fract(u_xlat13);
					            u_xlat6.x = u_xlat13 * 0.9921875 + 0.00390625;
					            break;
					        case 3:
					            u_xlat6.x = u_xlat6.x;
					            u_xlat6.x = clamp(u_xlat6.x, 0.0, 1.0);
					            u_xlat6.x = u_xlat6.x * 0.9921875 + 0.00390625;
					            break;
					        default:
					            break;
					    }
					    if(floatBitsToUint(u_xlat12.x) == uint(0)) {
					        gl_Position = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_COLOR0 = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_TEXCOORD0.xy = vec2(0.0, 0.0);
					        return;
					    }
					    u_xlati0 = u_xlati0 << 3;
					    u_xlati0 = u_xlati0 + 832;
					    u_xlat2.xyz = vec3(uintBitsToFloat(attributeBuffer_buf[(u_xlati0 >> 2) + 0]), uintBitsToFloat(attributeBuffer_buf[(u_xlati0 >> 2) + 1]), uintBitsToFloat(attributeBuffer_buf[(u_xlati0 >> 2) + 2]));
					    u_xlat0.xz = unity_ObjectToWorld[0].yy * unity_MatrixV[1].xy;
					    u_xlat0.xz = unity_MatrixV[0].xy * unity_ObjectToWorld[0].xx + u_xlat0.xz;
					    u_xlat3.xy = unity_MatrixV[2].xy * unity_ObjectToWorld[0].zz + u_xlat0.xz;
					    u_xlat0.xz = unity_ObjectToWorld[1].yy * unity_MatrixV[1].xy;
					    u_xlat0.xz = unity_MatrixV[0].xy * unity_ObjectToWorld[1].xx + u_xlat0.xz;
					    u_xlat4.xy = unity_MatrixV[2].xy * unity_ObjectToWorld[1].zz + u_xlat0.xz;
					    u_xlat0.xz = unity_ObjectToWorld[2].yy * unity_MatrixV[1].xy;
					    u_xlat0.xz = unity_MatrixV[0].xy * unity_ObjectToWorld[2].xx + u_xlat0.xz;
					    u_xlat0.xz = unity_MatrixV[2].xy * unity_ObjectToWorld[2].zz + u_xlat0.xz;
					    u_xlat3.z = u_xlat4.x;
					    u_xlat3.w = u_xlat0.x;
					    u_xlat0.x = dot(u_xlat3.xzw, u_xlat3.xzw);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat3.xzw = u_xlat0.xxx * u_xlat3.xzw;
					    u_xlat4.z = u_xlat3.y;
					    u_xlat4.w = u_xlat0.z;
					    u_xlat0.x = dot(u_xlat4.yzw, u_xlat4.yzw);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat4.xyz = u_xlat0.xxx * u_xlat4.zyw;
					    u_xlati0 = int(floatBitsToUint(Size_b.w) & 3u);
					    u_xlat1.y = Size_b.z;
					    u_xlat1 = textureLod(bakedTexture, u_xlat1.xy, 0.0);
					    u_xlat0.x = dot(u_xlat1, ImmCB_0_0_0[u_xlati0]);
					    u_xlati12 = int(floatBitsToUint(Alpha_d.w) & 3u);
					    u_xlat6.z = Alpha_d.z;
					    u_xlat1 = textureLod(bakedTexture, u_xlat6.xz, 0.0);
					    vs_COLOR0.w = dot(u_xlat1, ImmCB_0_0_0[u_xlati12]);
					    u_xlatu6 = uint(gl_VertexID) & 1u;
					    u_xlat1.x = float(u_xlatu6);
					    u_xlatu6 = bitfieldExtract(uint(gl_VertexID), 1, 1);
					    u_xlat1.y = float(u_xlatu6);
					    u_xlat5.xy = u_xlat1.xy + vec2(-0.5, -0.5);
					    u_xlat6.xyz = u_xlat0.xxx * u_xlat3.xzw;
					    u_xlat3.xyz = u_xlat0.xxx * u_xlat4.xyz;
					    u_xlat4.x = u_xlat6.x;
					    u_xlat4.y = u_xlat3.x;
					    u_xlat4.z = u_xlat2.x;
					    u_xlat5.z = 1.0;
					    u_xlat0.x = dot(u_xlat4.xyz, u_xlat5.xyz);
					    u_xlat4.x = u_xlat6.y;
					    u_xlat4.y = u_xlat3.y;
					    u_xlat4.z = u_xlat2.y;
					    u_xlat6.x = dot(u_xlat4.xyz, u_xlat5.xyz);
					    u_xlat2.x = u_xlat6.z;
					    u_xlat2.y = u_xlat3.z;
					    u_xlat12.x = dot(u_xlat2.xyz, u_xlat5.xyz);
					    u_xlat2 = u_xlat6.xxxx * unity_ObjectToWorld[1];
					    u_xlat2 = unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat2;
					    u_xlat0 = unity_ObjectToWorld[2] * u_xlat12.xxxx + u_xlat2;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat2;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat2;
					    vs_COLOR0.xyz = Color_c.xyz;
					    vs_TEXCOORD0.xy = u_xlat1.xy;
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