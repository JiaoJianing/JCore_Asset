#version 330 core
out vec4 FragColor;

uniform vec3 cubeColor;

in vec2 texCoord;

void main()
{
	FragColor = vec4(cubeColor, 1.0);
};