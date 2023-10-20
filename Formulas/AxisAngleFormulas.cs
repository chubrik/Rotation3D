namespace Rotation3D;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Tests;
using System.Numerics;
using static CommonFormulas;
using static MathF;

public static class AxisAngleFormulas
{
    public static AxisAngle Normalize(this AxisAngle axisAngle)
    {
        return new AxisAngle(
            axis: axisAngle.Axis.Normalize(),
            angle: NormalizeAngle(axisAngle.Angle));
    }

    public static Matrix4x4 ToMatrix4x4(this AxisAngle axisAngle)
    {
        // Reference: Matrix4x4.CreateFromAxisAngle(axisAngle.Axis, axisAngle.Angle);

        Assert.IsTrue(axisAngle.IsNormal());
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

        var matrix = Matrix4x4.Identity;

        matrix.M11 = xx + ca * (1f - xx);
        matrix.M12 = xy_caxy + saz;
        matrix.M13 = xz_caxz - say;

        matrix.M21 = xy_caxy - saz;
        matrix.M22 = yy + ca * (1f - yy);
        matrix.M23 = yz_cayz + sax;

        matrix.M31 = xz_caxz + say;
        matrix.M32 = yz_cayz - sax;
        matrix.M33 = zz + ca * (1f - zz);

        return matrix;
    }

    public static Quaternion ToQuaternion(this AxisAngle axisAngle)
    {
        // Reference: Quaternion.CreateFromAxisAngle(axisAngle.Axis, axisAngle.Angle);

        Assert.IsTrue(axisAngle.IsNormal());
        var (x, y, z, angle) = (axisAngle.Axis.X, axisAngle.Axis.Y, axisAngle.Axis.Z, axisAngle.Angle);

        var halfAngle = angle * 0.5f;
        var sa = Sin(halfAngle);

        var qX = x * sa;
        var qY = y * sa;
        var qZ = z * sa;
        var qW = Cos(halfAngle);

        return new Quaternion(qX, qY, qZ, qW);
    }
}
