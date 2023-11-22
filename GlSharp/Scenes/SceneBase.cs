using GlSharp.Cameras;

using OpenTK.Windowing.Common;

namespace GlSharp.Scene;
public class SceneBase : IScene {
    public virtual ICamera ActiveCamera { get; set; } = new FreeCamera();
    public virtual void Close() { }
    public virtual void Draw(FrameEventArgs args) { }
    public virtual void Load() { }
    public virtual void Update(FrameEventArgs args) { }
}
