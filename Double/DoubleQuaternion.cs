namespace Rotation3D.Double;

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

    public DoubleQuaternion Negate() => new(-X, -Y, -Z, -W);

    public double UnitDiff() => Abs(1.0 - Length());

    public double Diff(DoubleQuaternion q2)
    {
        var diffX1 = Abs(X - q2.X);
        var diffY1 = Abs(Y - q2.Y);
        var diffZ1 = Abs(Z - q2.Z);
        var diffW1 = Abs(W - q2.W);

        var diffX2 = Abs(X + q2.X);
        var diffY2 = Abs(Y + q2.Y);
        var diffZ2 = Abs(Z + q2.Z);
        var diffW2 = Abs(W + q2.W);

        var diffSum1 = diffX1 + diffY1 + diffZ1 + diffW1;
        var diffSum2 = diffX2 + diffY2 + diffZ2 + diffW2;
        var diffSum = Min(diffSum1, diffSum2);
        return diffSum;
    }

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
        var wz = Z * W;
        var xz = Z * X;
        var wy = Y * W;
        var yz = Y * Z;
        var wx = X * W;

        var matrix = DoubleMatrix4x4.Identity;

        matrix.M11 = 1.0 - 2.0 * (yy + zz);
        matrix.M12 = 2.0 * (xy + wz);
        matrix.M13 = 2.0 * (xz - wy);

        matrix.M21 = 2.0 * (xy - wz);
        matrix.M22 = 1.0 - 2.0 * (zz + xx);
        matrix.M23 = 2.0 * (yz + wx);

        matrix.M31 = 2.0 * (xz + wy);
        matrix.M32 = 2.0 * (yz - wx);
        matrix.M33 = 1.0 - 2.0 * (yy + xx);

        return matrix;
    }
}
