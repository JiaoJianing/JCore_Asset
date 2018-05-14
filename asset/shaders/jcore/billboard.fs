#version 330 core
out vec4 FragColor;

uniform sampler2D texture_billboard;

in vec2 texCoord;

void main()
{
	FragColor = texture(texture_billboard, vec2(texCoord.x, 1.0 - texCoord.y));
	if (FragColor.a <= 0.01){
		discard;
	}
};