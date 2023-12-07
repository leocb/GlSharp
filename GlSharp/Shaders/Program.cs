using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GlSharp.Shaders;

public class Program : IProgram {
    private readonly int handle;
    private readonly Dictionary<string, int> locationMap = new();
    private static readonly Dictionary<int, int> programList = new();

    public Program(string vertexSourceName, string fragmentSourceName) {

        int hash = vertexSourceName.GetHashCode() + fragmentSourceName.GetHashCode();

        if (programList.TryGetValue(hash, out handle))
            return;

        handle = GlslCompiler.CreateProgram(vertexSourceName, fragmentSourceName);

        programList.Add(hash, handle);
    }

    public void Use() {
        GL.UseProgram(handle);
    }

    public int GetAttribLocation(string attribName) {
        if (!locationMap.TryGetValue(attribName, out int location)) {
            location = GL.GetAttribLocation(handle, attribName);
            _ = locationMap.TryAdd(attribName, location);
        }

        return location;
    }

    public int GetUniformLocation(string uniformName) {
        if (!locationMap.TryGetValue(uniformName, out int location)) {
            location = GL.GetUniformLocation(handle, uniformName);
            _ = locationMap.TryAdd(uniformName, location);
        }

        return location;
    }

    public void SetInt(string uniformName, int value) => GL.Uniform1(GetUniformLocation(uniformName), value);
    public void SetMat4(string uniformName, Matrix4 value) => GL.UniformMatrix4(GetUniformLocation(uniformName), true, ref value);
    public void SetVec3(string uniformName, Vector3 value) => GL.Uniform3(GetUniformLocation(uniformName), ref value);

    public static void UnloadAllPrograms() {
        foreach (KeyValuePair<int, int> item in programList) {
            GL.DeleteProgram(item.Value);
        }

        programList.Clear();
    }
}
