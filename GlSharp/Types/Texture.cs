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
        public int Id { get; set; }
        public Type Type { get; set; }
    }

    public static string ToName(this Type textureType)
    {
        return textureType switch
        {
            Type.Diffuse => "diffuse",
            Type.Specular => "specular",
            _ => "unknown",
        };
    }
    public static Type FromName(string typeName)
    {
        return typeName switch
        {
            "diffuse" => Type.Diffuse,
            "specular" => Type.Specular,
            _ => Type.Diffuse,
        };
    }
}