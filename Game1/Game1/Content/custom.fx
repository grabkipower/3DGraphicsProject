#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

matrix WorldViewProjection;

const static float3  tab[29] =
{
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

const static float radius = 2.0f;
const static float d = 0.1f;

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

	float minVal = 100.0f;
	float minVal2 = 100.0f;
	int minInd = -1;
	int minInd2 = -1;
	int itval = 0;
	float difference;
	// TUTAJ TRZEBA ZMINIENIC WARTOSC NA 29, ALE WTEDY NIE BANGLA
	for (int i = 0; i < 18; i++)
	{
		float first = (input.Position[0] - ((radius * tab[i].x)));
		float second = (input.Position[1] - ((radius * tab[i].y)));
		float third = (input.Position[2] - ((radius * tab[i].z)));
		difference = sqrt( (first * first) +(second * second) + (third * third) );

		if (difference < minVal)
		{
			// tego ifa mozna sie pozbyc. Zostawiam dla testow, nie jestem taki glupi :p
		//	if (minVal < minVal2)
			{
				minVal2 = minVal;
				minInd2 = minInd;
			}
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
	if (dif < d)
		output.Color = float4(0, 0, 0, 1);
	else if (minInd < 12 )
		output.Color = float4(0, 0.1, 1, 1);
	else  
		output.Color = float4(1, 1, 1, 1);

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{

	///return float4(1, 0, 0, 1);
	return input.Color;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile vs_4_0_level_9_3   MainVS();
		PixelShader = compile ps_4_0_level_9_3   MainPS();
	}
};