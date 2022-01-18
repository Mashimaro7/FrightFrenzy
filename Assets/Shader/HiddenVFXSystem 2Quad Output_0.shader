Shader "Hidden/VFX/System 2/Quad Output" {
	Properties {
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 45174
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
						vec4 Size_a;
						float Color_b;
					};
					UNITY_BINDING(1) uniform outputParams {
						float nbMax;
					};
					UNITY_BINDING(2) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[7];
					};
					UNITY_BINDING(3) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
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
					vec2 u_xlat0;
					int u_xlati0;
					uvec3 u_xlatu0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					float u_xlat3;
					ivec2 u_xlati3;
					uint u_xlatu3;
					bool u_xlatb3;
					vec2 u_xlat6;
					int u_xlati6;
					float u_xlat9;
					uint u_xlatu9;
					void main()
					{
						ImmCB_0_0_0[0] = vec4(1.0, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[1] = vec4(0.0, 1.0, 0.0, 0.0);
						ImmCB_0_0_0[2] = vec4(0.0, 0.0, 1.0, 0.0);
						ImmCB_0_0_0[3] = vec4(0.0, 0.0, 0.0, 1.0);
					    u_xlatu0.x = uint(gl_VertexID) >> 2u;
					    u_xlati3.x = gl_InstanceID << 11;
					    u_xlatu0.x = uint(u_xlati3.x) + u_xlatu0.x;
					    u_xlat3 = uintBitsToFloat(deadListCount_buf[(0 >> 2) + 0]);
					    u_xlatu3 = (-floatBitsToUint(u_xlat3)) + floatBitsToUint(nbMax);
					    u_xlatb3 = u_xlatu0.x>=u_xlatu3;
					    if(u_xlatb3){
					        gl_Position = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_COLOR0 = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_TEXCOORD0.xy = vec2(0.0, 0.0);
					        return;
					    }
					    u_xlati0 = int(indirectBuffer_buf[u_xlatu0.x].value[(0 >> 2) + 0]);
					    u_xlati0 = u_xlati0 << 1;
					    u_xlati3.xy = ivec2(u_xlati0) << ivec2(1, 2);
					    u_xlat3 = uintBitsToFloat(attributeBuffer_buf[(u_xlati3.x >> 2) + 0]);
					    u_xlati6 = u_xlati3.y + 9216;
					    u_xlat6.xy = vec2(uintBitsToFloat(attributeBuffer_buf[(u_xlati6 >> 2) + 0]), uintBitsToFloat(attributeBuffer_buf[(u_xlati6 >> 2) + 1]));
					    u_xlat3 = u_xlat6.y / u_xlat3;
					    u_xlat1.x = u_xlat3 * Size_a.x + Size_a.y;
					    u_xlatu9 = floatBitsToUint(Size_a.w) >> 2u;
					    switch(int(u_xlatu9)){
					        case 1:
					            u_xlat9 = min(u_xlat1.x, 1.0);
					            u_xlat9 = fract(u_xlat9);
					            u_xlat1.x = u_xlat9 * 0.9921875 + 0.00390625;
					            break;
					        case 2:
					            u_xlat9 = max(u_xlat1.x, 0.0);
					            u_xlat9 = fract(u_xlat9);
					            u_xlat1.x = u_xlat9 * 0.9921875 + 0.00390625;
					            break;
					        case 3:
					            u_xlat1.x = u_xlat1.x;
					            u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
					            u_xlat1.x = u_xlat1.x * 0.9921875 + 0.00390625;
					            break;
					        default:
					            break;
					    }
					    if(floatBitsToUint(u_xlat6.x) == uint(0)) {
					        gl_Position = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_COLOR0 = vec4(0.0, 0.0, 0.0, 0.0);
					        vs_TEXCOORD0.xy = vec2(0.0, 0.0);
					        return;
					    }
					    u_xlati0 = u_xlati0 << 3;
					    u_xlati0 = u_xlati0 + 1024;
					    u_xlat2.yzw = vec3(uintBitsToFloat(attributeBuffer_buf[(u_xlati0 >> 2) + 0]), uintBitsToFloat(attributeBuffer_buf[(u_xlati0 >> 2) + 1]), uintBitsToFloat(attributeBuffer_buf[(u_xlati0 >> 2) + 2]));
					    u_xlati0 = int(floatBitsToUint(Size_a.w) & 3u);
					    u_xlat1.y = Size_a.z;
					    u_xlat1 = textureLod(bakedTexture, u_xlat1.xy, 0.0);
					    u_xlat2.x = dot(u_xlat1, ImmCB_0_0_0[u_xlati0]);
					    u_xlat3 = u_xlat3;
					    u_xlat3 = clamp(u_xlat3, 0.0, 1.0);
					    u_xlat0.x = u_xlat3 * 0.9921875 + 0.00390625;
					    u_xlat0.y = Color_b;
					    vs_COLOR0 = textureLod(bakedTexture, u_xlat0.xy, 0.0);
					    u_xlatu0.x = uint(gl_VertexID) & 1u;
					    u_xlatu0.z = bitfieldExtract(uint(gl_VertexID), 1, 1);
					    u_xlat0.xy = vec2(u_xlatu0.xz);
					    u_xlat1.xy = u_xlat0.xy + vec2(-0.5, -0.5);
					    u_xlat1.z = 1.0;
					    u_xlat6.x = dot(u_xlat2.xy, u_xlat1.xz);
					    u_xlat9 = dot(u_xlat2.xz, u_xlat1.yz);
					    u_xlat1 = vec4(u_xlat9) * unity_ObjectToWorld[1];
					    u_xlat1 = unity_ObjectToWorld[0] * u_xlat6.xxxx + u_xlat1;
					    u_xlat1 = unity_ObjectToWorld[2] * u_xlat2.wwww + u_xlat1;
					    u_xlat1 = u_xlat1 + unity_ObjectToWorld[3];
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    gl_Position = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    vs_TEXCOORD0.xy = u_xlat0.xy;
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