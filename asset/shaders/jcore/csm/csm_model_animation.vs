#version 330 core					
layout (location=0) in vec3 aPos;
layout (location=1) in vec3 aNormal;
layout (location=2) in vec2 aTexCoord;
layout (location=3) in vec3 aTangent;
layout (location=4) in vec3 aBiTangent;
layout (location=5) in ivec4 aBoneIDs;
layout (location=6) in vec4 aWeights;

const int MAX_BONES = 100;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 lightViewProj[3];
uniform mat4 gBones[MAX_BONES];

out vec2 texCoord;

out vec4 lightSpacePos[3];
out float clipSpacePosZ;
out vec3 fragPos;
out mat3 TBN;

void main()						
{			
    mat4 BoneTransform = gBones[aBoneIDs[0]] * aWeights[0];
    BoneTransform     += gBones[aBoneIDs[1]] * aWeights[1];
    BoneTransform     += gBones[aBoneIDs[2]] * aWeights[2];
    BoneTransform     += gBones[aBoneIDs[3]] * aWeights[3];

	vec4 pos = BoneTransform * vec4(aPos, 1.0);
	gl_Position = projection * view * model * pos;
	texCoord = aTexCoord;
	fragPos = vec3(model * BoneTransform * vec4(aPos, 1.0));

	for (int i=0; i<3; i++){
		lightSpacePos[i] = lightViewProj[i] * model * pos;
	}
	clipSpacePosZ = gl_Position.z;

	//¼ÆËãTBN¾ØÕó
	vec3 T = normalize(vec3(model * BoneTransform * vec4(aTangent, 0.0)));
	vec3 N = normalize(vec3(model * BoneTransform * vec4(aNormal, 0.0)));
	T = normalize(T - dot(T, N) * N);
	vec3 B = cross(T, N);
	TBN = mat3(T, B, N);
};