using GlSharp.Materials.Textures;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Materials;

public abstract class MaterialBase : IMaterial
{

    public Shaders.Program Program { get; private set; }

    protected int[] textureHandles;

    protected MaterialBase(string vertexShader, string fragShader, int[]? textures = null)
    {
        Program = new Shaders.Program(vertexShader, fragShader);
        textureHandles = textures ?? Array.Empty<int>();
    }

    public virtual void Use()
    {
        Program.Use();

        for (int i = 0; i < textureHandles.Length; i++)
        {
            TextureLoader.Use(textureHandles[i], TextureUnit.Texture0 + i);
        }
    }
}
