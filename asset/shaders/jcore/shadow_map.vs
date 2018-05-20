#version 330 core					
layout (location=0) in vec3 aPos;
layout (location=1) in vec3 aNormal;
layout (location=2) in vec2 aTexCoord;

uniform mat4 model;
uniform mat4 lightSpaceMat;

out vec2 texCoord;

void main()						
{
	gl_Position = lightSpaceMat * model * vec4(aPos, 1.0);
	texCoord = aTexCoord;
};