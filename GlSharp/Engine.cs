using System.Diagnostics;

using GlSharp.Scene;
using GlSharp.Scenes;
using GlSharp.Tools;

using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GlSharp;

public class Engine : GameWindow
{
    public const string TITLE = "Lengine";

    public static GameWindow window;
    public static Stopwatch Time { get; } = Stopwatch.StartNew();

    public Engine(int width, int height)
        : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = (width, height),
            Title = TITLE,
#if DEBUG
            Flags = ContextFlags.Debug
#endif
        })
    {
        this.Resize += EngineResize;
        this.Load += EngineLoad;
        this.RenderFrame += EngineRenderFrame;
        this.UpdateFrame += EngineUpdateFrame;
        this.MouseWheel += EngineMouseWheel;

        window = this;
    }

    private void EngineLoad()
    {
        PrintHardwareSupport();
        FpsTools.ShowFpsCounter(this);

        CursorState = CursorState.Grabbed;
        //VSync = VSyncMode.On;
        //UpdateFrequency = 60;

#if DEBUG
        GL.DebugMessageCallback(Debug.DebugMessageDelegate, IntPtr.Zero);
        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);
#endif

        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Back);
        GL.FrontFace(FrontFaceDirection.Ccw);

        GL.ClearColor(0f, 0f, 0f, 1.0f);
        // GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        GL.Enable(EnableCap.DepthTest);

        SceneManager.SetActiveScene(new LightMapsScene());

    }

    private void EngineUpdateFrame(FrameEventArgs obj)
    {
        if (!IsFocused)
            return;

        FpsTools.UpdateAverageFps(obj.Time);
        SceneManager.Update(obj);
    }

    private void EngineRenderFrame(FrameEventArgs obj)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        SceneManager.Draw(obj);
        SwapBuffers();
    }

    private void EngineResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
        SceneManager.GetActiveCamera.ChangeWindowSize(Size);
    }

    private void EngineMouseWheel(MouseWheelEventArgs obj)
    {
        SceneManager.GetActiveCamera.ChangeFov(-obj.OffsetY);
    }

    private static void PrintHardwareSupport()
    {
        GL.GetInteger(GetPName.MaxVertexAttribs, out int nrAttributes);
        Console.WriteLine($"Hardware supports:");
        Console.WriteLine($"Max number of vertex attributes: {nrAttributes}");
        Console.WriteLine($"---");
    }
}
