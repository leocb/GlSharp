using GlSharp.Cameras;
using GlSharp.Entities;

using OpenTK.Windowing.Common;

namespace GlSharp.Scene;
public abstract class SceneBase : IScene {
    protected readonly List<IEntity> entityList = new();
    public virtual ICamera ActiveCamera { get; set; } = new FreeCamera();
    public virtual void Close() {
        foreach (IEntity entity in entityList) {
            entity.Dispose();
        }
    }
    public virtual void Draw(FrameEventArgs args) {
        foreach (IEntity entity in entityList) {
            entity.Draw((float)args.Time);
        }
    }
    public virtual void Load() { }
    public virtual void Update(FrameEventArgs args) { }
}
