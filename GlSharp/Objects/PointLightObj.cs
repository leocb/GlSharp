using GlSharp.Behavior;
using GlSharp.Materials;
using GlSharp.Models;

using OpenTK.Mathematics;

namespace GlSharp.Objects;
public class PointLightObj : CubeModel {
    private Vector3 difuseColor;

    public Vector3 DifuseColor {
        get => difuseColor; set {
            difuseColor = value;
            if (Material is LightMaterial lm) {
                lm.UpdateLightColor(value);
            }
        }
    }
    public Vector3 AmbientColor { get; set; }
    public Vector3 SpecularColor { get; set; }
    public float KConstant { get; private set; }
    public float KLinear { get; private set; }
    public float KQuadratic { get; private set; }

    public PointLightObj(Vector3? position, Vector3 difuseColor, Vector3 ambientColor, Vector3 specularColor, float range, float intensity, List<IBehavior>? behaviorList)
        : base(position, null, new(0.1f, 0.1f, 0.1f), behaviorList) {

        DifuseColor = difuseColor;
        AmbientColor = ambientColor;
        SpecularColor = specularColor;

        KConstant = 1f / intensity;
        KLinear = 4.6905f * MathF.Pow(range, -1.01f);
        KQuadratic = 82.445f * MathF.Pow(range, -2.019f);

        Material = new LightMaterial(DifuseColor);
    }
}
