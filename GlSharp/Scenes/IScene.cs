using GlSharp.Cameras;

using OpenTK.Windowing.Common;

namespace GlSharp.Scene;

public interface IScene {
    ICamera ActiveCamera { get; set; }
    void Close();
    void Draw(FrameEventArgs args);
    void Load();
    void Update(FrameEventArgs args);
}