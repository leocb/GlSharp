using GlSharp.Cameras;
using GlSharp.Entities;
using GlSharp.Materials;
using GlSharp.Models;
using GlSharp.Objects;
using GlSharp.Scene;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp.Scenes;
public class LightMapsScene : SceneBase {

    private PhongTexturedMaterial phongMat;
    private PointLightObj light;

    public override void Load() {
        ActiveCamera = new FreeCamera();
        ActiveCamera.Init(Engine.window.MousePosition, Engine.window.Size);

        light = new(
            new(0.6f, 2f, 1.5f),
            new(1.0f, 1.0f, 1.0f),
            new(0.2f, 0.2f, 0.2f),
            new(1.0f, 1.0f, 1.0f),
            null);

        phongMat = new(light, "container2.png");

        entityList.Add(light);
        entityList.Add(new CubeModel(
            new(0f, 0f, 0f),
            null,
            null,
            null,
            phongMat));
    }

    public override void Update(FrameEventArgs args) {

        KeyboardState keyboard = Engine.window.KeyboardState;

        if (keyboard.IsKeyDown(Keys.Escape))
            Engine.window.Close();

        if (keyboard.IsKeyDown(Keys.R))
            SceneManager.SetActiveScene(new LightMapsScene());

        if (SceneManager.GetActiveCamera is FreeCamera camera) {
            camera.UpdateOrientationPosition(Engine.window.MousePosition, keyboard, (float)args.Time);
        }

        phongMat.UpdateCamera(SceneManager.GetActiveCamera.Position);

        foreach (IEntity entity in entityList) {
            entity.Update((float)args.Time);
        }
    }
}
