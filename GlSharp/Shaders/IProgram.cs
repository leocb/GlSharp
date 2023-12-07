using OpenTK.Mathematics;

namespace GlSharp.Shaders;
public interface IProgram {
    int GetAttribLocation(string attribName);
    int GetUniformLocation(string uniformName);
    void SetInt(string uniformName, int value);
    void SetMat4(string uniformName, Matrix4 value);
    void Use();
}