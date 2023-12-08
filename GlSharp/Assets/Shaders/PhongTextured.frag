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
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};
  
uniform Material material;

void main()
{
    // ambient
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoord));

    // diffuse 
    vec3 norm = normalize(Normal);
    //vec3 lightDir = normalize(light.position - FragPos);
    float diffAmmount = max(dot(Normal, -light.direction), 0.0);
    vec3 diffuse = light.diffuse * (diffAmmount * vec3(texture(material.diffuse, TexCoord)));

    // specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(light.direction, Normal);  
    float specAmmount = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (specAmmount * vec3(texture(material.specular, TexCoord)));  

    // final
    vec3 result = (ambient + diffuse + specular);// * VertColor.rgb;
    FragColor = vec4(result, 1.0);
}