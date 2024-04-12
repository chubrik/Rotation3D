﻿namespace Rotation3D.Double;

using static DoubleConstants;
using static Math;

public readonly struct DoubleAxisAngle
{
    public static DoubleAxisAngle Identity { get; } = new(x: 1.0, y: 0.0, z: 0.0, angle: 0.0);

    public readonly DoubleVector3 Axis;
    public readonly double Angle;

    public readonly double X => Axis.X;
    public readonly double Y => Axis.Y;
    public readonly double Z => Axis.Z;
    public readonly double AngleDegrees => Angle * RAD_TO_DEG;

    public DoubleAxisAngle(AxisAngle axisAngle)
        : this(axisAngle.Axis.ToDouble(), axisAngle.Angle)
    { }

    public DoubleAxisAngle(DoubleVector3 axis, double angle)
    {
        Axis = axis;
        Angle = angle;
    }

    public DoubleAxisAngle(double x, double y, double z, double angle)
        : this(new DoubleVector3(x, y, z), angle)
    { }

    public static DoubleAxisAngle FromDegrees(DoubleVector3 axis, double angle)
    {
        return new DoubleAxisAngle(axis, angle * DEG_TO_RAD);
    }

    public static DoubleAxisAngle FromDegrees(double x, double y, double z, double angle)
    {
        return new DoubleAxisAngle(x, y, z, angle * DEG_TO_RAD);
    }

    public DoubleEulerAngles ToEulerAngles()
    {
        throw new NotImplementedException();
    }

    public DoubleMatrix4x4 UnitToMatrix()
    {
        var sa = Sin(Angle);
        var ca = Cos(Angle);
        var xx = X * X;
        var yy = Y * Y;
        var zz = Z * Z;
        var xy = X * Y;
        var xz = X * Z;
        var yz = Y * Z;

        var xy_caxy = xy - xy * ca;
        var xz_caxz = xz - xz * ca;
        var yz_cayz = yz - yz * ca;
        var xsa = X * sa;
        var ysa = Y * sa;
        var zsa = Z * sa;

        var matrix = DoubleMatrix4x4.Identity;

        matrix.M11 = xx + ca * (1.0 - xx);
        matrix.M12 = xy_caxy + zsa;
        matrix.M13 = xz_caxz - ysa;

        matrix.M21 = xy_caxy - zsa;
        matrix.M22 = yy + ca * (1.0 - yy);
        matrix.M23 = yz_cayz + xsa;

        matrix.M31 = xz_caxz + ysa;
        matrix.M32 = yz_cayz - xsa;
        matrix.M33 = zz + ca * (1.0 - zz);

        return matrix;
    }

    public DoubleQuaternion UnitToQuaternion()
    {
        var halfAngle = Angle * 0.5;
        var sa = Sin(halfAngle);

        var qX = X * sa;
        var qY = Y * sa;
        var qZ = Z * sa;
        var qW = Cos(halfAngle);

        return new DoubleQuaternion(qX, qY, qZ, qW);
    }
}