using OpenTK.Graphics.OpenGL;

using StbImageSharp;

namespace GlSharp.Textures;

internal class Texture
{
    private readonly int handle;

    public Texture(string fileName)
    {
        // stb_image loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
        // This will correct that, making the texture display properly.
        StbImage.stbi_set_flip_vertically_on_load(1);

        // Load the image
        ImageResult image = ImageResult.FromStream(
            File.OpenRead(Path.Combine(Environment.CurrentDirectory, "Assets", "Textures", fileName)),
            ColorComponents.RedGreenBlueAlpha);

        // Upload texture to GPU
        this.handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, this.handle);
        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba, // the format stored in the gpu
            image.Width,
            image.Height,
            0,
            PixelFormat.Rgba, // the format from stb lib (fixed)
            PixelType.UnsignedByte,
            image.Data);

        // might not be necessary if the texture is never going to be smaller on screen
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }

    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, this.handle);
    }
}
