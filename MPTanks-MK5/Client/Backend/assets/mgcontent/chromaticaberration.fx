float2 offsetR, offsetG, offsetB;
texture rendering;
float sec;
sampler samp = sampler_state {
	Texture = <rendering>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	AddressU = WRAP;
	AddressV = WRAP;
};

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : SV_Target0
{
	return float4(tex2D(samp, texCoord + offsetR).r,
	tex2D(samp, texCoord + offsetG).g,
		tex2D(samp, texCoord + offsetB).b, 1);
}
float4 SinFuncPS(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_Target0
{
	texCoord.x += ((sin(sec / 1) * cos(sec / 9.73f)) *0.01);
	texCoord.y += ((sin(sec / 7.82) * cos(sec / 5.95f)) *0.02);
	return tex2D(samp, texCoord);
}

technique Aberrate
{
	pass Pass1
	{
#if SM4
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
#else
		PixelShader = compile ps_2_0 PixelShaderFunction();
#endif
	}
}
technique Sin
{
	pass Pass1
	{
#if SM4
		PixelShader = compile ps_4_0_level_9_1 SinFuncPS();
#else
		PixelShader = compile ps_2_0 SinFuncPS();
#endif
	}
}