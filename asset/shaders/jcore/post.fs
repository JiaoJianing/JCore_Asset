#version 330 core
out vec4 FragColor;

uniform sampler2D scene;

in vec2 texCoord;

void main()
{
	vec3 texColor = texture(scene, texCoord).rgb;
	FragColor = vec4(texColor, 1.0);
};