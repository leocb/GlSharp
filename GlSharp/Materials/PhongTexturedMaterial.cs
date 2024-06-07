
using GlSharp.Materials.Textures;
using GlSharp.Objects;
using GlSharp.Tools;

using OpenTK.Mathematics;

namespace GlSharp.Materials;
public class PhongTexturedMaterial : MaterialBase
{
    public PhongTexturedMaterial(SpotLight flashlight, PointLight[] lamps, SunLight sun)
        : base("Basic.vert", "PhongTextured.frag")
    {
        GlTools.TsGlCall(() =>
        {
            Program.Use();

            // Flashlight
            if (flashlight is not null)
            {
                Program.SetVec3("spotLight.position", flashlight.Position);
                Program.SetVec3("spotLight.direction", flashlight.Direction);
                Program.SetFloat("spotLight.cutOffStart", flashlight.CutOffStart);
                Program.SetFloat("spotLight.cutOffEnd", flashlight.CutOffEnd);
                Program.SetVec3("spotLight.ambient", flashlight.AmbientColor);
                Program.SetVec3("spotLight.diffuse", flashlight.DifuseColor);
                Program.SetVec3("spotLight.specular", flashlight.SpecularColor);
                Program.SetFloat("spotLight.Kc", flashlight.KConstant);
                Program.SetFloat("spotLight.Kl", flashlight.KLinear);
                Program.SetFloat("spotLight.Kq", flashlight.KQuadratic);
            }

            // Lamps
            for (int i = 0; i < 4; i++)
            {
                Program.SetVec3($"pointLights[{i}].position", lamps[i].Position);
                Program.SetVec3($"pointLights[{i}].ambient", lamps[i].AmbientColor);
                Program.SetVec3($"pointLights[{i}].diffuse", lamps[i].DifuseColor);
                Program.SetVec3($"pointLights[{i}].specular", lamps[i].SpecularColor);
                Program.SetFloat($"pointLights[{i}].Kc", lamps[i].KConstant);
                Program.SetFloat($"pointLights[{i}].Kl", lamps[i].KLinear);
                Program.SetFloat($"pointLights[{i}].Kq", lamps[i].KQuadratic);
            }

            // Sun
            Program.SetVec3("dirLight.direction", sun.Direction);
            Program.SetVec3("dirLight.ambient", sun.AmbientColor);
            Program.SetVec3("dirLight.diffuse", sun.DifuseColor);
            Program.SetVec3("dirLight.specular", sun.SpecularColor);

            // Textures
            // int value is defined by the order which the texture was added to the textureHandles array above
            Program.SetFloat("material.shininess", 0.9f * 128.0f);
        });
    }

    public void UpdateFlashlight(SpotLight light)
    {
        GlTools.TsGlCall(() =>
        {
            Program.Use();
            Program.SetVec3("spotLight.position", light.Position);
            Program.SetVec3("spotLight.direction", light.Direction);
        });
    }

    public void UpdateCamera(Vector3 cameraPosition)
    {
        GlTools.TsGlCall(() =>
        {
            Program.Use();
            Program.SetVec3("viewPos", cameraPosition);
        });
    }
}