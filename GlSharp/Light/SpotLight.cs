using GlSharp.Behavior;
using GlSharp.Materials;
using GlSharp.ModelsSimple;

using OpenTK.Mathematics;

namespace GlSharp.Objects;
public class SpotLight : SimpleModelBase
{

    public override float[] Vertices => Array.Empty<float>();
    public override uint[] Indices => Array.Empty<uint>();

    public Vector3 DifuseColor { get; set; }
    public Vector3 AmbientColor { get; set; }
    public Vector3 SpecularColor { get; set; }
    public Vector3 Direction { get; set; }
    public float CutOffStart { get; set; }
    public float CutOffEnd { get; set; }

    public float KConstant { get; private set; }
    public float KLinear { get; private set; }
    public float KQuadratic { get; private set; }

    public SpotLight(Vector3? position, Vector3 direction, float cutOffStart, float cutOffEnd, Vector3 difuseColor, Vector3 ambientColor, Vector3 specularColor, float range, float intensity, List<IBehavior>? behaviorList)
        : base(position, null, null, behaviorList)
    {

        DifuseColor = difuseColor;
        AmbientColor = ambientColor;
        SpecularColor = specularColor;

        Direction = direction.Normalized();
        CutOffStart = (float)Math.Cos(MathHelper.DegreesToRadians(cutOffStart));
        CutOffEnd = (float)Math.Cos(MathHelper.DegreesToRadians(cutOffEnd));

        KConstant = 1f / intensity;
        KLinear = 4.6905f * MathF.Pow(range, -1.01f);
        KQuadratic = 82.445f * MathF.Pow(range, -2.019f);

        Material = new LightMaterial(DifuseColor);
    }
}
