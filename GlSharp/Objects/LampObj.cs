using GlSharp.Behavior;
using GlSharp.Materials;
using GlSharp.Models;

using OpenTK.Mathematics;

namespace GlSharp.Objects;
public class LampObj : CubeModel {

    public LampObj(Vector3 objectColor, Vector3 lightColor, Vector3? position, List<IBehavior>? behaviorList)
        : base(position, null, new(0.1f, 0.1f, 0.1f), behaviorList) {
        Material = new LightMaterial(objectColor, lightColor);
    }

    public void UpdateLightColor(Vector3? objectColor = null, Vector3? lightColor = null) {
        if (Material is LightMaterial light) {
            light.UpdateLightColor(objectColor, lightColor);
        }
    }
}
