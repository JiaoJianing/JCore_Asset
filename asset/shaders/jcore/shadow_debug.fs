#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D texture_shadow;

void main()
{
	float depth = texture(texture_shadow, texCoord).r;
	FragColor = vec4(vec3(depth), 1.0);
};