using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GlSharp;

internal static class FreeCamera
{
    private static Vector3 up = Vector3.UnitY;
    private static Vector3 right = Vector3.UnitX;

    private static Vector2 lastPos;

    private static Vector3 front = new(0.0f, 0.0f, 0.0f);
    private static Vector3 position = new(0.0f, 0.0f, 3.0f);

    private static float yaw = -90.0f;
    private static float pitch = 0.0f;

    private static float fov = 90.0f;

    internal static float Sensitivity { get; private set; } = 0.1f;
    internal static Matrix4 ViewMatrix { get; private set; }
    internal static Matrix4 ProjectionMatrix { get; private set; }
    internal static float Speed { get; set; } = 3.5f;

    internal static void Update(KeyboardState kb, Vector2 mouse, float deltaT)
    {
        UpdateCameraOrientation(mouse);
        UpdateCameraPosition(kb, deltaT);

        ViewMatrix = Matrix4.LookAt(position, position + front, up);
    }

    public static void UpdateCameraFov(float fovDelta, Vector2 windowSize)
    {
        fov = MathHelper.Clamp(fov - fovDelta, 1.0f, 90.0f);
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), windowSize.X / windowSize.Y, 0.1f, 100.0f);
    }


    internal static void Init(Vector2 mouse)
    {
        lastPos = new Vector2(mouse.X, mouse.Y);
    }

    private static void UpdateCameraOrientation(Vector2 mouse)
    {
        float deltaX = mouse.X - lastPos.X;
        float deltaY = mouse.Y - lastPos.Y;
        lastPos = new Vector2(mouse.X, mouse.Y);

        yaw += deltaX * Sensitivity;

        pitch = MathHelper.Clamp(pitch - (deltaY * Sensitivity), -89.0f, 89.0f);

        front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
        front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
        front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
        front = Vector3.Normalize(front);

        right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
        up = Vector3.Normalize(Vector3.Cross(right, front));
    }

    private static void UpdateCameraPosition(KeyboardState kb, float deltaT)
    {
        if (kb.IsKeyDown(Keys.W))
            position += front * Speed * deltaT;
        if (kb.IsKeyDown(Keys.S))
            position -= front * Speed * deltaT;
        if (kb.IsKeyDown(Keys.A))
            position -= right * Speed * deltaT;
        if (kb.IsKeyDown(Keys.D))
            position += right * Speed * deltaT;
        if (kb.IsKeyDown(Keys.LeftShift))
            position += up * Speed * deltaT;
        if (kb.IsKeyDown(Keys.LeftControl))
            position -= up * Speed * deltaT;
    }
}
