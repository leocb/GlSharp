using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp.Cameras;

public class FreeCamera : ICamera
{

    public Vector3 Position { get; private set; } = new(0.0f, 0.0f, 3.0f);
    public Vector3 Direction { get => front; }
    private Vector3 front = new(0.0f, 0.0f, 0.0f);
    private Vector3 up = Vector3.UnitY;
    private Vector3 right = Vector3.UnitX;

    private Vector2 lastMouse;
    private float yaw = -90.0f;
    private float pitch = 0.0f;
    private Vector2 windowSize = new(800, 600);
    private float fov = 75.0f;

    public float Sensitivity { get; set; } = 0.1f;
    public Matrix4 ViewMatrix { get; private set; }
    public Matrix4 ProjectionMatrix { get; private set; }
    public float Speed { get; set; } = 3.5f;

    public void UpdateOrientationPosition(Vector2 mouse, KeyboardState kb, float deltaT)
    {
        UpdateCameraOrientation(mouse);
        UpdateCameraPosition(kb, deltaT);
        UpdateViewMatrix();
    }

    public void UpdateProjectionMatrix()
    {
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), windowSize.X / windowSize.Y, 0.1f, 100.0f);
    }

    public void UpdateViewMatrix()
    {
        ViewMatrix = Matrix4.LookAt(Position, Position + Direction, up);
    }

    public void ChangeWindowSize(Vector2 newWindowSize)
    {
        windowSize = newWindowSize;
        UpdateProjectionMatrix();
    }

    public void ChangeFov(float newFov, bool treatAsDelta = true)
    {
        fov = Math.Clamp(treatAsDelta ? fov + newFov : newFov, 1.0f, 90.0f);
        UpdateProjectionMatrix();
    }

    public void Init(Vector2 mouse, Vector2 windowSize)
    {
        this.windowSize = windowSize;
        lastMouse = new Vector2(mouse.X, mouse.Y);
        UpdateProjectionMatrix();
    }

    public void SetPosition(Vector3 position) { }

    public void SetLookAt(Vector3 target) { }

    private void UpdateCameraOrientation(Vector2 mouse)
    {
        float deltaX = mouse.X - lastMouse.X;
        float deltaY = mouse.Y - lastMouse.Y;
        lastMouse = new Vector2(mouse.X, mouse.Y);

        yaw += deltaX * Sensitivity;

        pitch = Math.Clamp(pitch - (deltaY * Sensitivity), -89.0f, 89.0f);

        front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
        front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
        front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
        front = Vector3.Normalize(front);

        right = Vector3.Normalize(Vector3.Cross(Direction, Vector3.UnitY));
        up = Vector3.Normalize(Vector3.Cross(right, Direction));
    }

    private void UpdateCameraPosition(KeyboardState kb, float deltaT)
    {
        if (kb.IsKeyDown(Keys.W))
            Position += Direction * Speed * deltaT;
        if (kb.IsKeyDown(Keys.S))
            Position -= Direction * Speed * deltaT;
        if (kb.IsKeyDown(Keys.A))
            Position -= right * Speed * deltaT;
        if (kb.IsKeyDown(Keys.D))
            Position += right * Speed * deltaT;
        if (kb.IsKeyDown(Keys.LeftShift))
            Position += up * Speed * deltaT;
        if (kb.IsKeyDown(Keys.LeftControl))
            Position -= up * Speed * deltaT;
    }
}
