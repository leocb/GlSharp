﻿using GlSharp.Behavior;
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

        entityList.Add(new PlaneModel(new(0f, 0f, 0f), null, null, null));
        entityList.Add(new CubeModel(new(2f, 0f, 0f), null, null, new() {new RandomRotationBehavior()}));
        entityList.Add(new CubeModel(new(3f, 0f, 0f), null, null, new() {new RandomRotationBehavior()}));
        entityList.Add(new CubeModel(new(4f, 0f, 0f), null, null, new() {new RandomRotationBehavior()}));
        entityList.Add(new CubeModel(new(5f, 0f, 0f), null, null, new() {new RandomRotationBehavior()}));
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
