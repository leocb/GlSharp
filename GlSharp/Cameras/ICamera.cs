using OpenTK.Mathematics;

namespace GlSharp.Cameras;
public interface ICamera {
    Vector3 Position { get; }
    Vector3 Direction { get; }
    Matrix4 ProjectionMatrix { get; }
    Matrix4 ViewMatrix { get; }
    float Sensitivity { get; set; }
    float Speed { get; set; }

    void Init(Vector2 mouse, Vector2 windowSize);
    void SetPosition(Vector3 position);
    void SetLookAt(Vector3 target);
    void ChangeFov(float newFov, bool treatAsDelta = true);
    void ChangeWindowSize(Vector2 newWindowSize);
    void UpdateViewMatrix();
    void UpdateProjectionMatrix();
}