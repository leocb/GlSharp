using GlSharp.Behavior;
using GlSharp.Materials;
using GlSharp.Models;

using OpenTK.Mathematics;

namespace GlSharp.Objects;
public class SunLight : ModelBase {

    public override float[] Vertices => Array.Empty<float>();
    public override uint[] Indices => Array.Empty<uint>();

    public Vector3 DifuseColor { get; set; }
    public Vector3 AmbientColor { get; set; }
    public Vector3 SpecularColor { get; set; }
    public Vector3 Direction { get; set; }

    public SunLight(Vector3 direction, Vector3 difuseColor, Vector3 ambientColor, Vector3 specularColor, List<IBehavior>? behaviorList)
        : base(null, null, null, behaviorList) {

        Direction = direction.Normalized();
        DifuseColor = difuseColor;
        AmbientColor = ambientColor;
        SpecularColor = specularColor;

        Material = new LightMaterial(DifuseColor);
    }

    public override void Draw(float time) {
        //sun has no geometry
    }
}
