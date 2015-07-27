float4x4 view;
float4x4 projection;
sampler txt;
struct VertexShaderInput
{
	float4 Position : SV_Position;
	float4 Offset : TEXCOORD0;
	float4 Size : TEXCOORD1;
	float4 RotationOrigin : TEXCOORD2;
	float4 Scale : TEXCOORD3;
	float Rotation : TEXCOORD4;
	float4 Color : COLOR;
	float4 TextureBounds : TEXCOORD5;
	float4 TexCoord : TEXCOORD6;
};

struct VertexShaderOutput
{
	float4 Position : SV_Position;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 pos = input.Position;
	pos.xy -= input.RotationOrigin.xy;
	//Rotation
	float4 rotTemp = pos;
	rotTemp.x = pos.x * cos(input.Rotation) - pos.y * sin(input.Rotation);
	rotTemp.y = pos.x * sin(input.Rotation) + pos.y * cos(input.Rotation);
	pos = rotTemp;
	pos.xy += input.RotationOrigin.xy;
	//Scaling and offset
	pos.xy -= input.Size.xy / float2(2, 2);
	pos.xy *= input.Scale.xy;
	pos.xy += input.Offset.xy;

	output.Position = mul(pos, projection);
	output.Color = input.Color;
	//Texture coordinate computation
	float2 texCoord = input.TexCoord;
	texCoord.xy *= input.TextureBounds.zw;
	texCoord.x = (texCoord.x % input.TextureBounds.z) + input.TextureBounds.x;
	texCoord.y = (texCoord.y % input.TextureBounds.w) + input.TextureBounds.y;
	output.TexCoord = texCoord;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target0
{
	return tex2D(txt, input.TexCoord) * input.Color;
}

technique Draw
{
	pass Pass1
	{
#if SM4
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
#else
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
#endif
	}
}