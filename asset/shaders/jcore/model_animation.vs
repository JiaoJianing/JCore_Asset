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
uniform mat4 gBones[MAX_BONES];

out vec2 texCoord;
out vec3 normal;
out vec3 fragPos;

void main()						
{
    mat4 BoneTransform = gBones[aBoneIDs[0]] * aWeights[0];
    BoneTransform     += gBones[aBoneIDs[1]] * aWeights[1];
    BoneTransform     += gBones[aBoneIDs[2]] * aWeights[2];
    BoneTransform     += gBones[aBoneIDs[3]] * aWeights[3];

	gl_Position = projection * view * model * BoneTransform * vec4(aPos, 1.0);
	texCoord = aTexCoord;
	mat3 normalMat = transpose(inverse(mat3(model * BoneTransform)));
	normal = normalMat * aNormal;
	fragPos = vec3(model * BoneTransform * vec4(aPos, 1.0));
};