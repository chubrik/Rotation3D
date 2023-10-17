namespace Trigonometry;

using System.Numerics;
using static MathF;

public static class Vector3Extensions
{
    public static Vector3 Normalize(this Vector3 vector)
    {
        // Reference:
        // return Vector3.Normalize(vector);

        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        var invNorm = 1f / Sqrt(x * x + y * y + z * z);
        return new Vector3(x * invNorm, y * invNorm, z * invNorm);
    }

    /// <summary>
    /// For debug only
    /// </summary>
    public static bool IsUnit(this Vector3 vector)
    {
        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        return Abs(x * x + y * y + z * z - 1f) < 0.00000036f;
    }
}
