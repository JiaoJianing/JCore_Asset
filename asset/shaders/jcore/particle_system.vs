#version 330 core
layout(location=0) in vec3 instancePos;
layout(location=1) in vec3 instanceColor;
layout(location=2) in float instanceSize;

out vec3 outcolor;
out float outsize;

void main(){
	gl_Position = vec4(instancePos, 1.0);

	outcolor = instanceColor;
	outsize = instanceSize;
}