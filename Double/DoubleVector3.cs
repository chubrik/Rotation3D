namespace Rotation3D.Double;

using System.Numerics;
using static Math;

public readonly struct DoubleVector3
{
    public readonly double X;
    public readonly double Y;
    public readonly double Z;

    public DoubleVector3(Vector3 vector)
        : this(vector.X, vector.Y, vector.Z)
    { }

    public DoubleVector3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double Length() => Sqrt(X * X + Y * Y + Z * Z);

    public DoubleVector3 Normalize()
    {
        // Reference: Vector3.Normalize(vector3);

        var invLen = 1.0 / Length();
        return new DoubleVector3(X * invLen, Y * invLen, Z * invLen);
    }

    public double DiffUnit() => Abs(1.0 - Length());
}
