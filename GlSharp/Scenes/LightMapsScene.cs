using GlSharp.Cameras;
using GlSharp.Entities;
using GlSharp.Materials;
using GlSharp.Models;
using GlSharp.Objects;
using GlSharp.Scene;
using GlSharp.Tools;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp.Scenes;
public class LightMapsScene : SceneBase
{

    private PhongTexturedMaterial phongMat;
    private SunLight sun;
    private PointLight[] lamps;
    private SpotLight flashlight;

    public override void Load()
    {
        ActiveCamera = new FreeCamera();
        ActiveCamera.Init(Engine.window.MousePosition, Engine.window.Size);

        flashlight = new(
            SceneManager.GetActiveCamera.Position,
            SceneManager.GetActiveCamera.Direction,
            14f, 18f,
            new(1.0f, 0f, 0f),
            new(0.1f, 0f, 0f),
            new(1.0f, .8f, .8f),
            50f, 2f, null);

        lamps = new PointLight[]{
            new(new(0.7f, 0.2f, 2.0f),
                new(1.0f, 1.0f, 0f),
                new(0.2f, 0.2f, 0f),
                new(1.0f, 1.0f, 0f),
                50f, 2f, null),
            new(new(2.3f, -3.3f, -4.0f),
                new(0f, 1.0f, 1.0f),
                new(0f, 0.2f, 0.2f),
                new(0f, 1.0f, 1.0f),
                50f, 2f, null),
            new(new(-4.0f, 2.0f, -12.0f),
                new(1.0f, 0f, 1.0f),
                new(0.2f, 0f, 0.2f),
                new(1.0f, 0f, 1.0f),
                50f, 2f, null),
            new(new(0.0f, 0.0f, -3.0f),
                new(0f, 1.0f, 0f),
                new(0f, 0.2f, 0f),
                new(0f, 1.0f, 0f),
                50f, 2f, null)
        };

        sun = new(
            new(-1.0f, -1.0f, -1.0f),
            new(1.0f, .98f, .9f),
            new(0.0f, 0.1f, 0.2f),
            new(1.0f, .98f, .9f),
            null);

        phongMat = new(flashlight, lamps, sun, "container2.png");

        entityList.Add(flashlight);

        for (int i = 0; i < 10; i++)
        {
            entityList.Add(new CubeModel(
                MathTools.GetRandomUnitVector() * (Random.Shared.NextSingle() * 10f),
                MathTools.GetRandomRotation(),
                null,
                null,
                phongMat));
        }
    }

    public override void Update(FrameEventArgs args)
    {

        KeyboardState keyboard = Engine.window.KeyboardState;

        if (keyboard.IsKeyDown(Keys.Escape))
            Engine.window.Close();

        if (keyboard.IsKeyDown(Keys.R))
            SceneManager.SetActiveScene(new LightMapsScene());

        if (SceneManager.GetActiveCamera is FreeCamera camera)
        {
            camera.UpdateOrientationPosition(Engine.window.MousePosition, keyboard, (float)args.Time);
        }

        flashlight.Position = SceneManager.GetActiveCamera.Position;
        flashlight.Direction = SceneManager.GetActiveCamera.Direction;
        phongMat.UpdateCamera(SceneManager.GetActiveCamera.Position);
        phongMat.UpdateFlashlight(flashlight);

        foreach (IEntity entity in entityList)
        {
            entity.Update((float)args.Time);
        }
    }
}
