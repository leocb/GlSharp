using GlSharp.Tools;

using OpenTK.Graphics.OpenGL;

using StbImageSharp;

namespace GlSharp.Materials.Textures;

internal static class TextureLoader
{

    private static readonly Dictionary<int, int> textureList = new();

    private static readonly int missingHandle = Load("missing.png", false, minFilter: TextureMinFilter.Nearest, magFilter: TextureMagFilter.Nearest, isManaged: false);

    public static int Load(string fileName,
                           bool generateMipmap = true,
                           TextureWrapMode wrapMode = TextureWrapMode.Repeat,
                           TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear,
                           TextureMagFilter magFilter = TextureMagFilter.Linear,
                           bool isManaged = true)
    {

        // First, check if a texture with all the parameters was already created
        int hash = fileName.GetHashCode() + generateMipmap.GetHashCode() + wrapMode.GetHashCode() + minFilter.GetHashCode() + magFilter.GetHashCode();

        if (textureList.TryGetValue(hash, out int handle))
            return handle;

        // Load the image
        string fullFilePath = Path.Combine(Environment.CurrentDirectory, "Assets", "Textures", fileName);

        if (!File.Exists(fullFilePath))
        {
            return missingHandle;
        }

        StbImage.stbi_set_flip_vertically_on_load(1);
        using FileStream fs = File.OpenRead(fullFilePath);
        ImageResult image = ImageResult.FromStream(fs, ColorComponents.RedGreenBlueAlpha);

        // Upload texture to GPU
        handle = GL.GenTexture();
        GlTools.TsGlCall(() =>
        {
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

        if (isManaged)
            textureList.Add(hash, handle);

        return handle;
    }

    public static void Use(int handle, TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, handle);
    }

    public static void UnloadAllTextures()
    {
        int[] texHandles = textureList.Values.ToArray();
        GL.DeleteTextures(texHandles.Length, texHandles);
        textureList.Clear();
    }
}
