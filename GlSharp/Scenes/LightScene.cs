using GlSharp.Behavior;
using GlSharp.Cameras;
using GlSharp.Entities;
using GlSharp.Materials;
using GlSharp.Models;
using GlSharp.Objects;
using GlSharp.Scene;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp.Scenes;
public class LightScene : SceneBase {

    PhongMaterial material;

    public override void Load() {
        ActiveCamera = new FreeCamera();
        ActiveCamera.Init(Engine.window.MousePosition, Engine.window.Size);

        LampObj lamp = new(
            new(0f, 2f, 0f),
            new(1f, 1f, 1f),
            null);

        material = new (new(1f, 1f, 1f), lamp.LightColor, lamp.Position);

        entityList.Add(lamp);
        entityList.Add(new CubeModel(
            new(0f, 0f, 0f),
            null,
            null,
            new() { new RandomRotationBehavior() },
            material));
        entityList.Add(new CubeModel(
            new(1.5f, 0f, 1.5f),
            null,
            null,
            new() { new RandomRotationBehavior() },
            material));
        entityList.Add(new CubeModel(
            new(-1.5f, 0f, -1.5f),
            null,
            null,
            new() { new RandomRotationBehavior() },
            material));
        entityList.Add(new CubeModel(
            new(1.5f, 0f, -1.5f),
            null,
            null,
            new() { new RandomRotationBehavior() },
            material));
        entityList.Add(new CubeModel(
            new(-1.5f, 0f, 1.5f),
            null,
            null,
            new() { new RandomRotationBehavior() },
            material));
    }

    public override void Update(FrameEventArgs args) {

        KeyboardState keyboard = Engine.window.KeyboardState;

        if (keyboard.IsKeyDown(Keys.Escape))
            Engine.window.Close();

        if (keyboard.IsKeyDown(Keys.R))
            SceneManager.SetActiveScene(new LightScene());

        if (SceneManager.GetActiveCamera is FreeCamera camera) {
            camera.UpdateOrientationPosition(Engine.window.MousePosition, keyboard, (float)args.Time);
        }

        material.UpdateCamera(SceneManager.GetActiveCamera.Position);

        foreach (IEntity entity in entityList) {
            entity.Update((float)args.Time);
        }
    }
}
