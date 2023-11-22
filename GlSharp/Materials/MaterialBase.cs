using GlSharp.Materials.Textures;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Materials;

internal class MaterialBase : IDisposable {

    internal Shaders.Program Program { get; private set; }
    private int[] textureHandles;

    internal int[] TextureHandles {
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

    internal MaterialBase(int[] textures, string vertexShader, string fragShader) {
        Program = new Shaders.Program(vertexShader, fragShader);
        TextureHandles = textures ?? Array.Empty<int>();
    }

    internal void Use() {
        SetTextureParameters();
        Program.Use();

        for (int i = 0; i < TextureHandles.Length; i++) {
            TextureLoader.Use(TextureHandles[i], TextureUnit.Texture0 + i);
        }
    }

    private void SetTextureParameters() {
        // wrapping mode
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        // Filter mode
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
    }

    protected virtual void Dispose(bool disposing) {
        if (!disposing)
            return;

        Program.Dispose();
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
