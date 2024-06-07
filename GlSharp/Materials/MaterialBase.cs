using GlSharp.Materials.Textures;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Materials;

public abstract class MaterialBase : IMaterial
{

    public Shaders.Program Program { get; private set; }

    protected int[] textureHandles;

    protected MaterialBase(string vertexShader, string fragShader)
    {
        Program = new Shaders.Program(vertexShader, fragShader);
    }

    public virtual void Use()
    {
        Program.Use();
    }
}
