using System.Runtime.InteropServices;

using OpenTK.Mathematics;

namespace GlSharp.Types;

public static class Vertex
{
    [StructLayout(LayoutKind.Sequential)]
    public record struct Data(
        Vector3 Position,
        Vector3 Normal,
        Vector2 TexCoords
    );
}
