using OpenTK.Graphics.OpenGL;

namespace GlSharp.Shaders;

internal class Shader : IDisposable
{
    private readonly int handle;

    public Shader(string vertexSourceName, string fragmentSourceName)
    {
        this.handle = ShaderCompiler.CreateShadersLink(vertexSourceName, fragmentSourceName);
    }

    public void Use()
    {
        GL.UseProgram(this.handle);
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(this.handle, attribName);
    }

    ~Shader()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            GL.DeleteProgram(this.handle);
        }
        else
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }
}
