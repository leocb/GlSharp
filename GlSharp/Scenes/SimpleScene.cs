using GlSharp.Behavior;
using GlSharp.Cameras;
using GlSharp.Entities;
using GlSharp.Materials;
using GlSharp.Models;
using GlSharp.Scene;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp.Scenes;
public class SimpleScene : SceneBase {

    public override void Load() {
        ActiveCamera = new FreeCamera();
        ActiveCamera.Init(Engine.window.MousePosition, Engine.window.Size);

        entityList.Add(new PlaneModel(new(0f, 0f, 0f), null, null, null));
        entityList.Add(new CubeModel(new(1.5f, 0f, 0f), null, null, new() {new RandomRotationBehavior()}));
        entityList.Add(new CubeModel(new(2.5f, 0f, 0f), null, null, null, new AwesomeCrateMaterial()));
        entityList.Add(new CubeModel(new(3.5f, 0f, 0f), null, null, null, new AwesomeCrateMaterial()));
        entityList.Add(new CubeModel(new(4.5f, 0f, 0f), null, null, null, new AwesomeCrateMaterial()));
        entityList.Add(new CubeModel(new(5.5f, 0f, 0f), null, null, null, new AwesomeCrateMaterial()));
        entityList.Add(new CubeModel(new(6.5f, 0f, 0f), null, null, null, new AwesomeCrateMaterial()));
        entityList.Add(new CubeModel(new(-1.5f, 0f, 0f), null, null, null));
    }

    public override void Update(FrameEventArgs args) {

        KeyboardState keyboard = Engine.window.KeyboardState;

        if (keyboard.IsKeyDown(Keys.Escape))
            Engine.window.Close();

        if (keyboard.IsKeyDown(Keys.R))
            SceneManager.SetActiveScene(new SimpleScene());

        if (SceneManager.GetActiveCamera is FreeCamera camera) {
            camera.UpdateOrientationPosition(Engine.window.MousePosition, keyboard, (float)args.Time);
        }

        foreach (IEntity entity in entityList) {
            entity.Update((float)args.Time);
        }
    }
}
