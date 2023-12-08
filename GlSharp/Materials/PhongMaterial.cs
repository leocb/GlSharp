
using GlSharp.Tools;

using OpenTK.Mathematics;

namespace GlSharp.Materials;
public class PhongMaterial : MaterialBase {
    public PhongMaterial(Vector3 objectColor, Vector3 lightColor, Vector3 lightPos)
        : base("Basic.vert", "Phong.frag") {

        GlTools.TsGlCall(() => {
            Program.Use();
            Program.SetVec3("objectColor", objectColor);
            Program.SetVec3("lightColor", lightColor);
            Program.SetVec3("lightPos", lightPos);
        });
    }

    public void UpdateLight(Vector3? lightPos = null, Vector3? lightColor = null) {

        GlTools.TsGlCall(() => {
            Program.Use();
            if (lightPos is not null)
                Program.SetVec3("lightPos", (Vector3)lightPos);
            if (lightColor is not null)
                Program.SetVec3("lightColor", (Vector3)lightColor);
        });
    }

    public void UpdateCamera(Vector3 cameraPosition) {
        GlTools.TsGlCall(() => {
            Program.Use();
            Program.SetVec3("viewPos", cameraPosition);
        });
    }
}