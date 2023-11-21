using GlSharp.Shaders;
using GlSharp.Textures;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GlSharp.Models;

internal class Basic : IDisposable
{
    private readonly Shader shader;
    private readonly Texture texture1;
    private readonly Texture texture2;
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
        // Vertex Array Object - Bundles the data into a single buffer
        this.vao = GL.GenVertexArray();
        GL.BindVertexArray(this.vao);

        // The vertices data
        this.vertexHandle = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);
        // Vertices attributes (data layout)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), 0 * sizeof(float));
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), 3 * sizeof(float));
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), (3 + 3) * sizeof(float));
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);

        // Element Buffer Object - How to draw the vertices
        this.ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);

        // transformation
        Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90.0f));
        Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
        Matrix4 trans = rotation * scale;

        // Textures
        this.texture1 = new("container.jpg");
        this.texture2 = new("awesomeface.png");

        // Shaders
        this.shader = new("Basic.vert", "Basic.frag");
        this.shader.Use();
        this.shader.SetInt("texture1", 0); // unit 0
        this.shader.SetInt("texture2", 1); // unit 1
        this.shader.SetMat4("transform", trans); // unit 1
    }

    private void SetTextureParameters()
    {
        // wrapping mode
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        // Filter mode
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
    }

    public void Draw()
    {
        GL.BindVertexArray(this.vao);
        this.shader.Use();
        this.texture1.Use(TextureUnit.Texture0);
        this.texture2.Use(TextureUnit.Texture1);
        SetTextureParameters();

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
