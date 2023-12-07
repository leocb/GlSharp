using GlSharp.Behavior;
using GlSharp.Materials;
using GlSharp.Models;

using OpenTK.Mathematics;

namespace GlSharp.Objects;
public class PointLightObj : CubeModel {
    private Vector3 difuseColor;

    public Vector3 DifuseColor { get => difuseColor; set {
            difuseColor = value;
            if (Material is LightMaterial lm) {
                lm.UpdateLightColor(value);
            }
        } }
    public Vector3 AmbientColor { get; set; }
    public Vector3 SpecularColor { get; set; }

    public PointLightObj(Vector3? position, Vector3 difuseColor, Vector3 ambientColor, Vector3 specularColor, List<IBehavior>? behaviorList)
        : base(position, null, new(0.1f, 0.1f, 0.1f), behaviorList) {

        DifuseColor = difuseColor;
        AmbientColor = ambientColor;
        SpecularColor = specularColor;

        Material = new LightMaterial(DifuseColor);
    }

}
