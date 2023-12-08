
using GlSharp.Materials.Textures;
using GlSharp.Objects;
using GlSharp.Tools;

using OpenTK.Mathematics;

namespace GlSharp.Materials;
public class PhongTexturedMaterial : MaterialBase {
    public PhongTexturedMaterial(SunLightObj light, string DiffuseTexName)
        : base("Basic.vert", "PhongTextured.frag") {

        string[] texNames = DiffuseTexName.Split('.');

        int diffuseTextHandle = TextureLoader.Load($"{texNames[0]}.{texNames[1]}");
        int SpecularTextHandle = TextureLoader.Load($"{texNames[0]}_specular.{texNames[1]}");

        textureHandles = new int[] {
            diffuseTextHandle,
            SpecularTextHandle
        };

        GlTools.TsGlCall(() => {
            Program.Use();

            Program.SetVec3("light.direction", light.Direction);
            Program.SetVec3("light.position", light.Position);
            Program.SetVec3("light.ambient", light.AmbientColor);
            Program.SetVec3("light.diffuse", light.DifuseColor);
            Program.SetVec3("light.specular", light.SpecularColor);

            // Textures
            // int value is defined by the order which the texture was added to the textureHandles array above
            Program.SetInt("material.diffuse", 0);
            Program.SetInt("material.specular", 1);
            Program.SetFloat("material.shininess", 0.8f * 128.0f);
        });
    }

    public void UpdateLight(PointLightObj light) {

        GlTools.TsGlCall(() => {
            Program.Use();
            Program.SetVec3("light.position", light.Position);
            Program.SetVec3("light.ambient", light.AmbientColor);
            Program.SetVec3("light.diffuse", light.DifuseColor);
            Program.SetVec3("light.specular", light.SpecularColor);
        });
    }

    public void UpdateCamera(Vector3 cameraPosition) {
        GlTools.TsGlCall(() => {
            Program.Use();
            Program.SetVec3("viewPos", cameraPosition);
        });
    }
}