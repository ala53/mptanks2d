float4x4 projection;
sampler lightMap;
sampler world;
float4 AmbientColor = float4(0.1, 0.1, 0.1, 0.1);
float AmbientIntensity = 0.15;

struct VShader
{
	float4 Position : SV_Position;
	float2 TexCoord : TEXCOORD0;
};

VShader VertexShaderFunction(VShader input)
{
	VShader output;
	output.Position = mul(input.Position, projection);
	output.TexCoord = input.TexCoord;
	return output;
}

float4 PixelShaderFunction(VShader input) : SV_Target0
{
	float4 lightData = tex2D(lightMap, input.TexCoord);
	float lightIntensity = lightData.a;
	float4 worldColor = tex2D(world, input.TexCoord);
	
	float brightness = ((1 - AmbientIntensity) * lightIntensity) + AmbientIntensity;

	return float4(worldColor.rgb * brightness, 1) * ((lightData * brightness) + (AmbientColor * (1 - brightness)));
}

technique Composite
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