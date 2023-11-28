using GlSharp.Entities;
using GlSharp.Materials;
using GlSharp.Scene;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GlSharp.Models;
public abstract class BaseModel : IEntity {

    private readonly int vao; // Vertex Array Object
    private readonly int ebo; // Vertex Array Object
    private readonly int vertexHandle; // Vertex Array Object
    private Vector3 position;
    private Vector3 scale;
    private Quaternion rotation;

    public virtual Vector3 Position { get => position; set { position = value; UpdateMatrix(); } }
    public virtual Vector3 Scale { get => scale; set { scale = value; UpdateMatrix(); } }
    public virtual Quaternion Rotation { get => rotation; set { rotation = value; UpdateMatrix(); } }

    public virtual Matrix4 ModelMatrix { get; protected set; }

    public virtual IMaterial Material { get; protected set; }

    public abstract float[] Vertices { get; }

    public abstract uint[] Indices { get; }

    protected BaseModel(Vector3? position, Quaternion? rotation, Vector3? scale) {
        vao = GL.GenVertexArray();
        ebo = GL.GenBuffer();
        vertexHandle = GL.GenBuffer();

        Tools.TsGlCall(() => {
            // Vertex Array Object - Bundles the data into a single buffer
            GL.BindVertexArray(vao);

            // The vertices data
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            // Vertices attributes (data layout)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), 0 * sizeof(float));
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, (3 + 3 + 2) * sizeof(float), (3 + 3) * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            // Element Buffer Object - How to draw the vertices
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);
        });


        this.position = position ?? new Vector3(0f, 0f, 0f);
        this.rotation = rotation ?? new Quaternion(0f, 0f, 0f);
        this.scale = scale ?? new Vector3(1f, 1f, 1f);
        UpdateMatrix();
    }

    public virtual void Update(float time) {

    }

    public virtual void Draw(float time) {
        Tools.TsGlCall(() => {
            GL.BindVertexArray(vao);
            Material.Use();
#warning TODO: Only upload again if changed
            Material.Program.SetMat4("model", ModelMatrix);
#warning TODO: These 2 should be inside a shared uniform
            Material.Program.SetMat4("view", SceneManager.GetViewMatrix);
            Material.Program.SetMat4("projection", SceneManager.GetProjectionMatrix);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        });
    }

    public void UpdateMatrix() {
        ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);
    }

    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            Material.Dispose();
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
