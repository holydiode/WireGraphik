﻿#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;


out vec3 FragPos;
out vec2 TexCoords;
out vec3 Normal;


uniform mat4 transform;
uniform mat4 projection;
uniform mat4 view;

void main()
{
	gl_Position = projection * view * transform * vec4(aPos, 1.0);
	TexCoords = vec2(aTexCoord.x, aTexCoord.y);
	FragPos = vec3(transform * vec4(aPos, 1.0));
	Normal = aNormal;
}