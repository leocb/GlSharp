using GlSharp.Cameras;
using GlSharp.Entities;
using GlSharp.Materials;
using GlSharp.Models;
using GlSharp.Objects;
using GlSharp.Scene;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp.Scenes;
public class LightScene : SceneBase {

    private PhongGoldMaterial goldMaterial;
    private Vector3 lightColor;
    private PointLightObj light;

    public override void Load() {
        ActiveCamera = new FreeCamera();
        ActiveCamera.Init(Engine.window.MousePosition, Engine.window.Size);

        light = new(
            new(0f, 2f, 0f),
            new(0.5f, 0.5f, 0.5f),
            new(0.2f, 0.2f, 0.2f),
            new(1.0f, 1.0f, 1.0f),
            32f,1f,
            null);

        goldMaterial = new(light);

        entityList.Add(light);
        entityList.Add(new CubeModel(
            new(0f, 0f, 0f),
            null,
            null,
            null,
            goldMaterial));
        entityList.Add(new CubeModel(
            new(1.5f, 0f, 1.5f),
            null,
            null,
            null,
            goldMaterial));
        entityList.Add(new CubeModel(
            new(-1.5f, 0f, -1.5f),
            null,
            null,
            null,
            goldMaterial));
        entityList.Add(new CubeModel(
            new(1.5f, 0f, -1.5f),
            null,
            null,
            null,
            goldMaterial));
        entityList.Add(new CubeModel(
            new(-1.5f, 0f, 1.5f),
            null,
            null,
            null,
            goldMaterial));
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

        goldMaterial.UpdateCamera(SceneManager.GetActiveCamera.Position);

        float time = (float)Engine.Time.Elapsed.TotalSeconds;
        lightColor.X = (float)Math.Cos(time) + 1.5f;
        lightColor.Y = (float)Math.Cos(time) + 1.5f;
        lightColor.Z = (float)Math.Cos(time) + 1.5f;

        light.AmbientColor = lightColor * new Vector3(0.2f);
        light.DifuseColor = lightColor * new Vector3(0.5f);

        goldMaterial.UpdateLight(light);

        foreach (IEntity entity in entityList) {
            entity.Update((float)args.Time);
        }
    }
}
