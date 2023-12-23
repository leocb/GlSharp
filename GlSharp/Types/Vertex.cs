using System.Runtime.InteropServices;

using OpenTK.Mathematics;

namespace GlSharp.Types;

public static class Vertex
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Data
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 TexCoords { get; set; }
    }
}
