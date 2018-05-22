#version 330 core

out vec4 FragColor;

in vec2 texCoord;
in vec3 fragPos;

uniform sampler2D texture_grass;
uniform sampler2D texture_rock;
uniform sampler2D texture_snow;

void main()
{
	FragColor = texture(texture_rock, texCoord);
};