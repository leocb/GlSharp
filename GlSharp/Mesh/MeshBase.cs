using System.Runtime.InteropServices;

using GlSharp.Shaders;
using GlSharp.Tools;
using GlSharp.Types;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Mesh;
public class MeshBase : IDisposable
{
    //  render data
    private readonly int vao;
    private readonly int ebo;
    private readonly int vbo;

    public List<Vertex.Data> Vertices { get; set; }
    public List<int> Indices { get; set; }
    public List<Texture.Data> Textures { get; set; }
    public IProgram Program { get; set; }

    public MeshBase(List<Vertex.Data> vertices, List<int> indices, List<Texture.Data> textures, IProgram program)
    {
        Vertices = vertices;
        Indices = indices;
        Textures = textures;
        Program = program;

        vao = GL.GenVertexArray();
        ebo = GL.GenBuffer();
        vbo = GL.GenBuffer();

        GlTools.TsGlCall(SetupMesh);
        GlTools.TsGlCall(SetupTextures);
    }

    public void Draw()
    {
        GlTools.TsGlCall(InternalDraw);
    }

    private void InternalDraw()
    {
        for (int i = 0; i < Textures.Count; i++)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + i);
            GL.BindTexture(TextureTarget.Texture2D, Textures[i].Id);
        }

        // draw mesh
        GL.BindVertexArray(vao);
        GL.DrawElements(PrimitiveType.Triangles, Indices.Count, DrawElementsType.UnsignedInt, 0);
    }

    private void SetupMesh()
    {
        // Vertex Array Object
        GL.BindVertexArray(vao);

        // Vertices
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        int size = Marshal.SizeOf<Vertex.Data>();
        GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * size, Vertices.ToArray(), BufferUsageHint.StaticDraw);

        // Element Buffer
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Count * sizeof(uint), Indices.ToArray(), BufferUsageHint.StaticDraw);

        // Vertices attributes (data layout)
        // position
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex.Data>(), 0 * sizeof(float));

        // normal
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex.Data>(), 3 * sizeof(float));

        // UV
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<Vertex.Data>(), (3 + 3) * sizeof(float));
    }

    private void SetupTextures()
    {
        for (int i = 0; i < Textures.Count; i++)
        {
            Program.SetInt($"material.{Textures[i].Type.ToName()}", i);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            GL.DeleteVertexArray(vao);
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
