#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D texture_diffuse1;
	sampler2D texture_normal1;
	sampler2D texture_specular1;
};

uniform Material material;
uniform vec3 viewPos;
uniform bool g_highLight;
uniform vec3 g_highLightColor;
uniform vec3 g_Color;

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

void main()
{
	vec3 viewDir = normalize(viewPos - fragPos);
	vec3 norm = normalize(normal);

	vec3 texColor = texture(material.texture_diffuse1, texCoord).rgb;

	if (g_highLight){
		float p = dot(viewDir, norm);
		texColor = mix(texColor, g_highLightColor, p);
	}

	FragColor = vec4(texColor, 1.0);
};