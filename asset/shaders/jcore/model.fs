#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D texture_diffuse1;
	sampler2D texture_specular1;
	sampler2D texture_reflect1;
	sampler2D texture_normal1;
};

uniform Material material;

in vec2 texCoord;

void main()
{
	vec3 texColor = texture(material.texture_diffuse1, texCoord).rgb;
	FragColor = vec4(texColor, 1.0);
};