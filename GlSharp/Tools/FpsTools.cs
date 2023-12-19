using Cyotek.Collections.Generic;

using OpenTK.Windowing.Desktop;

namespace GlSharp.Tools;
internal static class FpsTools
{
    private static readonly CircularBuffer<float> fpsDeltas = new(60);
    internal static float AverageFps => 1 / fpsDeltas.Average();
    internal static void UpdateAverageFps(double deltaTime)
    {
        fpsDeltas.Put((float)deltaTime);
    }
    internal static void ShowFpsCounter(NativeWindow window)
    {
        _ = Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                window.Title = $"{Engine.TITLE} - {AverageFps:0.0}FPS";
            }
        });
    }
}
