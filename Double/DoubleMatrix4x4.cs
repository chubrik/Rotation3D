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

    public DoubleMatrix4x4(Matrix4x4 matrix)
        : this(matrix.M11, matrix.M12, matrix.M13, matrix.M14,
               matrix.M21, matrix.M22, matrix.M23, matrix.M24,
               matrix.M31, matrix.M32, matrix.M33, matrix.M34,
               matrix.M41, matrix.M42, matrix.M43, matrix.M44)
    { }

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

    public readonly DoubleMatrix4x4 Normalize()
    {
        var normRight = Right.Normalize();
        var normUp = Up.Normalize();
        var normForward = Forward.Normalize();

        var matrix = Identity;

        matrix.M11 = normRight.X;
        matrix.M12 = normRight.Y;
        matrix.M13 = normRight.Z;

        matrix.M21 = normUp.X;
        matrix.M22 = normUp.Y;
        matrix.M23 = normUp.Z;

        matrix.M31 = normForward.X;
        matrix.M32 = normForward.Y;
        matrix.M33 = normForward.Z;

        return matrix;
    }

    public readonly double DiffUnit()
    {
        var rightDiff = Abs(1.0 - Right.Length());
        var upDiff = Abs(1.0 - Up.Length());
        var forwardDiff = Abs(1.0 - Forward.Length());
        return rightDiff + upDiff + forwardDiff;
    }

    public readonly DoubleAxisAngle UnitToAxisAngle()
    {
        throw new NotImplementedException();
    }

    public readonly DoubleEulerAngles UnitToEulerAngles()
    {
        throw new NotImplementedException();
    }

    public readonly DoubleEulerAngles ScaledToEulerAngles()
    {
        throw new NotImplementedException();
    }

    public readonly DoubleQuaternion UnitToQuaternion()
    {
        var trace = M11 + M22 + M33;
        double x, y, z, w;

        if (trace > 0.0)
        {
            w = Sqrt(1.0 + trace) * 0.5;
            var invS = 0.25 / w;
            x = (M23 - M32) * invS;
            y = (M31 - M13) * invS;
            z = (M12 - M21) * invS;
        }
        else if (M11 >= M22 && M11 >= M33)
        {
            x = Sqrt(1.0 + M11 - M22 - M33) * 0.5;
            var invS = 0.25 / x;
            y = (M12 + M21) * invS;
            z = (M13 + M31) * invS;
            w = (M23 - M32) * invS;
        }
        else if (M22 > M33)
        {
            y = Sqrt(1.0 + M22 - M11 - M33) * 0.5;
            var invS = 0.25 / y;
            x = (M21 + M12) * invS;
            z = (M32 + M23) * invS;
            w = (M31 - M13) * invS;
        }
        else
        {
            z = Sqrt(1.0 + M33 - M11 - M22) * 0.5;
            var invS = 0.25 / z;
            x = (M31 + M13) * invS;
            y = (M32 + M23) * invS;
            w = (M12 - M21) * invS;
        }

        return new DoubleQuaternion(x, y, z, w);
    }
}
