namespace Trigonometry;

using System.Numerics;
using static System.MathF;

public static class AxisAngleExtensions
{
    [Obsolete("Not tested.")]
    public static Matrix4x4 ToMatrix(this AxisAngle axisAngle)
    {
        // Reference:
        // return Matrix4x4.CreateFromAxisAngle(axisAngle.Axis, axisAngle.Angle);

        var (x, y, z, angle) = (axisAngle.Axis.X, axisAngle.Axis.Y, axisAngle.Axis.Z, axisAngle.Angle);

        var sa = Sin(angle);
        var ca = Cos(angle);
        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var xy = x * y;
        var xz = x * z;
        var yz = y * z;

        var sax = sa * x;
        var say = sa * y;
        var saz = sa * z;
        var xy_caxy = xy - ca * xy;
        var xz_caxz = xz - ca * xz;
        var yz_cayz = yz - ca * yz;

        var m11 = xx + ca * (1f - xx);
        var m12 = xy_caxy + saz;
        var m13 = xz_caxz - say;

        var m21 = xy_caxy - saz;
        var m22 = yy + ca * (1f - yy);
        var m23 = yz_cayz + sax;

        var m31 = xz_caxz + say;
        var m32 = yz_cayz - sax;
        var m33 = zz + ca * (1f - zz);

        return new Matrix4x4(m11, m12, m13, 0f, m21, m22, m23, 0f, m31, m32, m33, 0f, 0f, 0f, 0f, 1f);
    }

    [Obsolete("Not tested.")]
    public static Quaternion ToQuaternion(this AxisAngle axisAngle)
    {
        // Reference:
        // return Quaternion.CreateFromAxisAngle(axisAngle.Axis.Normalize(), axisAngle.Angle);

        var normAxis = axisAngle.Axis.Normalize(); // Vector must be normalized
        var (x, y, z, angle) = (normAxis.X, normAxis.Y, normAxis.Z, axisAngle.Angle);

        var halfAngle = angle * 0.5f;
        var s = Sin(halfAngle);
        var qW = Cos(halfAngle);
        var qX = x * s;
        var qY = y * s;
        var qZ = z * s;

        return new Quaternion(qX, qY, qZ, qW);
    }

    [Obsolete("Not implemented.")]
    public static EulerAngles ToEulerAngles(this AxisAngle axisAngle)
    {
        throw new NotImplementedException();
    }

    public static AxisAngle Normalize(this AxisAngle axisAngle)
    {
        return new AxisAngle(axisAngle.Axis.Normalize(), axisAngle.Angle);
    }

    /// <summary>
    /// For debug only
    /// </summary>
    public static bool IsUnit(this AxisAngle axisAngle)
    {
        return axisAngle.Axis.IsUnit();
    }
}
