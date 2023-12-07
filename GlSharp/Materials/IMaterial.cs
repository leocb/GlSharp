
namespace GlSharp.Materials;

public interface IMaterial {
    Shaders.Program Program { get; }
    void Use();
}