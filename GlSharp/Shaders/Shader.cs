using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GlSharp.Shaders;

internal class Shader : IDisposable
{
    private readonly int handle;
    private readonly Dictionary<string, int> locationMap = new();

    public Shader(string vertexSourceName, string fragmentSourceName)
    {
        this.handle = GlslCompiler.CreateShadersLink(vertexSourceName, fragmentSourceName);
    }

    public void Use()
    {
        GL.UseProgram(this.handle);
    }

    public int GetAttribLocation(string attribName)
    {
        if (!this.locationMap.TryGetValue(attribName, out int location))
        {
            location = GL.GetAttribLocation(this.handle, attribName);
            _ = this.locationMap.TryAdd(attribName, location);
        }

        return location;
    }

    public int GetUniformLocation(string uniformName)
    {
        if (!this.locationMap.TryGetValue(uniformName, out int location))
        {
            location = GL.GetUniformLocation(this.handle, uniformName);
            _ = this.locationMap.TryAdd(uniformName, location);
        }

        return location;
    }

    public void SetInt(string uniformName, int value) => GL.Uniform1(GetUniformLocation(uniformName), value);
    public void SetMat4(string uniformName, Matrix4 value) => GL.UniformMatrix4(GetUniformLocation(uniformName), true, ref value);

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
