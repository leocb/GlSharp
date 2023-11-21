using System.Diagnostics;

using GlSharp.Shaders;
using GlSharp.Textures;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Models;

internal class Basic : IDisposable
{
    private readonly Stopwatch sw;
    private readonly Shader shader;
    private readonly Texture texture;
    private readonly int vertexHandle;
    private readonly int vao; // Vertex Array Object
    private readonly int ebo; // Element Buffer Object
    private readonly float[] vertices = {
      // positions        // colors          // Texture coordinates
      0.5f,  0.5f, 0.0f,  1.0f, 1.0f, 1.0f,  1.0f, 1.0f,  // top right
      0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 1.0f,  1.0f, 0.0f,  // bottom right
     -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,  // bottom left
     -0.5f,  0.5f, 0.0f,  0.0f, 1.0f, 1.0f,  0.0f, 1.0f   // top left
    };
    private readonly uint[] indices = {
        0, 1, 3,   // first triangle
        2, 3, 1,   // second triangle
    };

    public Basic()
    {
        this.sw = Stopwatch.StartNew();

        // Textures
        this.texture = new("container.jpg");

        // Shaders
        this.shader = new("Basic.vert", "Basic.frag");

        // Vertex Array Object - Tells the GPU how to handle the data we send
        this.vao = GL.GenVertexArray();
        GL.BindVertexArray(this.vao);

        // The Geometry data
        this.vertexHandle = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), 0 * sizeof(float));
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), 3 * sizeof(float));
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), (3 + 3) * sizeof(float));
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        // Element Buffer Object - How to draw the vertices
        this.ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);
    }

    private void SetTextureParameters()
    {
        // wrapping mode
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        // without mipmap
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        // with mipmap
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
    }

    public void Draw()
    {
        GL.BindVertexArray(this.vao);
        this.texture.Use();
        this.shader.Use();
        SetTextureParameters();

        double timeValue = this.sw.Elapsed.TotalSeconds;
        float greenValue = ((float)Math.Sin(timeValue) / 2.0f) + 0.5f;
        GL.Uniform4(this.shader.GetUniformLocation("ourColor"), 0.0f, greenValue, 0.0f, 1.0f);

        GL.DrawElements(PrimitiveType.Triangles, this.indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.shader.Dispose();
            GL.DeleteBuffer(this.vertexHandle);
            GL.DeleteBuffer(this.ebo);
            GL.DeleteVertexArray(this.vao);
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
