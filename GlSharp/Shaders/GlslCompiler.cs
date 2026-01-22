using OpenTK.Graphics.OpenGL;

namespace GlSharp.Shaders;

internal static class GlslCompiler
{

    public static int CreateProgram(string vertexSourceName, string fragmentSourceName)
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
        GL.GetProgrami(handle, ProgramProperty.LinkStatus, out int success);
        if (success == 0)
        {
            GL.GetProgramInfoLog(handle, out string infoLog);
            Console.WriteLine(infoLog);
            _ = Console.Read();
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
        GL.GetShaderi(handle, ShaderParameterName.CompileStatus, out int result);
        if (result == 0)
        {
            GL.GetShaderInfoLog(handle, out string infoLog);
            Console.WriteLine(infoLog);
            _ = Console.Read();
        }

        return handle;
    }
}
