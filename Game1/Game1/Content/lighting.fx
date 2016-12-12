#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif
#define NUMLIGHTS 2

float4x4 World;
float4x4 View;
float4x4 Projection;

float3 AmbientLightColor = float3(.15, .15, .15);
float3 DiffuseColor = float3(.85, .85, .85);

// Point Light:
float3 LightPosition = float3(-5, -5, 80);
float3 LightColor = float3(1, 1, 1);
float LightAttenuation = 30000;
float LightFalloff = 2;

float3 PointLightColors[NUMLIGHTS];
float3 PointLightPositions[NUMLIGHTS];


// Spot Light:
float ConeAngle = 100;
float3 SpotLightColor = float3(1, 0, 1);
float SpotLightFalloff = 200;


// Spot Lights
float3 SpotLightPosition = float3(0, -10, 0);
float3 LightDirection = float3(0, -1, 0);

float3 SpotLightDirections[NUMLIGHTS];
float3 SpotLightColors[NUMLIGHTS];
float3 SpotLightPositions[NUMLIGHTS];




texture BasicTexture;




sampler BasicTextureSampler = sampler_state {
	texture = <BasicTexture>;
};

bool TextureEnabled = true;

bool SpotLight = true;
bool PointLight = true;


struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
};
struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float4 WorldPosition : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.WorldPosition = worldPosition;
	output.UV = input.UV;
	output.Normal = mul(input.Normal, World);

	return output;
}


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 diffuseColor = DiffuseColor;

	if (TextureEnabled)
		diffuseColor *= tex2D(BasicTextureSampler, input.UV).rgb;
	float3 totalLight = float3(0, 0, 0);

	totalLight += AmbientLightColor;


	if (SpotLight)
	for (int i = 0; i < NUMLIGHTS; i++)
	{
		float3 lightDir = normalize(SpotLightPositions[i] - input.WorldPosition);
		float diffuse = saturate(dot(normalize(input.Normal), lightDir));

		// (dot(p - lp, ld) / cos(a))^f
		float d = dot(-lightDir, normalize(SpotLightDirections[i]));
		float a = cos(ConeAngle);

		float att = 0;

		if (a < d)
			att = 1 - pow(clamp(a / d, 0, 1), SpotLightFalloff);

		totalLight += diffuse * att * SpotLightColors[i];
	}

	if (PointLight)
	for (int i = 0; i < NUMLIGHTS; i++)
	{
		float3 lightDir = normalize(PointLightPositions[i] - input.WorldPosition);
		float diffuse = saturate(dot(normalize(input.Normal), lightDir));
		float d = distance(PointLightPositions[i], input.WorldPosition);
		float att = 1 - pow(clamp(d / LightAttenuation, 0, 1),
			LightFalloff);

		totalLight += diffuse * att * PointLightColors[i];

	}
	return float4(diffuseColor * totalLight, 1);
	return float4(0, 0, 0, 0);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
};