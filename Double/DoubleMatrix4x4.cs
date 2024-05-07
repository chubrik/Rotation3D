namespace Rotation3D.Double;

using System.Numerics;
using static Math;

public struct DoubleMatrix4x4
{
    public static DoubleMatrix4x4 Identity { get; } = new(1.0, 0.0, 0.0, 0.0,
                                                          0.0, 1.0, 0.0, 0.0,
                                                          0.0, 0.0, 1.0, 0.0,
                                                          0.0, 0.0, 0.0, 1.0);
    public double M11;
    public double M12;
    public double M13;
    public double M14;

    public double M21;
    public double M22;
    public double M23;
    public double M24;

    public double M31;
    public double M32;
    public double M33;
    public double M34;

    public double M41;
    public double M42;
    public double M43;
    public double M44;

    public DoubleMatrix4x4(double m11, double m12, double m13, double m14,
                           double m21, double m22, double m23, double m24,
                           double m31, double m32, double m33, double m34,
                           double m41, double m42, double m43, double m44)
    {
        M11 = m11;
        M12 = m12;
        M13 = m13;
        M14 = m14;

        M21 = m21;
        M22 = m22;
        M23 = m23;
        M24 = m24;

        M31 = m31;
        M32 = m32;
        M33 = m33;
        M34 = m34;

        M41 = m41;
        M42 = m42;
        M43 = m43;
        M44 = m44;
    }

    public readonly DoubleVector3 Right => new(M11, M12, M13);

    public readonly DoubleVector3 Up => new(M21, M22, M23);

    public readonly DoubleVector3 Forward => new(M31, M32, M33);

    public readonly double UnitDiff()
    {
        var rightDiff = Abs(1.0 - Right.Length());
        var upDiff = Abs(1.0 - Up.Length());
        var forwardDiff = Abs(1.0 - Forward.Length());
        return rightDiff + upDiff + forwardDiff;
    }

    public readonly double Diff(DoubleMatrix4x4 other)
    {
        var diff1 = Abs(M11 - other.M11);
        var diff2 = Abs(M12 - other.M12);
        var diff3 = Abs(M13 - other.M13);
        var diff4 = Abs(M21 - other.M21);
        var diff5 = Abs(M22 - other.M22);
        var diff6 = Abs(M23 - other.M23);
        var diff7 = Abs(M31 - other.M31);
        var diff8 = Abs(M32 - other.M32);
        var diff9 = Abs(M33 - other.M33);

        var diffSum = diff1 + diff2 + diff3 + diff4 + diff5 + diff6 + diff7 + diff8 + diff9;
        return diffSum;
    }

    [Obsolete("Need to prove")]
    public readonly DoubleMatrix4x4 Normalize()
    {
        var sqLenRight = M11 * M11 + M12 * M12 + M13 * M13;
        var sqLenUp = M21 * M21 + M22 * M22 + M23 * M23;
        var sqLenForward = M31 * M31 + M32 * M32 + M33 * M33;

        var invLenRight = sqLenRight > 0.0 ? 1.0 / Sqrt(sqLenRight) : 0.0;
        var invLenUp = sqLenUp > 0.0 ? 1.0 / Sqrt(sqLenUp) : 0.0;
        var invLenForward = sqLenForward > 0.0 ? 1.0 / Sqrt(sqLenForward) : 0.0;

        var matrix = Identity;

        matrix.M11 = M11 * invLenRight;
        matrix.M12 = M12 * invLenRight;
        matrix.M13 = M13 * invLenRight;

        matrix.M21 = M21 * invLenUp;
        matrix.M22 = M22 * invLenUp;
        matrix.M23 = M23 * invLenUp;

        matrix.M31 = M31 * invLenForward;
        matrix.M32 = M32 * invLenForward;
        matrix.M33 = M33 * invLenForward;

        return matrix;
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Quaternion.CreateFromRotationMatrix(Matrix4x4)"/>
    /// </summary>
    public readonly DoubleQuaternion UnitToQuaternion()
    {
        //todo assert unit
        var trace = M11 + M22 + M33;
        double x, y, z, w;

        if (trace > 0.0)
        {
            var s = Sqrt(trace + 1.0);
            w = s * 0.5;
            s = 0.5 / s;
            x = (M23 - M32) * s;
            y = (M31 - M13) * s;
            z = (M12 - M21) * s;
        }
        else if (M11 >= M22 && M11 >= M33)
        {
            var s = Sqrt(1.0 + M11 - M22 - M33);
            var invS = 0.5 / s;
            x = 0.5 * s;
            y = (M12 + M21) * invS;
            z = (M13 + M31) * invS;
            w = (M23 - M32) * invS;
        }
        else if (M22 > M33)
        {
            var s = Sqrt(1.0 + M22 - M11 - M33);
            var invS = 0.5 / s;
            x = (M21 + M12) * invS;
            y = 0.5 * s;
            z = (M32 + M23) * invS;
            w = (M31 - M13) * invS;
        }
        else
        {
            var s = Sqrt(1.0 + M33 - M11 - M22);
            var invS = 0.5 / s;
            x = (M31 + M13) * invS;
            y = (M32 + M23) * invS;
            z = 0.5 * s;
            w = (M12 - M21) * invS;
        }

        return new DoubleQuaternion(x, y, z, w);
    }
}
