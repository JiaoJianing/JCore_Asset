#version 330 core

out vec4 FragColor;

in vec3 fragPos;
in vec3 normal;
in vec2 texCoord;

uniform vec3 viewPos;
uniform sampler2D texture1;
uniform vec3 lightPos;
uniform bool blinn;

void main()
{
	vec3 texColor = texture(texture1, texCoord).rgb;
	//环境光
	vec3 ambient = texColor * 0.05;
	//漫反射
	vec3 lightDir = normalize(lightPos - fragPos);
	vec3 norNormal = normalize(normal);
	float diff = max(dot(lightDir, norNormal), 0.0);
	vec3 diffuse = texColor * diff;
	//镜面光
	vec3 viewDir = normalize(viewPos - fragPos);
	float spec = 0.0;
	if (blinn){
		vec3 halfWayDir = normalize(lightDir + viewDir);
		spec = pow(max(dot(halfWayDir, norNormal), 0.0), 16);
	}else{	
		vec3 reflectDir = reflect(-lightDir, norNormal);
		spec = pow(max(dot(reflectDir, viewDir), 0.0), 16);
	}
	vec3 specular = vec3(0.3) * spec;
	
	FragColor = vec4(ambient + diffuse + specular, 1.0);
};