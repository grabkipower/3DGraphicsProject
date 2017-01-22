#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif


#define NUMSPOTLIGHTS 2 
#define NUMPOINTLIGHTS 2


float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightDirection = float3(1, 0, 0);

texture BasicTexture;
float3 DiffuseColor = float3(.85, .85, .85);
sampler BasicTextureSampler = sampler_state
{
    texture = <BasicTexture>;
};

texture ReflectionCubeMap;
sampler ReflectionCubeMapSampler = sampler_state
{
    texture = <ReflectionCubeMap>;
};

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

   // float3 color = tex2D(BasicTextureSampler, input.UV).rgb;
    //float3 color2 = tex2D(ReflectionCubeMapSampler, input.UV).rgb;


    //float3 output = color2;

    //return float4(output, 1);
    float Diff = saturate(dot(input.UV, LightDirection));
    float3 Reflect = normalize(2 * Diff * (input.Normal - LightDirection));
    float3 ReflectColor = tex2D(ReflectionCubeMapSampler, Reflect);


    return float4(ReflectColor,1);
 
}

technique Ambient
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}

