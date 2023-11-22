using System.Diagnostics;

using GlSharp.Materials;
using GlSharp.Materials.Textures;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GlSharp.Models;

internal class Basic : IDisposable {
    private readonly Stopwatch sw;
    private readonly MaterialBase material;
    private readonly int vertexHandle;
    private readonly int vao; // Vertex Array Object
    private readonly int ebo; // Element Buffer Object

    private Matrix4 modelMatrix = Matrix4.CreateScale(1f);

    private readonly float[] vertices = {
      // positions        // colors          // Texture coordinates
      0.5f,  0.5f, 0.0f,  1.0f, 1.0f, 1.0f,  1.0f, 1.0f,  // top right
      0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 1.0f,  1.0f, 0.0f,  // bottom right
     -0.5f, -0.5f, 0.5f,  0.0f, 0.0f, 1.0f,  0.0f, 0.0f,  // bottom left
     -0.5f,  0.5f, 0.5f,  0.0f, 1.0f, 1.0f,  0.0f, 1.0f   // top left
    };

    private readonly uint[] indices = {
        0, 1, 3,   // first triangle
        2, 3, 1,   // second triangle
    };

    public Basic() {
        sw = Stopwatch.StartNew();

        vao = GL.GenVertexArray();
        vertexHandle = GL.GenBuffer();
        ebo = GL.GenBuffer();

        Tools.TsGlCall(() => {
            // Vertex Array Object - Bundles the data into a single buffer
            GL.BindVertexArray(vao);

            // The vertices data
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Vertices attributes (data layout)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), 0 * sizeof(float));
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), (3 + 3) * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            // Element Buffer Object - How to draw the vertices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        });

        // Material
        material = new(
            new int[] {
                TextureLoader.Load("container.jpg"),
                TextureLoader.Load("awesomeface.png")
            },"basic.vert", "basic.frag");
    }

    public void Update(float deltaTime) {
        modelMatrix = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(sw.Elapsed.TotalSeconds * 10));
    }

    public void Draw() {
        Tools.TsGlCall(() => {
            GL.BindVertexArray(vao);
            material.Use();
            material.Program.SetMat4("model", modelMatrix);
#warning TODO: These 2 should be inside a shared uniform
            material.Program.SetMat4("view", FreeCamera.ViewMatrix);
            material.Program.SetMat4("projection", FreeCamera.ProjectionMatrix);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        });
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            material.Dispose();
            GL.DeleteBuffer(vertexHandle);
            GL.DeleteBuffer(ebo);
            GL.DeleteVertexArray(vao);
        }
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
