using System.Runtime.InteropServices;

namespace GlSharp.Types;


public static class Texture
{
    public enum Type
    {
        Diffuse,
        Specular
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Data
    {
        public uint Id { get; set; }
        public Type Type { get; set; }
    }

    public static string ToName(this Type tt)
    {
        return tt switch
        {
            Type.Diffuse => "diffuse",
            Type.Specular => "specular",
            _ => "unknown", 
        };
    }
}