using GlSharp.Behavior;
using GlSharp.Entities;
using GlSharp.Materials;
using GlSharp.Mesh;
using GlSharp.Scene;
using GlSharp.Tools;

using OpenTK.Mathematics;

namespace GlSharp.Models;
public class ModelBase : IEntity
{

    protected List<IBehavior> behaviorList;
    private Vector3 position;
    private Vector3 scale;
    private Quaternion rotation;

    public virtual Vector3 Position { get => position; set { position = value; UpdateMatrix(); } }
    public virtual Vector3 Scale { get => scale; set { scale = value; UpdateMatrix(); } }
    public virtual Quaternion Rotation { get => rotation; set { rotation = value; UpdateMatrix(); } }

    public virtual List<MeshBase> Meshes { get; protected set; }

    public virtual Matrix4 ModelMatrix { get; protected set; }

    public virtual IMaterial Material { get; protected set; }

    public ModelBase(string pathname, Vector3? position, Quaternion? rotation, Vector3? scale, List<IBehavior>? behaviorList, IMaterial material)
    {
        Material = material;
        Meshes = ModelLoader.LoadModel(pathname, material.Program);
        this.position = position ?? new Vector3(0f, 0f, 0f);
        this.rotation = rotation ?? new Quaternion(0f, 0f, 0f);
        this.scale = scale ?? new Vector3(1f, 1f, 1f);
        this.behaviorList = behaviorList ?? [];
        UpdateMatrix();
    }

    public virtual void Update(float time)
    {
        behaviorList.ForEach(b => b.Update(this, time));
    }

    public virtual void Draw(float time)
    {
        GlTools.TsGlCall(() =>
        {
            Material.Use();
            Material.Program.SetMat4("model", ModelMatrix);
            Material.Program.SetMat4("view", SceneManager.GetViewMatrix);
            Material.Program.SetMat4("projection", SceneManager.GetProjectionMatrix);
        });

        foreach (MeshBase mesh in Meshes)
        {
            mesh.Draw();
        }
    }

    public void UpdateMatrix()
    {
        ModelMatrix = Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Meshes.ForEach(m => m.Dispose());
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

