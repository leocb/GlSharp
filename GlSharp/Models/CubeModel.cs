using GlSharp.Behavior;
using GlSharp.Materials;

using OpenTK.Mathematics;

namespace GlSharp.Models;
public class CubeModel : ModelBase
{

    public override float[] Vertices => new float[] {
        // Positions          // colors          // UV Map    // Normals
        // Back
         0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f,  1.0f, 0.0f,   0f,  0f, -1f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f, 0.0f,  0.0f, 0.0f,   0f,  0f, -1f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f, 0.0f,  1.0f, 1.0f,   0f,  0f, -1f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f,   0f,  0f, -1f,
        // Front
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,   0f,  0f,  1f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,  1.0f, 0.0f,   0f,  0f,  1f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f, 1.0f,  1.0f, 1.0f,   0f,  0f,  1f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 1.0f,  0.0f, 1.0f,   0f,  0f,  1f,
        // Left
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 1.0f,  1.0f, 0.0f,  -1f,  0f,  0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 0.0f,  1.0f, 1.0f,  -1f,  0f,  0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f, 0.0f,  0.0f, 1.0f,  -1f,  0f,  0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,  -1f,  0f,  0f,
        // Right
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f, 1.0f,  1.0f, 0.0f,   1f,  0f,  0f,
         0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f,  0.0f, 1.0f,   1f,  0f,  0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f, 0.0f,  1.0f, 1.0f,   1f,  0f,  0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,  0.0f, 0.0f,   1f,  0f,  0f,
         // Down
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f, 0.0f,  0.0f, 1.0f,   0f, -1f,  0f,
         0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f,   0f, -1f,  0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 1.0f,  1.0f, 0.0f,   0f, -1f,  0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,   0f, -1f,  0f,
        // Up
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f, 0.0f,  1.0f, 1.0f,   0f,  1f,  0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f,   0f,  1f,  0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f, 1.0f,  1.0f, 0.0f,   0f,  1f,  0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 1.0f,  0.0f, 0.0f,   0f,  1f,  0f,
    };

    public override uint[] Indices => new uint[] {
        0,1,2,3,2,1,
        4,5,6,6,7,4,
        8,9,10,10,11,8,
        12,13,14,15,13,12,
        16,17,18,18,19,16,
        20,21,22,23,22,21,
    };

    public CubeModel(Vector3? position, Quaternion? rotation, Vector3? scale, List<IBehavior>? behaviorList, IMaterial? materialOverride = null) : base(position, rotation, scale, behaviorList)
    {
        // Material
        Material = materialOverride ?? new VertexColorMaterial();
    }
}
