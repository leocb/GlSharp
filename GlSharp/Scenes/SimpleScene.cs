using GlSharp.Cameras;
using GlSharp.Entities;
using GlSharp.Models;
using GlSharp.Scene;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp.Scenes;
public class SimpleScene : SceneBase {

    public override void Load() {
        ActiveCamera = new FreeCamera();
        ActiveCamera.Init(Engine.window.MousePosition, Engine.window.Size);

        entityList.Add(new PlaneModel(new(0f, 0f, 0f), null, null));
    }

    public override void Update(FrameEventArgs args) {

        if (Engine.window.KeyboardState.IsKeyDown(Keys.Escape))
            Engine.window.Close();

        if (SceneManager.GetActiveCamera is FreeCamera camera) {
            camera.UpdateOrientationPosition(Engine.window.MousePosition, Engine.window.KeyboardState, (float)args.Time);
        }

        foreach (IEntity entity in entityList) {
            entity.Update((float)args.Time);
        }
    }
}
