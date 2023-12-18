#version 330 core
out vec4 FragColor;  

in vec3 FragPos;
in vec3 Normal;
in vec3 VertColor;
in vec2 TexCoord;

uniform vec3 viewPos;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};
uniform Material material;

struct DirLight {
    vec3 direction;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};  
uniform DirLight dirLight;

struct PointLight {
    vec3 position;
    
    float Kc;
    float Kl;
    float Kq;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];

struct SpotLight{
    vec3 position;
    vec3 direction;

    float cutOffStart;
    float cutOffEnd;
  
    float Kc;
    float Kl;
    float Kq;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform SpotLight spotLight;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

void main()
{
    // properties
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    // phase 1: Directional lighting
    vec3 result = CalcDirLight(dirLight, norm, viewDir);
    // phase 2: Point lights
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);    
    // phase 3: Spot light
    result += CalcSpotLight(spotLight, norm, FragPos, viewDir);    
    
    FragColor = vec4(result, 1.0);
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    // diffuse shading
    float diff = max(dot(normal, -light.direction), 0.0);
    // specular shading
    vec3 reflectDir = reflect(light.direction, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // combine results
    vec3 ambient  = light.ambient  * vec3(texture(material.diffuse, TexCoord));
    vec3 diffuse  = light.diffuse  * diff * vec3(texture(material.diffuse, TexCoord));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoord));
    return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // attenuation
    float distance    = length(light.position - fragPos);
    float attenuation = 1.0 / (light.Kc + light.Kl * distance + light.Kq * (distance * distance));
    // combine results
    vec3 ambient  = light.ambient  * vec3(texture(material.diffuse, TexCoord));
    vec3 diffuse  = light.diffuse  * diff * vec3(texture(material.diffuse, TexCoord));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoord));
    return (ambient + diffuse + specular) * attenuation;
}

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    // diffuse shading
    float diff = max(dot(Normal, lightDir), 0.0);
    // specular
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // attenuation
    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.Kc + light.Kl * distance + light.Kq * (distance * distance));
    // spotlight cutoff
    float theta = dot(lightDir, normalize(-light.direction));
    float epsilon = light.cutOffStart - light.cutOffEnd;
    float intensity = clamp((theta - light.cutOffEnd) / epsilon, 0.0, 1.0);
    // combine results
    vec3 ambient  = light.ambient  * vec3(texture(material.diffuse, TexCoord));
    vec3 diffuse  = light.diffuse  * diff * intensity * vec3(texture(material.diffuse, TexCoord));
    vec3 specular = light.specular * spec * intensity * vec3(texture(material.specular, TexCoord));
    return (ambient + diffuse + specular) * attenuation;
}
