using System.Runtime.CompilerServices;

namespace GlSharp.Tools;
internal static class GlTools {
    private static readonly SemaphoreSlim glSemaphore = new(1, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void TsGlCall(Action action) {
        glSemaphore.Wait();
        action();
        _ = glSemaphore.Release();
    }
}
