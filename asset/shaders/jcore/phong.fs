#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D texture_diffuse1;
	sampler2D texture_normal1;
	sampler2D texture_specular1;
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

struct Attenuation
{
	float constant;
	float linear;
	float exp;
};

struct PointLight{
	BaseLight base;
	Attenuation attenuation;
	vec3 position;
};

struct SpotLight{
	PointLight base;
	vec3 direction;
	float cutoff;
};

#define MAX_DIR_LIGHTS 2
#define MAX_POINT_LIGHTS 10
#define MAX_SPOT_LIGHTS 10
uniform Material material;
uniform DirLight dirLights[MAX_DIR_LIGHTS];
uniform int dirLightNum;
uniform PointLight pointLights[MAX_POINT_LIGHTS];
uniform int pointLightNum;
uniform SpotLight spotLights[MAX_SPOT_LIGHTS];
uniform int spotLightNum;
uniform vec3 viewPos;
uniform vec3 g_Color;
uniform bool g_highLight;
uniform vec3 g_highLightColor;

in vec2 texCoord;
in vec3 fragPos;
in mat3 TBN;

//计算光照通用部分
vec3 calcLightCommon(BaseLight light, vec3 lightDirection, vec3 normal);
//计算方向光照
vec3 calcDirLight(DirLight light, vec3 normal, vec3 viewDir);
//计算点光照
vec3 calcPointLight(PointLight light, vec3 normal, vec3 viewDir);
//计算聚光
vec3 calcSpotLight(SpotLight light, vec3 normal, vec3 viewDir);

void main()
{
	//用TBN矩阵计算法向量
	vec3 normal = texture(material.texture_normal1, texCoord).rgb;
	normal = normalize(normal * 2.0 - 1.0);//to [-1,1]
	normal = normalize(TBN * normal);
	//单位化视线方向
	vec3 viewDir = normalize(viewPos - fragPos);
	vec3 result = vec3(0.0);
	//平行光照
	for (int i=0; i<dirLightNum; i++){
		result += calcDirLight(dirLights[i], normal, viewDir);
	}
	//点光照
	for (int i=0; i<pointLightNum; i++){
		result += calcPointLight(pointLights[i], normal, viewDir);
	}
	//聚光灯
	for (int i=0; i<spotLightNum; i++){
		result += calcSpotLight(spotLights[i], normal, viewDir);
	}
	
	//高亮效果
	if (g_highLight){
		float p = dot(viewDir, normal);
		result = mix(result, g_highLightColor, p);
	}
	
	FragColor = vec4(result, 1.0);
};

vec3 calcLightCommon(BaseLight light, vec3 lightDirection, vec3 normal, vec3 viewDir){
	//diffuse贴图颜色
	vec3 diffuseTex = texture(material.texture_diffuse1, texCoord).rgb;
	//specular贴图颜色
	vec3 specularTex = texture(material.texture_specular1, texCoord).rgb;
	//光源方向
	vec3 lightDir = lightDirection;
	//环境光
	vec3 ambient = light.color * diffuseTex * light.ambientIntensity;
	//漫反射
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse = light.color * light.diffuseIntensity * diffuseTex * diff;
	//镜面反射
	vec3 halfwayDir = normalize(lightDir + viewDir);
	float spec = pow(max(dot(normal, halfwayDir), 0.0), 32.0);
	vec3 specular = light.color * specularTex * spec;
	
	return (ambient + diffuse + specular);
}

vec3 calcDirLight(DirLight light, vec3 normal, vec3 viewDir){	
	return calcLightCommon(light.base, normalize(light.direction), normal, viewDir);
}

vec3 calcPointLight(PointLight light, vec3 normal, vec3 viewDir){
	vec3 lightDirection = light.position - fragPos;
	float distance = length(lightDirection);
	
	vec3 result = calcLightCommon(light.base, normalize(lightDirection), normal, viewDir);
	float attenuation = light.attenuation.constant + 
						light.attenuation.linear * distance +
						light.attenuation.exp * distance * distance;
	return result / attenuation;
}

vec3 calcSpotLight(SpotLight light, vec3 normal, vec3 viewDir){
	vec3 lightDirection = normalize(fragPos - light.base.position);
	float spotFactor = dot(lightDirection, normalize(light.direction));

	vec3 result = vec3(0.0);
	if (spotFactor > light.cutoff){
		result = calcPointLight(light.base, normal, viewDir);
		return result * (1.0 - (1.0 - spotFactor) * 1.0 / (1.0 - light.cutoff));
	}

	return result;
}