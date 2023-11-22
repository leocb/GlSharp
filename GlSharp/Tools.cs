using System.Runtime.CompilerServices;

using Cyotek.Collections.Generic;

using OpenTK.Windowing.Desktop;

namespace GlSharp;

internal static class Tools {
    #region FPS
    private static readonly CircularBuffer<float> fpsDeltas = new(60);
    internal static float AverageFps => 1 / fpsDeltas.Average();
    internal static void UpdateAverageFps(double deltaTime) {
        fpsDeltas.Put((float)deltaTime);
    }
    public static void ShowFpsCounter(NativeWindow window) {
        _ = Task.Run(async () => {
            while (true) {
                await Task.Delay(TimeSpan.FromSeconds(1));
                window.Title = $"{Engine.TITLE} - {AverageFps:0.0}FPS";
            }
        });
    }
    #endregion

    #region GL Calls
    private static readonly SemaphoreSlim glSemaphore = new(1, 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void TsGlCall(Action action) {
        glSemaphore.Wait();
        action();
        _ = glSemaphore.Release();
    }
    #endregion
}
