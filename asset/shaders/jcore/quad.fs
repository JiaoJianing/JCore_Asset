#version 330 core
out vec4 FragColor;

uniform sampler2D texture1;

in vec2 texCoord;

void main()
{
	vec3 texColor = texture(texture1, texCoord).rgb;
	FragColor = vec4(texColor, 1.0f);

	//FragColor = vec4(1.0f, 0.0f, 1.0f, 1.0f);
};