using GlSharp.Materials.Textures;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Materials;

public abstract class MaterialBase : IMaterial {

    public Shaders.Program Program { get; private set; }
    private int[] textureHandles = Array.Empty<int>();

    private int[] TextureHandles {
        get => textureHandles;
        set {
            textureHandles = value;

            if (textureHandles.Length <= 0)
                return;

            Tools.TsGlCall(() => {
                Program.Use();
                for (int i = 0; i < value?.Length; i++) {
                    Program.SetInt($"texture{i}", i);
                }
            });
        }
    }

    protected MaterialBase(int[] textures, string vertexShader, string fragShader) {
        Program = new Shaders.Program(vertexShader, fragShader);
        TextureHandles = textures ?? Array.Empty<int>();
    }

    public virtual void Use() {
        Program.Use();

        for (int i = 0; i < TextureHandles.Length; i++) {
            TextureLoader.Use(TextureHandles[i], TextureUnit.Texture0 + i);
        }
    }
}
