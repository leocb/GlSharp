using OpenTK.Graphics.OpenGL;

using StbImageSharp;

namespace GlSharp.Materials.Textures;

internal static class TextureLoader {
    public static int Load(string fileName,
                           bool generateMipmap = true,
                           TextureWrapMode wrapMode = TextureWrapMode.Repeat,
                           TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear,
                           TextureMagFilter magFilter = TextureMagFilter.Linear) {

        // Load the image
        StbImage.stbi_set_flip_vertically_on_load(1);
        using FileStream fs = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "Assets", "Textures", fileName));
        ImageResult image = ImageResult.FromStream(fs, ColorComponents.RedGreenBlueAlpha);

        // Upload texture to GPU
        int handle = GL.GenTexture();
        Tools.TsGlCall(() => {
            GL.BindTexture(TextureTarget.Texture2D, handle);
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba, // the format stored in the gpu
                image.Width,
                image.Height,
                0,
                PixelFormat.Rgba, // the format from stb lib (always RGBA)
                PixelType.UnsignedByte,
                image.Data);

            // Mipmaps
            if (generateMipmap)
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // Wrap mode
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);

            // Filter mode
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

        });

        return handle;
    }

    public static void Use(int handle, TextureUnit unit = TextureUnit.Texture0) {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, handle);
    }
}
