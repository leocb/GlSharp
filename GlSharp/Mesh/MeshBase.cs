using System.Runtime.InteropServices;

using GlSharp.Shaders;
using GlSharp.Tools;
using GlSharp.Types;

using OpenTK.Graphics.OpenGL;

namespace GlSharp.Mesh;
public class MeshBase
{
    //  render data
    private int vao;

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
        vao = GL.GenVertexArray();
        int ebo = GL.GenBuffer();
        int vbo = GL.GenBuffer();

        // Vertex Array Object
        GL.BindVertexArray(vao);

        // Vertices
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * Marshal.SizeOf<Vertex.Data>(), Vertices.ToArray(), BufferUsageHint.StaticDraw);

        // Element Buffer
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Count * sizeof(uint), Indices.ToArray(), BufferUsageHint.StaticDraw);

        // Vertices attributes (data layout)
        // position
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false,
            Marshal.SizeOf<Vertex.Data>(),
            Marshal.OffsetOf<Vertex.Data>(nameof(Vertex.Data.Position)));

        // normal
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false,
            Marshal.SizeOf<Vertex.Data>(),
            Marshal.OffsetOf<Vertex.Data>(nameof(Vertex.Data.Normal)));

        // UV
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false,
            Marshal.SizeOf<Vertex.Data>(),
            Marshal.OffsetOf<Vertex.Data>(nameof(Vertex.Data.TexCoords)));
    }

    private void SetupTextures()
    {
        int diffuseNr = 1;
        int specularNr = 1;

        for (int i = 0; i < Textures.Count; i++)
        {
            int number = Textures[i].Type switch
            {
                Texture.Type.Diffuse => diffuseNr++,
                Texture.Type.Specular => specularNr++,
                _ => 0
            };
            Program.SetInt($"material.{Textures[i].Type.ToName()}{number}", i);
        }
    }
}
