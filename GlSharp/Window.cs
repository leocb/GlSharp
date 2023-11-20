using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp;

internal class Window : GameWindow
{
    public Window(int width, int height, string title)
        : base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = (width, height),
            Title = title
        })
    {
        this.Resize += Window_Resize;
        this.Load += Window_Load;
        this.RenderFrame += Window_RenderFrame;
        this.UpdateFrame += Window_UpdateFrame;
    }

    private void Window_UpdateFrame(FrameEventArgs obj)
    {
        if (this.KeyboardState.IsKeyDown(Keys.Escape))
            Close();
    }

    private void Window_RenderFrame(FrameEventArgs obj)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        SwapBuffers();
    }

    private void Window_Load()
    {
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    }

    private void Window_Resize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}
