using GlSharp.Behavior;
using GlSharp.Materials;
using GlSharp.ModelsSimple;

using OpenTK.Mathematics;

namespace GlSharp.Objects;
public class PointLight : SimpleModelBase
{

    public override float[] Vertices => Array.Empty<float>();
    public override uint[] Indices => Array.Empty<uint>();

    public Vector3 DifuseColor { get; set; }
    public Vector3 AmbientColor { get; set; }
    public Vector3 SpecularColor { get; set; }

    public float KConstant { get; private set; }
    public float KLinear { get; private set; }
    public float KQuadratic { get; private set; }

    public PointLight(Vector3? position, Vector3 difuseColor, Vector3 ambientColor, Vector3 specularColor, float range, float intensity, List<IBehavior>? behaviorList)
        : base(position, null, null, behaviorList)
    {

        DifuseColor = difuseColor;
        AmbientColor = ambientColor;
        SpecularColor = specularColor;

        KConstant = 1f / intensity;
        KLinear = 4.6905f * MathF.Pow(range, -1.01f);
        KQuadratic = 82.445f * MathF.Pow(range, -2.019f);

        Material = new LightMaterial(DifuseColor);
    }
}
