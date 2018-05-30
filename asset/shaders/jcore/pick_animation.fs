#version 330 core
out vec4 FragColor;

uniform int nodeID;

void main()
{
	FragColor = vec4(vec3(nodeID), 1.0f);
};