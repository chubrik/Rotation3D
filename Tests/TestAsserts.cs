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
        const float maxDiff = 6.556511E-07f;

        var (m11, m21, m31) = (matrix.M11, matrix.M21, matrix.M31);

        var diff = Abs(m11 * m11 + m21 * m21 + m31 * m31 - 1f);
        var isNormal = diff <= maxDiff;

        if (!isNormal)
            Console.WriteLine($"Diff is {diff}");

        return isNormal;
    }

    public static bool IsUniformScaled(this Matrix4x4 matrix)
    {
        const float maxDiff = 0f; //todo test

        Matrix4x4.Decompose(matrix, out var scale, out _, out _);

        var diff1 = Abs(scale.X - scale.Y);
        var diff2 = Abs(scale.Y - scale.Z);
        var diff3 = Abs(scale.Z - scale.X);
        var diff = Max(Max(diff1, diff2), diff3);
        var isUnisformScaled = diff <= maxDiff;

        if (!isUnisformScaled)
            Console.WriteLine($"Diff is {diff}");

        return isUnisformScaled;
    }

    public static bool IsNormal(this Quaternion quaternion)
    {
        const float maxDiff = 4.7683716E-07f;

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var diff = Abs(x * x + y * y + z * z + w * w - 1f);
        var isNormal = diff <= maxDiff;

        if (!isNormal)
            Console.WriteLine($"Diff is {diff}");

        return isNormal;
    }

    public static bool IsNormal(this Vector3 vector)
    {
        const float maxDiff = 1.7881393E-07f;

        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        var diff = Abs(x * x + y * y + z * z - 1f);
        var isNormal = diff <= maxDiff;

        if (!isNormal)
            Console.WriteLine($"Diff is {diff}");

        return isNormal;
    }

    public static bool IsNormalAngle(float angle)
    {
        return angle >= MINUS_PI && angle <= PI;
    }
}
