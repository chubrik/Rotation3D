namespace Rotation3D.Tests;

using System.Numerics;
using static MathF;
using static MathFConstants;

public static class TestAsserts
{
    public static bool IsNormal(this AxisAngle axisAngle)
    {
        return axisAngle.Axis.IsNormal()
            && IsNormalAngle(axisAngle.Angle);
    }

    public static bool IsNormal(this EulerAngles eulerAngles)
    {
        return IsNormalAngle(eulerAngles.Yaw)
            && eulerAngles.Pitch >= MINUS_HALF_PI
            && eulerAngles.Pitch <= HALF_PI
            && IsNormalAngle(eulerAngles.Roll);
    }

    public static bool IsNormal(this Matrix4x4 matrix)
    {
        const float maxDiff = 5.96046448E-07f;

        var (m11, m21, m31) = (matrix.M11, matrix.M21, matrix.M31);

        return Abs(m11 * m11 + m21 * m21 + m31 * m31 - 1f) <= maxDiff;
    }

    public static bool IsUniformScaled(this Matrix4x4 matrix)
    {
        const float maxDiff = 0.00000024f; //todo test

        Matrix4x4.Decompose(matrix, out var scale, out _, out _);

        return Abs(scale.X - scale.Y) <= maxDiff
            && Abs(scale.Y - scale.Z) <= maxDiff
            && Abs(scale.Z - scale.X) <= maxDiff;
    }

    public static bool IsNormal(this Quaternion quaternion)
    {
        const float maxDiff = 0.00000048f; //todo test

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        return Abs(x * x + y * y + z * z + w * w - 1f) <= maxDiff;
    }

    public static bool IsNormal(this Vector3 vector)
    {
        const float maxDiff = 0.00000036f; //todo test

        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        return Abs(x * x + y * y + z * z - 1f) <= maxDiff;
    }

    public static bool IsNormalAngle(float angle)
    {
        return angle >= MINUS_PI && angle <= PI;
    }
}
