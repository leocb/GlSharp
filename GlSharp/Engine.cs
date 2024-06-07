using System.Diagnostics;

using GlSharp.Scene;
using GlSharp.Content.Scenes;
using GlSharp.Tools;

using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace GlSharp;

public class Engine : GameWindow
{
    public const string TITLE = "GlSharp";

    public static GameWindow window;
    public static Stopwatch Time { get; } = Stopwatch.StartNew();

    public Engine(int width, int height)
        : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            ClientSize = (width, height),
            Title = "Loading...",
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

        CursorState = CursorState.Grabbed;
        if (SupportsRawMouseInput)
            RawMouseInput = true;
        //VSync = VSyncMode.On;
        //UpdateFrequency = 60;

#if DEBUG
        GL.DebugMessageCallback(Debug.DebugMessageDelegate, IntPtr.Zero);
        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);
#endif

        //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Back);
        GL.FrontFace(FrontFaceDirection.Ccw);

        GL.ClearColor(0.553f, 0.796f, 0.992f, 1.0f);
        GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        GL.Enable(EnableCap.DepthTest);

        SceneManager.SetActiveScene(new FirstModelScene());

        FpsTools.ShowFpsCounter(this);
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
