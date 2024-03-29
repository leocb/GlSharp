﻿#version 330 core
out vec4 FragColor;  

in vec3 FragPos;
in vec3 Normal;
in vec3 VertColor;

uniform vec3 viewPos;

struct Light {
    vec3 position;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float shininess;
};
  
uniform Material material;

void main()
{
    // ambient
    vec3 ambient = light.ambient * material.ambient;

    // diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diffAmmount = max(dot(Normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * (diffAmmount * material.diffuse);

    // specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, Normal);  
    float specAmmount = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (specAmmount * material.specular);  

    // final
    vec3 result = (ambient + diffuse + specular);// * VertColor.rgb;
    FragColor = vec4(result, 1.0);
}