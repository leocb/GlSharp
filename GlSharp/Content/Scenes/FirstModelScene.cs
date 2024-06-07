using GlSharp.Cameras;
using GlSharp.Entities;
using GlSharp.Materials;
using GlSharp.Models;
using GlSharp.Objects;
using GlSharp.Scene;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp.Content.Scenes;
public class FirstModelScene : SceneBase
{
    private PhongTexturedMaterial? phongMat;
    private SpotLight? flashlight;

    public override void Load()
    {
        ActiveCamera = new FreeCamera();
        ActiveCamera.Init(Engine.window.MousePosition, Engine.window.Size);

        flashlight = new(
            SceneManager.GetActiveCamera.Position,
            SceneManager.GetActiveCamera.Direction,
            14f, 18f,
            new(1.0f, 1.0f, 1.0f),
            new(0.1f, 0.1f, 0.1f),
            new(1.0f, 1.0f, 1.0f),
            30f, 10f, null);
        entityList.Add(flashlight);

        PointLight[] lamps = [
            new PointLight(
                new(3.0f, 2.0f, 3.0f),
                new(1.0f, 0.8f, 0.8f),
                new(0f, 0f, 0f),
                new(1.0f, 0.8f, 0.8f),
                50f, 1f, null),
            new PointLight(
                new(3.0f, 2.0f, -3.0f),
                new(0.8f, 1.0f, 0.8f),
                new(0f, 0f, 0f),
                new(0.8f, 1.0f, 0.8f),
                50f, 2f, null),
            new PointLight(
                new(-3.0f, 2.0f, 3.0f),
                new(0.8f, 0.8f, 1.0f),
                new(0f, 0f, 0f),
                new(0.8f, 0.8f, 1.0f),
                50f, 1f, null),
            new PointLight(
                new(-3.0f, 2.0f, -3.0f),
                new(1.0f, 1.0f, 0.8f),
                new(0f, 0f, 0f),
                new(1.0f, 1.0f, 0.8f),
                50f, 1f, null)
        ];
        entityList.Add(lamps[0]);
        entityList.Add(lamps[1]);
        entityList.Add(lamps[2]);
        entityList.Add(lamps[3]);

        SunLight sun = new(
            new(-1.0f, -1.0f, -1.0f),
            new(1.0f, 0.98f, 0.9f),
            new(0.0f, 0.1f, 0.2f),
            new(1.0f, 0.98f, 0.9f),
            null);
        entityList.Add(sun);

        phongMat = new(flashlight, lamps, sun);

        entityList.Add(new ModelBase(
            "sponza",
            new(0, 0, 0),
            new(0, 0, 0),
            new(.01f,.01f,.01f),
            null,
            phongMat));
    }

    public override void Update(FrameEventArgs args)
    {

        KeyboardState keyboard = Engine.window.KeyboardState;

        if (keyboard.IsKeyDown(Keys.Escape))
            Engine.window.Close();

        if (keyboard.IsKeyDown(Keys.R))
            SceneManager.SetActiveScene(new FirstModelScene());

        if (SceneManager.GetActiveCamera is FreeCamera camera)
            camera.UpdateOrientationPosition(Engine.window.MousePosition, keyboard, (float)args.Time);

        if (flashlight is not null)
        {
            flashlight.Position = SceneManager.GetActiveCamera.Position;
            flashlight.Direction = SceneManager.GetActiveCamera.Direction;
            phongMat?.UpdateFlashlight(flashlight);
        }

        phongMat?.UpdateCamera(SceneManager.GetActiveCamera.Position);

        foreach (IEntity entity in entityList)
        {
            entity.Update((float)args.Time);
        }
    }
}
