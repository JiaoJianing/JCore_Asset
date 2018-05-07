#version 330 core
out vec4 FragColor;

uniform vec3 cubeColor;
uniform bool highLight;
uniform vec3 highLightColor;
uniform vec3 viewPos;

uniform bool useTexture;
uniform sampler2D cubeTexture;

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

void main()
{
	vec3 viewDir = normalize(viewPos - fragPos);
	vec3 norm = normalize(normal);

	vec3 result = cubeColor;
	if (useTexture){
		result = texture(cubeTexture, texCoord).rgb;
	}

	if (highLight){
		float p = dot(viewDir, norm);
		result = mix(highLightColor, result, p);
	}

	FragColor = vec4(result, 1.0);
};