using GlSharp.Materials;

using OpenTK.Mathematics;

namespace GlSharp.Entities;

public interface IEntity : IDisposable {
    Vector3 Position { get; set; }
    Vector3 Scale { get; set; }
    Quaternion Rotation { get; set; }
    IMaterial Material { get; }

    void Draw(float time);
    void Update(float time);
}
