namespace Rotation3D.Double;

using System.Numerics;
using static DoubleConstants;
using static Math;

public readonly struct DoubleAxisAngle
{
    public static DoubleAxisAngle Zero { get; } = new(axis: DoubleVector3.Zero, angle: 0.0);

    public readonly DoubleVector3 Axis;
    public readonly double Angle;

    public readonly double X => Axis.X;
    public readonly double Y => Axis.Y;
    public readonly double Z => Axis.Z;
    public readonly double AngleDegrees => Angle * RAD_TO_DEG;

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

    [Obsolete("Need to prove")]
    public DoubleAxisAngle NormalizeHard()
    {
        var axis = Axis.Normalize();
        var angle = Angle.NormalizeAngleHard();
        return new DoubleAxisAngle(axis, angle);
    }

    [Obsolete("Need to prove")]
    public DoubleEulerAngles UnitToEulerAngles()
    {
        //todo assert unit
        throw new NotImplementedException();
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Matrix4x4.CreateFromAxisAngle(Vector3, float)"/>
    /// </summary>
    public DoubleMatrix4x4 UnitToMatrix()
    {
        //todo assert unit
        var sa = Sin(Angle);
        var ca = Cos(Angle);
        var xx = X * X;
        var yy = Y * Y;
        var zz = Z * Z;
        var xy = X * Y;
        var xz = X * Z;
        var yz = Y * Z;

        var result = DoubleMatrix4x4.Identity;

        result.M11 = xx + ca * (1.0f - xx);
        result.M12 = xy - ca * xy + sa * Z;
        result.M13 = xz - ca * xz - sa * Y;

        result.M21 = xy - ca * xy - sa * Z;
        result.M22 = yy + ca * (1.0f - yy);
        result.M23 = yz - ca * yz + sa * X;

        result.M31 = xz - ca * xz + sa * Y;
        result.M32 = yz - ca * yz - sa * X;
        result.M33 = zz + ca * (1.0f - zz);

        return result;
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Quaternion.CreateFromAxisAngle(Vector3, float)"/>
    /// </summary>
    public DoubleQuaternion UnitToQuaternion()
    {
        //todo assert unit
        var halfAngle = Angle * 0.5;
        var s = Sin(halfAngle);
        var c = Cos(halfAngle);

        var qX = X * s;
        var qY = Y * s;
        var qZ = Z * s;
        var qW = c;

        return new DoubleQuaternion(qX, qY, qZ, qW);
    }
}
