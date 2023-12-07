﻿
using OpenTK.Mathematics;

namespace GlSharp.Materials;
public class PhongMaterial : MaterialBase {
    public PhongMaterial(Vector3 objectColor, Vector3 lightColor, Vector3 lightPos)
        : base(Array.Empty<int>(), "Basic.vert", "Phong.frag") {

        Tools.TsGlCall(() => {
            Program.Use();
            Program.SetVec3("objectColor", objectColor);
            Program.SetVec3("lightColor", lightColor);
            Program.SetVec3("lightPos", lightPos);
        });
    }

    public void UpdateLight(Vector3? lightPos = null, Vector3? lightColor = null) {

        Tools.TsGlCall(() => {
            Program.Use();
            if (lightPos is not null)
                Program.SetVec3("lightPos", (Vector3)lightPos);
            if (lightColor is not null)
                Program.SetVec3("lightColor", (Vector3)lightColor);
        });
    }

    public void UpdateCamera(Vector3 cameraPosition) {
        Tools.TsGlCall(() => {
            Program.Use();
            Program.SetVec3("viewPos", cameraPosition);
        });
    }
}