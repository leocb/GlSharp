
namespace GlSharp.Materials;

public interface IMaterial : IDisposable {
    Shaders.Program Program { get; }
    int[] TextureHandles { get; }
    void Use();
}