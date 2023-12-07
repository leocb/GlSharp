using GlSharp.Cameras;
using GlSharp.Scenes;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GlSharp.Scene;
public static class SceneManager {

    private static IScene activeScene = new BlankScene();

    public static void SetActiveScene(IScene scene) {
        activeScene.Close();
        scene.Load();
        activeScene = scene;
    }

    public static void Update(FrameEventArgs args) {
        activeScene.Update(args);
    }

    public static void Draw(FrameEventArgs args) {
        activeScene.Draw(args);
    }

    public static Matrix4 GetProjectionMatrix => activeScene.ActiveCamera.ProjectionMatrix;
    public static Matrix4 GetViewMatrix => activeScene.ActiveCamera.ViewMatrix;
    public static ICamera GetActiveCamera => activeScene.ActiveCamera;
}
