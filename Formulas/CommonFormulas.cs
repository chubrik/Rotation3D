namespace Rotation3D;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Tests;
using System.Numerics;
using static MathFConstants;

public static class CommonFormulas
{
    public static Vector3 Normalize(this Vector3 vector)
    {
        // Reference: Vector3.Normalize(vector);

        var (x, y, z) = (vector.X, vector.Y, vector.Z);

        var invNorm = 1f / MathF.Sqrt(x * x + y * y + z * z);
        return new Vector3(x * invNorm, y * invNorm, z * invNorm);
    }

    public static float NormalizeAngle(float angle)
    {
        var normAngle = angle < MINUS_PI ? angle + TWO_PI
                      : angle > MathF.PI ? angle - TWO_PI
                      : angle;

        Assert.IsTrue(TestAsserts.IsNormalAngle(normAngle));
        return normAngle;
    }
}
