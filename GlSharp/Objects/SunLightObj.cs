using System.Reflection.Metadata.Ecma335;

using GlSharp.Behavior;
using GlSharp.Materials;
using GlSharp.Models;

using OpenTK.Mathematics;

namespace GlSharp.Objects;
public class SunLightObj : ModelBase {
    private Vector3 difuseColor;

    public override float[] Vertices => Array.Empty<float>();
    public override uint[] Indices => Array.Empty<uint>();

    public Vector3 DifuseColor { get => difuseColor; set {
            difuseColor = value;
            if (Material is LightMaterial lm) {
                lm.UpdateLightColor(value);
            }
        } }
    public Vector3 AmbientColor { get; set; }
    public Vector3 Direction { get; set; }
    public Vector3 SpecularColor { get; set; }

    public SunLightObj(Vector3 direction, Vector3 difuseColor, Vector3 ambientColor, Vector3 specularColor, List<IBehavior>? behaviorList)
        : base(null, null, new(1f, 1f, 1f), behaviorList) {

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
