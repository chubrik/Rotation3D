namespace Rotation3D;

using System.Numerics;
using static Constants;
using static MathF;

public static class CommonFormulas
{
    public static Vector3 Normalize(this Vector3 vector)
    {
        // Reference: Vector3.Normalize(vector);

        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        var invLen = 1f / Sqrt(x * x + y * y + z * z);
        return new Vector3(x * invLen, y * invLen, z * invLen);
    }

    public static float NormalizeAngle(this float angle)
    {
        return angle < -F_PI ? angle + F_TWO_PI
            : angle > F_PI ? angle - F_TWO_PI
            : angle;
    }
}
