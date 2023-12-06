#version 330 core
out vec4 FragColor;  

in vec3 FragPos;
in vec3 Normal;
in vec3 VertColor;

uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPos;
uniform vec3 viewPos;


void main()
{
    float specularStrength = 0.9;

    vec3 ambientLight = vec3(0.2, 0.3, 0.3);
    vec3 ambient = ambientLight * lightColor;

    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 lightDir = normalize(lightPos - FragPos);

    float diff = max(dot(Normal, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    vec3 reflectDir = reflect(-lightDir, Normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = spec * specularStrength * lightColor;

    vec3 result = (ambient + diffuse + specular) * VertColor.rgb * objectColor;
    FragColor = vec4(result, 1.0);
}