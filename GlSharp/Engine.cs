using System.Diagnostics;

using GlSharp.Scene;
using GlSharp.Scenes;

using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp;

public class Engine : GameWindow {
    public const string TITLE = "Lengine";

    public static GameWindow window;
    public static Stopwatch Time { get; } = Stopwatch.StartNew();

    public Engine(int width, int height)
        : base(GameWindowSettings.Default, new NativeWindowSettings() {
            Size = (width, height),
            Title = TITLE,
#if DEBUG
            Flags = ContextFlags.Debug
#endif
        }) {
        this.Resize += EngineResize;
        this.Load += EngineLoad;
        this.RenderFrame += EngineRenderFrame;
        this.UpdateFrame += EngineUpdateFrame;
        this.MouseWheel += EngineMouseWheel;

        window = this;
    }

    private void EngineLoad() {
        PrintHardwareSupport();
        Tools.ShowFpsCounter(this);

        CursorState = CursorState.Grabbed;
        VSync = VSyncMode.On;
        //UpdateFrequency = 60;

#if DEBUG
        GL.DebugMessageCallback(Debug.DebugMessageDelegate, IntPtr.Zero);
        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);
#endif

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        GL.Enable(EnableCap.DepthTest);

        SceneManager.SetActiveScene(new SimpleScene());

    }

    private void EngineUpdateFrame(FrameEventArgs obj) {
        if (!IsFocused)
            return;

        Tools.UpdateAverageFps(obj.Time);
        SceneManager.Update(obj);
    }

    private void EngineRenderFrame(FrameEventArgs obj) {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        SceneManager.Draw(obj);
        SwapBuffers();
    }

    private void EngineResize(ResizeEventArgs e) {
        GL.Viewport(0, 0, e.Width, e.Height);
        SceneManager.GetActiveCamera.ChangeWindowSize(Size);
    }

    private void EngineMouseWheel(MouseWheelEventArgs obj) {
        SceneManager.GetActiveCamera.ChangeFov(-obj.OffsetY);
    }

    private static void PrintHardwareSupport() {
        GL.GetInteger(GetPName.MaxVertexAttribs, out int nrAttributes);
        Console.WriteLine($"Hardware supports:");
        Console.WriteLine($"Max number of vertex attributes: {nrAttributes}");
        Console.WriteLine($"---");
    }
}
