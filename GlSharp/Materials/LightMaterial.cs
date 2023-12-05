
using OpenTK.Mathematics;

namespace GlSharp.Materials;
public class LightMaterial : MaterialBase {
    public LightMaterial(Vector3 objectColor, Vector3 lightColor)
        : base(Array.Empty<int>(), "Light.vert", "Light.frag") {

        Tools.TsGlCall(() => {
            Program.Use();
            Program.SetVec3("objectColor", objectColor);
            Program.SetVec3("lightColor", lightColor);
        });
    }

    public void UpdateLightColor(Vector3? objectColor = null, Vector3? lightColor = null) {

        Tools.TsGlCall(() => {
            Program.Use();
            if (objectColor is not null)
                Program.SetVec3("objectColor", (Vector3)objectColor);
            if (lightColor is not null)
                Program.SetVec3("lightColor", (Vector3)lightColor);
        });
    }
}