
using System.Drawing;

using GlSharp.Cameras;
using GlSharp.Entities;
using GlSharp.Models;
using GlSharp.Scene;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp.Scenes;
public class SimpleScene : SceneBase {

    private readonly List<IEntity> entityList = new();

    public override void Load() {
        GameWindow window = Engine.Window;

        entityList.Add(new Basic());

        ActiveCamera = new FreeCamera();
        ActiveCamera.Init(window.MousePosition, window.Size);
    }

    public override void Update(FrameEventArgs args) {
        GameWindow window = Engine.Window;

        if (window.KeyboardState.IsKeyDown(Keys.Escape))
            window.Close();

        if (SceneManager.GetActiveCamera is FreeCamera camera) {
            camera.UpdateOrientationPosition(window.MousePosition, window.KeyboardState, (float)args.Time);
        }

        foreach (IEntity entity in entityList) {
            entity.Update((float)args.Time);
        }
    }

    public override void Draw(FrameEventArgs args) {

        foreach (IEntity entity in entityList) {
            entity.Draw((float)args.Time);
        }
    }

    public override void Close() {
        foreach (IEntity entity in entityList) {
            entity.Dispose();
        }
    }
}
