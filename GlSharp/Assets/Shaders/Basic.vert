#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;
layout (location = 3) in vec3 aColor;
  
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoord;
out vec3 VertColor;

void main()
{
    // Rotate and deal with scale for the normals
    mat3 normalMatrix = mat3(transpose(inverse(model)));
    Normal = normalize(aNormal * normalMatrix);

    // vertex world positions for frag light
    FragPos = vec3(vec4(aPos, 1.0) * model);

    // other vertex attribs
    TexCoord = aTexCoord;
    VertColor = aColor;

    // final screen-space positions (mvp)
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
}