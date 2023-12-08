using OpenTK.Mathematics;

namespace GlSharp.Tools;
internal static class MathTools {
    internal static Quaternion GetRandomRotation() {
        return Quaternion.FromAxisAngle(GetRandomUnitVector(), Random.Shared.NextSingle() * MathHelper.TwoPi);
    }

    internal static Vector3 GetRandomUnitVector() {
        return new Vector3(
            Random.Shared.NextSingle() - 0.5f,
            Random.Shared.NextSingle() - 0.5f,
            Random.Shared.NextSingle() - 0.5f
            ).Normalized();
    }
}
