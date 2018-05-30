#version 330 core					
layout (location=0) in vec3 aPos;
layout (location=1) in vec3 aNormal;
layout (location=2) in vec2 aTexCoord;
layout (location=5) in ivec4 aBoneIDs;
layout (location=6) in vec4 aWeights;	

const int MAX_BONES = 100;
uniform	mat4 view;
uniform	mat4 projection;
uniform mat4 model;
uniform mat4 gBones[MAX_BONES];

out VS_OUT{
	vec3 normal;
}vs_out;

void main()						
{			
    mat4 BoneTransform = gBones[aBoneIDs[0]] * aWeights[0];
    BoneTransform     += gBones[aBoneIDs[1]] * aWeights[1];
    BoneTransform     += gBones[aBoneIDs[2]] * aWeights[2];
    BoneTransform     += gBones[aBoneIDs[3]] * aWeights[3];

	gl_Position = projection * view * model * BoneTransform * vec4(aPos, 1.0);
	mat3 normalMat = mat3(transpose(inverse(view * model * BoneTransform)));
	vs_out.normal = normalize(vec3(projection * vec4(normalMat * aNormal, 0.0)));
};