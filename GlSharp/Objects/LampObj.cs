using GlSharp.Behavior;
using GlSharp.Materials;
using GlSharp.Models;

using OpenTK.Mathematics;

namespace GlSharp.Objects;
public class LampObj : CubeModel {
    public Vector3 LightColor { get; private set; }

    public LampObj(Vector3? position, Vector3 lightColor, List<IBehavior>? behaviorList)
        : base(position, null, new(0.1f, 0.1f, 0.1f), behaviorList) {
        LightColor = lightColor;

        Material = new LightMaterial(LightColor);
    }

    public void UpdateLightColor(Vector3? lightColor = null) {
        if (lightColor is not null)
            LightColor = lightColor.Value;

        if (Material is LightMaterial light) {
            light.UpdateLightColor(LightColor);
        }
    }
}
