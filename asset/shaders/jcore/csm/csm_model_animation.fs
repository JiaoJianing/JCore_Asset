#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D texture_diffuse1;
};

struct BaseLight{
	vec3 color;
	float ambientIntensity;
	float diffuseIntensity; 
};

struct DirLight{
	BaseLight base;
	vec3 direction;
};

uniform Material material;
uniform DirLight dirLight;

uniform vec3 viewPos;
uniform bool g_highLight;
uniform vec3 g_highLightColor;
uniform sampler2D texture_shadow[3];

in vec2 texCoord;
in vec3 fragPos;
in vec4 lightSpacePos[3];
uniform float cascadeSpace[3];
in vec3 worldNormal;
in float clipSpacePosZ;

//计算光照通用部分
vec3 calcLightCommon(BaseLight light, vec3 lightDirection, vec3 normal, float shadowFactor);
//计算方向光照
vec3 calcDirLight(DirLight light, vec3 normal, vec3 viewDir, float shadowFactor);

//计算shadowmap
float calcShadowFactor(int index, vec4 lSpacePos);

void main()
{
	//世界法向量
	vec3 normal = normalize(worldNormal);
	//计算shadowmap	在3张阴影贴图间插值
	float shadowFactor = 0.0;
	if (clipSpacePosZ <= cascadeSpace[0]){
		float ShadowFactor0 = calcShadowFactor(0, lightSpacePos[0]);
		float ShadowFactor1 = calcShadowFactor(1, lightSpacePos[1]);
		shadowFactor = mix(ShadowFactor0, ShadowFactor1, clipSpacePosZ / cascadeSpace[0]);
	}else if (clipSpacePosZ <= cascadeSpace[1]){
		float ShadowFactor1 = calcShadowFactor(1, lightSpacePos[1]);
		float ShadowFactor2 = calcShadowFactor(2, lightSpacePos[2]);
		shadowFactor = mix(ShadowFactor1, ShadowFactor2, (clipSpacePosZ-cascadeSpace[0]) / (cascadeSpace[1]-cascadeSpace[0]));
	}else if (clipSpacePosZ <= cascadeSpace[2]){
		float ShadowFactor2 = calcShadowFactor(2, lightSpacePos[2]);
		shadowFactor = ShadowFactor2;
	}
	
	//单位化视线方向
	vec3 viewDir = normalize(viewPos - fragPos);
	vec3 result = vec3(0.0);
	//平行光照
	result += calcDirLight(dirLight, normal, viewDir, shadowFactor);
	
	//高亮效果
	if (g_highLight){
		float p = dot(viewDir, normal);
		result = mix(result, g_highLightColor, p);
	}
	
	FragColor = vec4(result, 1.0);
};

vec3 calcLightCommon(BaseLight light, vec3 lightDirection, vec3 normal, vec3 viewDir, float shadowFactor){
	//diffuse贴图颜色
	vec3 diffuseTex = texture(material.texture_diffuse1, texCoord).rgb;
	//光源方向
	vec3 lightDir = lightDirection;
	//环境光
	vec3 ambient = light.color * diffuseTex * light.ambientIntensity;
	//漫反射
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse = light.color * light.diffuseIntensity * diffuseTex * diff;
	
	return (ambient + (1-shadowFactor) * diffuse);
}

vec3 calcDirLight(DirLight light, vec3 normal, vec3 viewDir, float shadowFactor){	
	return calcLightCommon(light.base, normalize(light.direction), normal, viewDir, shadowFactor);
}

float calcShadowFactor(int index, vec4 lSpacePos){
	vec3 projCoords = lSpacePos.xyz / lSpacePos.w;
	projCoords = projCoords * 0.5 + 0.5;
    float currentDepth = projCoords.z;

	float bias = 0.001;
	float shadow = 0.0;
	vec2 texelSize = 1.0 / textureSize(texture_shadow[index], 0);
	for(int x = -1; x <= 1; ++x)
	{
		for(int y = -1; y <= 1; ++y)
		{
			float pcfDepth = texture(texture_shadow[index], projCoords.xy + vec2(x, y) * texelSize).r; 
			shadow += currentDepth - bias > pcfDepth ? 1.0 : 0.0;        
		}    
	}
	shadow /= 9.0;

	if (projCoords.z > 1.0){
		shadow = 0.0;
	}

	return shadow;
}