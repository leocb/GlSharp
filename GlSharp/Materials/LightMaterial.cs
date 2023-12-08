
using GlSharp.Tools;

using OpenTK.Mathematics;

namespace GlSharp.Materials;
public class LightMaterial : MaterialBase {
    public LightMaterial(Vector3 lightColor)
        : base("Light.vert", "Light.frag") {

        GlTools.TsGlCall(() => {
            Program.Use();
            Program.SetVec3("lightColor", lightColor);
        });
    }

    public void UpdateLightColor(Vector3? lightColor = null) {

        GlTools.TsGlCall(() => {
            Program.Use();
            if (lightColor is not null)
                Program.SetVec3("lightColor", (Vector3)lightColor);
        });
    }
}