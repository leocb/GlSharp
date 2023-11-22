using GlSharp.Models;

using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp;

internal class Engine : GameWindow {
    public const string TITLE = "Lengine";
    private Basic? basicShape;
    public Engine(int width, int height, string title)
        : base(GameWindowSettings.Default, new NativeWindowSettings() {
            Size = (width, height),
            Title = TITLE
        }) {
        this.Resize += EngineResize;
        this.Load += EngineLoad;
        this.Unload += EngineUnload;
        this.RenderFrame += EngineRenderFrame;
        this.UpdateFrame += EngineUpdateFrame;
        this.MouseWheel += EngineMouseWheel;
    }

    private void EngineLoad() {
        PrintHardwareSupport();
        Tools.ShowFpsCounter(this);

        CursorState = CursorState.Grabbed;
        FreeCamera.Init(MousePosition);

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);
        basicShape = new();
    }

    private void EngineUpdateFrame(FrameEventArgs obj) {
        if (!IsFocused)
            return;

        if (KeyboardState.IsKeyDown(Keys.Escape))
            Close();

        Tools.UpdateAverageFps(obj.Time);

        FreeCamera.Update(KeyboardState, MousePosition, (float)obj.Time);
        basicShape?.Update((float)obj.Time);
    }

    private void EngineRenderFrame(FrameEventArgs obj) {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        basicShape?.Draw();
        SwapBuffers();
    }

    private void EngineUnload() {
        basicShape?.Dispose();
    }

    private void EngineResize(ResizeEventArgs e) {
        GL.Viewport(0, 0, e.Width, e.Height);
        FreeCamera.UpdateCameraFov(0.0f, Size);
    }

    private void EngineMouseWheel(MouseWheelEventArgs obj) {
        FreeCamera.UpdateCameraFov(obj.OffsetY, Size);
    }

    private static void PrintHardwareSupport() {
        GL.GetInteger(GetPName.MaxVertexAttribs, out int nrAttributes);
        Console.WriteLine($"Hardware supports:");
        Console.WriteLine($"Max number of vertex attributes: {nrAttributes}");
        Console.WriteLine($"---");
    }
}
