namespace Trigonometry;

using System.Numerics;
using static MathF;

public static class Vector3Extensions
{
    public static Vector3 Normalize(this Vector3 vector)
    {
        return Vector3.Normalize(vector);
    }

    // For debug only
    public static bool IsNormalized(this Vector3 vector)
    {
        var (x, y, z) = (vector.X, vector.Y, vector.Z);
        return Abs(x * x + y * y + z * z - 1) < 0.00000036;
    }
}
