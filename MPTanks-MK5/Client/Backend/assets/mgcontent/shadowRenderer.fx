float2 shadowOffset;
float4 shadowColor;
float4x4 projection;
texture txt;
sampler samp = sampler_state {
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

float4 ShadowSamplerFunction(VShader input) : SV_Target0
{
	float4 color = tex2D(samp, input.TexCoord.xy + shadowOffset);
	return color.a * shadowColor;
}

float4 RenderDrawerFunction(VShader input) : SV_Target0
{
	return tex2D(samp, input.TexCoord.xy);
}

technique Draw
{
	pass Pass1
	{
#if SM4
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_1 ShadowSamplerFunction();
#else
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 ShadowSamplerFunction();
#endif
	}

	pass Pass2 {
#if SM4
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_1 RenderDrawerFunction();
#else
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 RenderDrawerFunction();
#endif
	}
}