namespace Rotation3D;

using System.Numerics;
using static MathF;

public static class Vector3Formulas
{
    public static Vector3 Normalize(this Vector3 vector)
    {
        // Reference: Vector3.Normalize(vector);

        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        var sqLen = x * x + y * y + z * z;

        if (sqLen == 0)
            return Vector3.Zero;

        var invLen = 1f / Sqrt(sqLen);
        return new Vector3(x * invLen, y * invLen, z * invLen);
    }
}
