#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D texture_diffuse1;
	sampler2D texture_specular1;
	sampler2D texture_reflect1;
	sampler2D texture_normal1;
};

uniform Material material;
uniform bool highLight;
uniform vec3 highLightColor;
uniform vec3 viewPos;

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

void main()
{
	vec3 viewDir = normalize(viewPos - fragPos);
	vec3 norm = normalize(normal);

	vec3 texColor = texture(material.texture_diffuse1, texCoord).rgb;

	if (highLight){
		float p = dot(viewDir, norm);
		texColor = mix(highLightColor, texColor, p);
	}

	FragColor = vec4(texColor, 1.0);
};