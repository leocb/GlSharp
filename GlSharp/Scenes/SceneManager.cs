using GlSharp.Cameras;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GlSharp.Scene;
public static class SceneManager {

    private static IScene activeScene = new SceneBase();

    public static void SetActiveScene(IScene scene) {
        activeScene.Close();
        activeScene = scene;
        activeScene.Load();
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
