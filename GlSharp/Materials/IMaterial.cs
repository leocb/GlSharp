
namespace GlSharp.Materials;

public interface IMaterial : IDisposable {
    Shaders.Program Program { get; }
    void Use();
}