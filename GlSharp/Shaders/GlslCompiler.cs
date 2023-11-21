using OpenTK.Graphics.OpenGL;

namespace GlSharp.Shaders;

internal static class GlslCompiler
{

    public static int CreateShadersLink(string vertexSourceName, string fragmentSourceName)
    {
        // Compile
        int vertexHandle = CompileShader(vertexSourceName, ShaderType.VertexShader);
        int fragmentHandle = CompileShader(fragmentSourceName, ShaderType.FragmentShader);

        // Link to Program
        int handle = GL.CreateProgram();
        GL.AttachShader(handle, vertexHandle);
        GL.AttachShader(handle, fragmentHandle);
        GL.LinkProgram(handle);

        // Check for failure
        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(handle);
            Console.WriteLine(infoLog);
        }

        // Clean up
        GL.DetachShader(handle, vertexHandle);
        GL.DetachShader(handle, fragmentHandle);
        GL.DeleteShader(vertexHandle);
        GL.DeleteShader(fragmentHandle);

        return handle;
    }

    private static int CompileShader(string sourceName, ShaderType type)
    {
        // Compile
        int handle = GL.CreateShader(type);
        GL.ShaderSource(handle, File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Assets", "Shaders", sourceName)));
        GL.CompileShader(handle);

        // Check for failure
        GL.GetShader(handle, ShaderParameter.CompileStatus, out int result);
        if (result == 0)
        {
            string infoLog = GL.GetShaderInfoLog(handle);
            Console.WriteLine(infoLog);
        }

        return handle;
    }
}
