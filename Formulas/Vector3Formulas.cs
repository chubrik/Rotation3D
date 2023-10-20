namespace Rotation3D;

using System.Numerics;
using static MathF;

public static class Vector3Formulas
{
    public static Vector3 Normalize(this Vector3 vector)
    {
        // Reference:
        // return Vector3.Normalize(vector);

        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        var invNorm = 1f / Sqrt(x * x + y * y + z * z);
        return new Vector3(x * invNorm, y * invNorm, z * invNorm);
    }
}
