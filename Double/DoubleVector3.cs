namespace Rotation3D.Double;

using System.Numerics;
using static Math;

public readonly struct DoubleVector3
{
    public static DoubleVector3 Zero { get; } = new(x: 0.0, y: 0.0, z: 0.0);

    public readonly double X;
    public readonly double Y;
    public readonly double Z;

    public DoubleVector3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double Length() => Sqrt(X * X + Y * Y + Z * Z);

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Vector3.Normalize(Vector3)"/>
    /// </summary>
    public DoubleVector3 Normalize()
    {
        var sqLen = X * X + Y * Y + Z * Z;

        if (sqLen == 0)
            return Zero;

        var invLen = 1.0 / Sqrt(sqLen);
        return new DoubleVector3(X * invLen, Y * invLen, Z * invLen);
    }

    public double UnitDiff() => Abs(1.0 - Length());
}
