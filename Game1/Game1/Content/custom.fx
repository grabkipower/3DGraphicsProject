#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

matrix WorldViewProjection;

const static float3  tab[32] =
{
	float3(0.0000, 0.6045, 0.9780),
	float3(-0.0000, -0.6045, 0.9780),
	float3(0.9780, -0.0000, 0.6045),
	float3(0.6045f, -0.9780f, 0.0000f),
	float3(0.9780f, -0.0000f, -0.6045f),
	float3(-0.0000f, -0.6045f, -0.9780f),
	float3(0.0000f, 0.6045f, -0.9780f),
	float3(-0.9780f, 0.0000f, -0.6045f),
	float3(-0.6045f, 0.9780f, -0.0000f),
	float3(-0.9780f, 0.0000f, 0.6045f),
	float3(-0.6045f, -0.9780f, -0.0000f),
	float3(0.6045f, 0.9780f, 0.0000f),
	float3(0.3568f, -0.0000f, 0.9342f),
	float3(0.5774f, 0.5774f, 0.5774f),
	float3(0.0000f, 0.9342f, 0.3568f),
	float3(-0.5774f, 0.5774f, 0.5774f),
	float3(-0.3568f, 0.0000f, 0.9342f),
	float3(-0.5774f, -0.5774f, 0.5774f),
	float3(-0.0000f, -0.9342f, 0.3568f),
	float3(0.5774f, -0.5774f, 0.5774f),
	float3(0.9342f, 0.3568f, 0.0000f),
	float3(0.9342f, -0.3568f, 0.0000f),
	float3(-0.0000f, -0.9342f, -0.3568f),
	float3(0.5774f, -0.5774f, -0.5774f),
	float3(0.5774f, 0.5774f, -0.5774f),
	float3(0.3568f, -0.0000f, -0.9342f),
	float3(-0.5774f, -0.5774f, -0.5774f),
	float3(-0.3568f, 0.0000f, -0.9342f),
	float3(0.0000f, 0.9342f, -0.3568f),
	float3(-0.5774f, 0.5774f, -0.5774f),
	float3(-0.9342f, -0.3568f, -0.0000f),
	float3(-0.9342f, 0.3568f, -0.0000f)

};

const static float radius = 3.0f;
const static float d = 0.5f;

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
	output.Position = mul(input.Position, WorldViewProjection);

	float minVal = 1000.0f;
	float minVal2 = 1000.0f;
	int minInd = -1;
	int minInd2 = -1;
	int itval = 0;
	float difference;
	for (int i = 0; i < 32; i++)
	{
		float first = (input.Position.x - (radius * tab[i].x));
		float second = (input.Position.y - (radius * tab[i].y));
		float third = (input.Position.z - (radius * tab[i].z));
		difference = sqrt((first * first) + (second * second) + (third * third));

		if (difference < minVal)
		{
			minVal2 = minVal;
			minInd2 = minInd;
			minVal = difference;
			minInd = i;

		}
		else if (difference < minVal2)
		{
			minVal2 = difference;
			minInd2 = i;
		}

	}

	float dif = minVal2 - minVal;
	if (dif < 0)
		dif = dif*-1.0f;
	//if (dif <= 0.05f)
	//{
	//	output.Color = float4(1, 0, 0, 1);
	//	return output;
	//}
	if (minInd < 12)
	{
		if (dif < d)
		{
			float v = smoothstep(float4(0, 0, 0, 1), float4(0, 0, 1, 1), dif);
			output.Color = float4(0, 0, v, 1);
			return output;
		}
		output.Color = float4(0, 0, 1, 1);

	}
	else
	{
		if (dif < d)
		{
			float v = smoothstep(float4(0, 0, 0, 1), float4(1, 1, 1, 1), dif);
			output.Color = float4(v, v, v, 1);
			return output;
		}
		output.Color = float4(1, 1, 1, 1);
	}

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return input.Color;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile vs_4_0   MainVS();
		PixelShader = compile ps_4_0  MainPS();
	}
};