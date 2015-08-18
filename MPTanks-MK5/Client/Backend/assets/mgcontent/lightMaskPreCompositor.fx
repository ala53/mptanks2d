float intensity;
sampler txt;
float4x4 transform;
float4 color;

struct VShader
{
	float4 Position : SV_Position;
	float2 TexCoord : TEXCOORD0;
};

VShader VertexShaderFunction(VShader input)
{
	VShader output;
	output.Position = mul(input.Position, transform);
	output.TexCoord = input.TexCoord;
	return output;
}

float4 PixelShaderFunction(VShader input) : SV_Target0
{
	return color;
	float4 mask = tex2D(txt, input.TexCoord);
	float4 colorized = color;
	colorized.a = mask.r * intensity;

	//r,g,b = color
	//a = intensity
	return colorized;
}

technique Draw
{
	pass Pass1
	{
#if SM4
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
#else
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
#endif
	}
}