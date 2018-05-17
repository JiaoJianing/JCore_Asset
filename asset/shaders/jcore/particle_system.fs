#version 330 core
out vec4 FragColor;

in vec2 texCoord;
in vec3 color;

uniform sampler2D texture_particle;

void main(){
	FragColor = texture(texture_particle, texCoord) * vec4(color, 1.0);

	if (FragColor.r <= 0.01 && FragColor.g <= 0.01 && FragColor.b <= 0.01){
		discard;
	}
}