#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec2 aTexCoord;
  
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 TexCoord;
out vec3 VertColor;

void main()
{
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
    TexCoord = aTexCoord;
    VertColor = aColor;
}   