using Assimp;

using GlSharp.Behavior;
using GlSharp.Materials;
using GlSharp.Models.Simple;

using OpenTK.Mathematics;

namespace GlSharp.Objects;
public class PointLight : SimpleCubeModel
{
    public Vector3 DifuseColor { get; set; }
    public Vector3 AmbientColor { get; set; }
    public Vector3 SpecularColor { get; set; }

    public float KConstant { get; private set; }
    public float KLinear { get; private set; }
    public float KQuadratic { get; private set; }

    public PointLight(Vector3? position, Vector3 difuseColor, Vector3 ambientColor, Vector3 specularColor, float range, float intensity, List<IBehavior>? behaviorList)
        : base(position, null, new(.05f, .05f, .05f), behaviorList, new LightMaterial(difuseColor))
    {
        DifuseColor = difuseColor;
        AmbientColor = ambientColor;
        SpecularColor = specularColor;

        KConstant = 1f / intensity;
        KLinear = 4.6905f * MathF.Pow(range, -1.01f);
        KQuadratic = 82.445f * MathF.Pow(range, -2.019f);
    }
    public override void Draw(float time)
    {
        ((LightMaterial)Material).UpdateLightColor(DifuseColor);
        base.Draw(time);
    }
}
