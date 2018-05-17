#version 330 core
layout(points) in;
layout(triangle_strip, max_vertices=4) out;

uniform mat4 view;
uniform mat4 projection;
uniform vec3 viewPos;

in vec3 outcolor[];
in float outsize[];

out vec2 texCoord;
out vec3 color;

void main(){
	mat4 viewProj = projection * view;
	vec3 worldPos = gl_in[0].gl_Position.xyz;
	vec3 viewDir = normalize(viewPos - worldPos);
	vec3 upDir = vec3(0.0, 1.0, 0.0);
	vec3 right = cross(viewDir, upDir);
	
	worldPos -= (right * outsize[0] / 2);
    gl_Position = viewProj * vec4(worldPos, 1.0);
    texCoord = vec2(0.0, 0.0);
	color = outcolor[0];
    EmitVertex();

    worldPos.y += outsize[0];
    gl_Position = viewProj * vec4(worldPos, 1.0);
    texCoord = vec2(0.0, 1.0);
	color = outcolor[0];
    EmitVertex();

    worldPos.y -= outsize[0];
    worldPos += right * outsize[0];
    gl_Position = viewProj * vec4(worldPos, 1.0);
    texCoord = vec2(1.0, 0.0);
	color = outcolor[0];
    EmitVertex();

    worldPos.y += outsize[0];
    gl_Position = viewProj * vec4(worldPos, 1.0);
    texCoord = vec2(1.0, 1.0);
	color = outcolor[0];
    EmitVertex();

    EndPrimitive();
}