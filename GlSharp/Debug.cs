using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL;

namespace GlSharp;
public static class Debug
{

    public static readonly GLDebugProc DebugMessageDelegate =
        (source, type, id, severity, length, pMessage, param) => // The pointer you gave to OpenGL, explained later.
        {
            // In order to access the string pointed to by pMessage, you can use Marshal
            // class to copy its contents to a C# string without unsafe code. You can
            // also use the new function Marshal.PtrToStringUTF8 since .NET Core 1.1.
            string message = Marshal.PtrToStringAnsi(pMessage, length);

            // The rest of the function is up to you to implement, however a debug output
            // is always useful.
            Console.WriteLine("[{0} source={1} type={2} id={3}] {4}", severity, source, type, id, message);
        };
}
