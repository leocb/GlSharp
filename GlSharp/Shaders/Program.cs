﻿using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GlSharp.Shaders;

internal class Program : IDisposable {
    private readonly int handle;
    private readonly Dictionary<string, int> locationMap = new();

    public Program(string vertexSourceName, string fragmentSourceName) {
        handle = GlslCompiler.CreateProgram(vertexSourceName, fragmentSourceName);
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

    protected virtual void Dispose(bool disposing) {
        if (!disposing)
            return;

        GL.DeleteProgram(handle);
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}