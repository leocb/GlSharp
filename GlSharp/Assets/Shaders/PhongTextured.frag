#version 330 core
out vec4 FragColor;  

in vec3 FragPos;
in vec3 Normal;
in vec3 VertColor;
in vec2 TexCoord;

uniform vec3 viewPos;

struct Light {
    vec3 position;
    vec3 direction;
    float cutOffStart;
    float cutOffEnd;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float Kc;
    float Kl;
    float Kq;
};

uniform Light light;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};
  
uniform Material material;

// Spot light
void main()
{

    // attenuation
    float distance = length(light.position - FragPos);
    float attenuation = 1.0 / (light.Kc + light.Kl * distance + light.Kq * (distance * distance));

    // ambient
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoord));

    // diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diffAmmount = max(dot(Normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * (diffAmmount * vec3(texture(material.diffuse, TexCoord)));

    // specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, Normal);  
    float specAmmount = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (specAmmount * vec3(texture(material.specular, TexCoord)));  

    // spotlight cutoff
    float theta = dot(lightDir, normalize(-light.direction));
    float epsilon = light.cutOffStart - light.cutOffEnd;
    float intensity = clamp((theta - light.cutOffEnd) / epsilon, 0.0, 1.0);    
    
    
    // final
    ambient  *= attenuation;
    diffuse  *= attenuation * intensity;
    specular *= attenuation * intensity;
    vec3 result = (ambient + diffuse + specular);// * VertColor.rgb;
    FragColor = vec4(result, 1.0);
}

// Point Light
//void main()
//{
//
//    // attenuation
//    float distance = length(light.position - FragPos);
//    float attenuation = 1.0 / (light.Kc + light.Kl * distance + light.Kq * (distance * distance));
//
//    // ambient
//    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoord));
//
//    // diffuse 
//    vec3 norm = normalize(Normal);
//    vec3 lightDir = normalize(light.position - FragPos);
//    float diffAmmount = max(dot(Normal, lightDir), 0.0);
//    vec3 diffuse = light.diffuse * (diffAmmount * vec3(texture(material.diffuse, TexCoord)));
//
//    // specular
//    vec3 viewDir = normalize(viewPos - FragPos);
//    vec3 reflectDir = reflect(-lightDir, Normal);  
//    float specAmmount = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
//    vec3 specular = light.specular * (specAmmount * vec3(texture(material.specular, TexCoord)));  
//
//    // final
//    ambient  *= attenuation;
//    diffuse  *= attenuation;
//    specular *= attenuation;
//    vec3 result = (ambient + diffuse + specular);// * VertColor.rgb;
//    FragColor = vec4(result, 1.0);
//}