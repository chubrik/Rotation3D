namespace Rotation3D.Double;

using System.Numerics;
using static Math;

public readonly struct DoubleQuaternion
{
    public readonly double X;
    public readonly double Y;
    public readonly double Z;
    public readonly double W;

    public DoubleQuaternion(Quaternion quaternion)
        : this(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W)
    { }

    public DoubleQuaternion(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public double Length() => Sqrt(X * X + Y * Y + Z * Z + W * W);

    public DoubleQuaternion Normalize()
    {
        // Reference: Quaternion.Normalize(quaternion);

        var invLen = 1.0 / Length();
        return new DoubleQuaternion(X * invLen, Y * invLen, Z * invLen, W * invLen);
    }
    public double DiffUnit() => Abs(1.0 - Length());

    public DoubleAxisAngle UnitToAxisAngle()
    {
        throw new NotImplementedException();
    }

    public DoubleEulerAngles UnitToEulerAngles()
    {
        throw new NotImplementedException();
    }

    public DoubleEulerAngles ScaledToEulerAngles()
    {
        throw new NotImplementedException();
    }

    public DoubleMatrix4x4 UnitToMatrix()
    {
        /// Reference: Matrix4x4.CreateFromQuaternion(quaternion);

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

    public DoubleMatrix4x4 ScaledToMatrix()
    {
        var xx = X * X;
        var yy = Y * Y;
        var zz = Z * Z;
        var ww = W * W;

        var xy = X * Y;
        var xz = X * Z;
        var xw = X * W;
        var yz = Y * Z;
        var yw = Y * W;
        var zw = Z * W;

        var invS = 1.0 / (xx + yy + zz + ww);
        var invSx2 = invS * 2.0;

        var matrix = DoubleMatrix4x4.Identity;

        matrix.M11 = (ww + xx - yy - zz) * invS;
        matrix.M12 = (xy + zw) * invSx2;
        matrix.M13 = (xz - yw) * invSx2;

        matrix.M21 = (xy - zw) * invSx2;
        matrix.M22 = (ww - xx + yy - zz) * invS;
        matrix.M23 = (xw + yz) * invSx2;

        matrix.M31 = (xz + yw) * invSx2;
        matrix.M32 = (yz - xw) * invSx2;
        matrix.M33 = (ww - xx - yy + zz) * invS;

        return matrix;
    }
}
