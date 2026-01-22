using System.Diagnostics;
using System.Runtime.InteropServices;

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
            NumberOfSamples = 4,
            ClientSize = (width, height),
            Title = "Loading...",
            API = ContextAPI.OpenGL,
            APIVersion = new Version(3,3),
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
        VSync = VSyncMode.On;
        UpdateFrequency = 60;

#if DEBUG
        // This causes the app to crash in Mac OS, no idea why
        // GL.DebugMessageCallback((source, type, id, severity, length, pMessage, param) =>
        // {
        //     string message = Marshal.PtrToStringAnsi(pMessage, length);
        //     Console.WriteLine("[{0} source={1} type={2} id={3}] {4}", severity, source, type, id, message);
        // },[0]);
        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);
#endif

        // MSAA
        GL.Enable(EnableCap.Multisample);
        
        //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

        GL.Enable(EnableCap.CullFace);
        GL.CullFace(TriangleFace.Back);
        GL.FrontFace(FrontFaceDirection.Ccw);

        GL.ClearColor(0.553f, 0.796f, 0.992f, 1.0f);
        //GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest); // what did this do?
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
        GL.Viewport(0, 0, this.FramebufferSize.X, FramebufferSize.Y);
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
