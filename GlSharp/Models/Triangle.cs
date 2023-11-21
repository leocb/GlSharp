using System.Diagnostics;

using GlSharp.Shaders;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Models;

internal class Triangle : IDisposable
{
    private readonly Stopwatch sw;
    private readonly Shader shader;
    private readonly int vertexHandle;
    private readonly int vao; // Vertex Array Object
    private readonly int ebo; // Element Buffer Object
    private readonly float[] vertices = {
         0.5f,  0.5f, 0.0f,  // top right
         0.5f, -0.5f, 0.0f,  // bottom right
        -0.5f, -0.5f, 0.0f,  // bottom left
        -0.5f,  0.5f, 0.0f   // top left
    };
    private readonly uint[] indices = {
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    };

    public Triangle()
    {
        this.sw = Stopwatch.StartNew();

        // Shaders
        this.shader = new("Basic.vert", "Basic.frag");

        // Vertex Array Object - Tells the GPU how to handle the data we send
        this.vao = GL.GenVertexArray();
        GL.BindVertexArray(this.vao);

        // The Geometry data
        this.vertexHandle = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Length * sizeof(float), this.vertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(this.shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        // Element Buffer Object - How to draw the vertices
        this.ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Length * sizeof(uint), this.indices, BufferUsageHint.StaticDraw);

        // VAO - continued
        GL.EnableVertexAttribArray(0);
    }

    public void Draw()
    {
        GL.BindVertexArray(this.vao);
        this.shader.Use();

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
