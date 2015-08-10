

#ifdef XBOX
#define FXAA_360 1
#else
#define FXAA_PC 1
#endif
#define FXAA_HLSL_3 1
#define FXAA_GREEN_AS_LUMA 1

#include "fxaa3_11.fxh"

float4x4 projection;
texture txt;
sampler2D samp = sampler_state {
	Texture = <txt>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	AddressU = CLAMP;
	AddressV = CLAMP;
};
struct VShader {
	float4 Position : SV_Position;
	float4 TexCoord : TEXCOORD;
};

VShader VertexShaderFunction(VShader input) {
	VShader output;
	output.Position = mul(input.Position, projection);
	output.TexCoord = input.TexCoord;
	return output;

}


float2 InverseViewportSize;
float4 ConsoleSharpness;
float4 ConsoleOpt1;
float4 ConsoleOpt2;
float SubPixelAliasingRemoval;
float EdgeThreshold;
float EdgeThresholdMin;
float ConsoleEdgeSharpness;

float ConsoleEdgeThreshold;
float ConsoleEdgeThresholdMin;

// Must keep this as constant register instead of an immediate
float4 Console360ConstDir = float4(1.0, -1.0, 0.25, -0.25);

float4 RenderDrawerFunction(VShader input) : SV_Target0
{
	float4 theSample = tex2D(samp, input.TexCoord);

	float4 value = FxaaPixelShader(
		input.TexCoord,
		0,	// Not used in PC or Xbox 360
		samp,
		samp,			// *** TODO: For Xbox, can I use additional sampler with exponent bias of -1
		samp,			// *** TODO: For Xbox, can I use additional sampler with exponent bias of -2
		InverseViewportSize,	// FXAA Quality only
		ConsoleSharpness,		// Console only
		ConsoleOpt1,
		ConsoleOpt2,
		SubPixelAliasingRemoval,	// FXAA Quality only
		EdgeThreshold,// FXAA Quality only
		EdgeThresholdMin,
		ConsoleEdgeSharpness,
		ConsoleEdgeThreshold,	// TODO
		ConsoleEdgeThresholdMin, // TODO
		Console360ConstDir
		);

	return value;
}

technique Draw
{
	pass Obj {
#if SM4
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 RenderDrawerFunction();
#else
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 RenderDrawerFunction();
#endif
	}
}