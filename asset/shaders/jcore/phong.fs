#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D texture_diffuse1;
	sampler2D texture_normal1;
	sampler2D texture_specular1;
};

struct DirLight{
	vec3 lightColor;
	vec3 lightPos;
	float ambientIntensity;
	float diffuseIntensity;
};

uniform Material material;
uniform DirLight dirLight;
uniform vec3 viewPos;
uniform vec3 g_Color;
uniform vec3 g_highLightColor;

in vec2 texCoord;
in vec3 fragPos;
in mat3 TBN;

//计算方向光照
vec3 calcDirLight(DirLight light, vec3 normal, vec3 viewDir);

void main()
{
	//用TBN矩阵计算法向量
	vec3 normal = texture(material.texture_normal1, texCoord).rgb;
	normal = normalize(normal * 2.0 - 1.0);//to [-1,1]
	normal = normalize(TBN * normal);
	//单位化视线方向
	vec3 viewDir = normalize(viewPos - fragPos);
	
	vec3 temp = texture(material.texture_normal1, texCoord).rgb;
	
	vec3 result = calcDirLight(dirLight, normal, viewDir);
	
	if (g_highLight){
		float p = dot(viewDir, normal);
		result = mix(result, g_highLightColor, p);
	}
	
	FragColor = vec4(result, 1.0);
};

vec3 calcDirLight(DirLight light, vec3 normal, vec3 viewDir){
	//diffuse贴图颜色
	vec3 diffuseTex = texture(material.texture_diffuse1, texCoord).rgb;
	//specular贴图颜色
	vec3 specularTex = texture(material.texture_specular1, texCoord).rgb;
	//光源方向
	vec3 lightDir = normalize(light.lightPos);
	//环境光
	vec3 ambient = light.lightColor * diffuseTex * light.ambientIntensity;
	//漫反射
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse = light.lightColor * light.diffuseIntensity * diffuseTex * diff;
	//镜面反射
	vec3 halfwayDir = normalize(lightDir + viewDir);
	float spec = pow(max(dot(normal, halfwayDir), 0.0), 32.0);
	vec3 specular = light.lightColor * specularTex * spec;
	
	return (ambient + diffuse + specular);
}