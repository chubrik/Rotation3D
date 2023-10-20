namespace Rotation3D.Debugging;

using System.Numerics;
using static System.MathF;

public static class DebuggingFormulas
{
    public static bool IsUnit(this AxisAngle axisAngle)
    {
        return axisAngle.Axis.IsUnit();
    }

    public static bool IsUnit(this Matrix4x4 matrix)
    {
        Matrix4x4.Decompose(matrix, out var scale, out _, out _);

        return Abs(scale.X - 1f) < 0.00000012f &&
               Abs(scale.Y - 1f) < 0.00000012f &&
               Abs(scale.Z - 1f) < 0.00000012f;
    }

    public static bool HasUniformScale(this Matrix4x4 matrix)
    {
        Matrix4x4.Decompose(matrix, out var scale, out _, out _);

        return Abs(scale.X - scale.Y) < 0.00000024f &&
               Abs(scale.Y - scale.Z) < 0.00000024f &&
               Abs(scale.Z - scale.X) < 0.00000024f;
    }

    public static bool IsUnit(this Quaternion quaternion)
    {
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        return Abs(x * x + y * y + z * z + w * w - 1) <= 0.00000048f;
    }

    public static bool IsUnit(this Vector3 vector)
    {
        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        return Abs(x * x + y * y + z * z - 1f) < 0.00000036f;
    }
}
