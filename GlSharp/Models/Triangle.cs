using GlSharp.Shaders;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Models;

internal class Triangle : IDisposable
{
    private readonly Shader shader;
    private readonly int vertexHandle;
    private readonly int vao;
    private readonly float[] vertices ={
    -0.5f, -0.5f, 0.0f, //Bottom-left vertex
     0.5f, -0.5f, 0.0f, //Bottom-right vertex
     0.0f,  0.5f, 0.0f, //Top vertex
};

    public Triangle()
    {
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

        // VAO - continued
        GL.EnableVertexAttribArray(0);
    }

    public void Draw()
    {
        GL.BindVertexArray(this.vao);
        this.shader.Use();
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.shader.Dispose();
            GL.DeleteBuffer(this.vertexHandle);
            GL.DeleteVertexArray(this.vao);
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
