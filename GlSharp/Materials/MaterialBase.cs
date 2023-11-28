using GlSharp.Materials.Textures;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Materials;

public class MaterialBase : IMaterial {

    public Shaders.Program Program { get; private set; }
    private int[] textureHandles = Array.Empty<int>();

    public int[] TextureHandles {
        get => textureHandles;
        private set {
            textureHandles = value;
            Tools.TsGlCall(() => {
                Program.Use();
                for (int i = 0; i < value?.Length; i++) {
                    Program.SetInt($"texture{i}", i);
                }
            });
        }
    }

    public MaterialBase(int[] textures, string vertexShader, string fragShader) {
        Program = new Shaders.Program(vertexShader, fragShader);
        TextureHandles = textures ?? Array.Empty<int>();
    }

    public void Use() {
        Program.Use();

        for (int i = 0; i < TextureHandles.Length; i++) {
            TextureLoader.Use(TextureHandles[i], TextureUnit.Texture0 + i);
        }
    }

    protected virtual void Dispose(bool disposing) {
        if (!disposing)
            return;

        Program.Dispose();
        GL.DeleteTextures(textureHandles.Length, textureHandles);
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
