
using GlSharp.Objects;
using GlSharp.Tools;

using OpenTK.Mathematics;

namespace GlSharp.Materials;
public class PhongGoldMaterial : MaterialBase
{
    public PhongGoldMaterial(PointLight light)
        : base("Basic.vert", "PhongParameterized.frag")
    {

        GlTools.TsGlCall(() =>
        {
            Program.Use();

            Program.SetVec3("light.position", light.Position);
            Program.SetVec3("light.ambient", light.AmbientColor);
            Program.SetVec3("light.diffuse", light.DifuseColor);
            Program.SetVec3("light.specular", light.SpecularColor);

            Program.SetVec3("material.ambient", new Vector3(0.24725f, 0.1995f, 0.0745f));
            Program.SetVec3("material.diffuse", new Vector3(0.75164f, 0.60648f, 0.22648f));
            Program.SetVec3("material.specular", new Vector3(0.628281f, 0.555802f, 0.366065f));
            Program.SetFloat("material.shininess", 0.4f * 128.0f);
        });
    }

    public void UpdateLight(PointLight light)
    {

        GlTools.TsGlCall(() =>
        {
            Program.Use();
            Program.SetVec3("light.position", light.Position);
            Program.SetVec3("light.ambient", light.AmbientColor);
            Program.SetVec3("light.diffuse", light.DifuseColor);
            Program.SetVec3("light.specular", light.SpecularColor);
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