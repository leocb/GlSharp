
using GlSharp.Tools;

using OpenTK.Mathematics;

namespace GlSharp.Materials;
public class LightMaterial : MaterialBase
{
    public LightMaterial(Vector3 lightColor)
        : base("Light.vert", "Light.frag")
    {
        UpdateLightColor(lightColor);
    }

    public void UpdateLightColor(Vector3 lightColor)
    {
        GlTools.TsGlCall(() =>
        {
            Program.Use();
            Program.SetVec3("lightColor", lightColor);
        });
    }
}