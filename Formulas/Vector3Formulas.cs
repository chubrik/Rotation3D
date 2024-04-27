namespace Rotation3D;

using System.Numerics;
using static MathF;

public static class Vector3Formulas
{
    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Vector3.Normalize(Vector3)"/>
    /// </summary>
    public static Vector3 Normalize(this Vector3 vector)
    {
        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        var invLen = 1f / Sqrt(x * x + y * y + z * z);
        return new Vector3(x * invLen, y * invLen, z * invLen);
    }
}
