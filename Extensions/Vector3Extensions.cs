namespace Trigonometry;

using System.Numerics;
using static MathF;

internal static class Vector3Extensions
{
    public static bool IsNormalized(this Vector3 vector)
    {
        var (x, y, z) = (vector.X, vector.Y, vector.Z);
        return Abs(x * x + y * y + z * z - 1) < 0.00000025;
    }
}
