﻿namespace Rotation3D.Double;

using System.Numerics;
using static Math;

public readonly struct DoubleQuaternion
{
    public static DoubleQuaternion Zero { get; } = new(x: 0.0, y: 0.0, z: 0.0, w: 0.0);

    public readonly double X;
    public readonly double Y;
    public readonly double Z;
    public readonly double W;

    public DoubleQuaternion(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public double Length() => Sqrt(X * X + Y * Y + Z * Z + W * W);

    public double UnitDiff() => Abs(1.0 - Length());

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Quaternion.Normalize(Quaternion)"/>
    /// </summary>
    public DoubleQuaternion Normalize()
    {
        var invLen = 1.0 / Sqrt(X * X + Y * Y + Z * Z + W * W);
        return new DoubleQuaternion(X * invLen, Y * invLen, Z * invLen, W * invLen);
    }

    [Obsolete("Need to prove")]
    public DoubleAxisAngle UnitToAxisAngle()
    {
        //todo assert unit
        throw new NotImplementedException();
    }

    [Obsolete("Need to prove")]
    public DoubleEulerAngles UnitToEulerAngles()
    {
        //todo assert unit
        throw new NotImplementedException();
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Matrix4x4.CreateFromQuaternion(Quaternion)"/>
    /// </summary>
    public DoubleMatrix4x4 UnitToMatrix()
    {
        //todo assert unit
        var xx = X * X;
        var yy = Y * Y;
        var zz = Z * Z;

        var xy = X * Y;
        var xz = X * Z;
        var xw = X * W;
        var yz = Y * Z;
        var yw = Y * W;
        var zw = Z * W;

        var matrix = DoubleMatrix4x4.Identity;

        matrix.M11 = 1.0 - (yy + zz) * 2.0;
        matrix.M12 = (xy + zw) * 2.0;
        matrix.M13 = (xz - yw) * 2.0;

        matrix.M21 = (xy - zw) * 2.0;
        matrix.M22 = 1.0 - (xx + zz) * 2.0;
        matrix.M23 = (xw + yz) * 2.0;

        matrix.M31 = (xz + yw) * 2.0;
        matrix.M32 = (yz - xw) * 2.0;
        matrix.M33 = 1.0 - (xx + yy) * 2.0;

        return matrix;
    }
}
