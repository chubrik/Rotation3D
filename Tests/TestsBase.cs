namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

public abstract class TestsBase
{
    protected const int _iterationCount = 10_000_000;

    private const double DOUBLE_TWO_PI = Math.PI * 2.0;
    private const double DOUBLE_HALF_PI = Math.PI / 2.0;
    private static readonly Random _random = new();

    protected static AxisAngle GetRandomNormalAxisAngle()
    {
        var rawX = _random.NextDouble() * 2.0 - 1.0;
        var rawY = _random.NextDouble() * 2.0 - 1.0;
        var rawZ = _random.NextDouble() * 2.0 - 1.0;

        var invNorm = 1.0 / Math.Sqrt(rawX * rawX + rawY * rawY + rawZ * rawZ);
        var normX = (float)(rawX * invNorm);
        var normY = (float)(rawY * invNorm);
        var normZ = (float)(rawZ * invNorm);

        var angle = (float)(_random.NextDouble() * DOUBLE_TWO_PI - Math.PI);
        var axisAngle = new AxisAngle(normX, normY, normZ, angle);
        Assert.IsTrue(axisAngle.IsNormal());
        return axisAngle;
    }

    protected static EulerAngles GetRandomNormalEulerAngles()
    {
        var yaw = (float)(_random.NextDouble() * DOUBLE_TWO_PI - Math.PI);
        var pitch = (float)(_random.NextDouble() * Math.PI - DOUBLE_HALF_PI);
        var roll = (float)(_random.NextDouble() * DOUBLE_TWO_PI - Math.PI);

        var eulerAngles = new EulerAngles(yaw, pitch, roll);
        Assert.IsTrue(eulerAngles.IsNormal());
        return eulerAngles;
    }

    protected static Matrix4x4 GetRandomNormalMatrix4x4()
    {
        // Creating a normal quaternion and converting it to a matrix.
        // The Convertion implementaion is similar to: Matrix4x4.CreateFromQuaternion(Quaternion quaternion)
        // but on doubles to prevent float quantization violations.

        double rawX = _random.NextDouble() * 2.0 - 1.0;
        double rawY = _random.NextDouble() * 2.0 - 1.0;
        double rawZ = _random.NextDouble() * 2.0 - 1.0;
        double rawW = _random.NextDouble() * 2.0 - 1.0;

        double invNorm = 1.0 / Math.Sqrt(rawX * rawX + rawY * rawY + rawZ * rawZ + rawW * rawW);
        double quaternionX = rawX * invNorm;
        double quaternionY = rawY * invNorm;
        double quaternionZ = rawZ * invNorm;
        double quaternionW = rawW * invNorm;

        Matrix4x4 matrix = Matrix4x4.Identity;

        double xx = quaternionX * quaternionX;
        double yy = quaternionY * quaternionY;
        double zz = quaternionZ * quaternionZ;

        double xy = quaternionX * quaternionY;
        double wz = quaternionZ * quaternionW;
        double xz = quaternionZ * quaternionX;
        double wy = quaternionY * quaternionW;
        double yz = quaternionY * quaternionZ;
        double wx = quaternionX * quaternionW;

        matrix.M11 = (float)(1.0 - 2.0 * (yy + zz));
        matrix.M12 = (float)(2.0 * (xy + wz));
        matrix.M13 = (float)(2.0 * (xz - wy));

        matrix.M21 = (float)(2.0 * (xy - wz));
        matrix.M22 = (float)(1.0 - 2.0 * (zz + xx));
        matrix.M23 = (float)(2.0 * (yz + wx));

        matrix.M31 = (float)(2.0 * (xz + wy));
        matrix.M32 = (float)(2.0 * (yz - wx));
        matrix.M33 = (float)(1.0 - 2.0 * (yy + xx));

        Assert.IsTrue(matrix.IsNormal());
        return matrix;
    }

    protected static Quaternion GetRandomNormalQuaternion()
    {
        var rawX = _random.NextDouble() * 2.0 - 1.0;
        var rawY = _random.NextDouble() * 2.0 - 1.0;
        var rawZ = _random.NextDouble() * 2.0 - 1.0;
        var rawW = _random.NextDouble() * 2.0 - 1.0;

        var invNorm = 1 / Math.Sqrt(rawX * rawX + rawY * rawY + rawZ * rawZ + rawW * rawW);
        var normX = (float)(rawX * invNorm);
        var normY = (float)(rawY * invNorm);
        var normZ = (float)(rawZ * invNorm);
        var normW = (float)(rawW * invNorm);

        var quaternion = new Quaternion(normX, normY, normZ, normW);
        Assert.IsTrue(quaternion.IsNormal());
        return quaternion;
    }

    protected static float CalcSumDiff(Matrix4x4 m1, Matrix4x4 m2)
    {
        if (m1.Equals(m2))
            return 0f;

        var m11Diff = MathF.Abs(m1.M11 - m2.M11);
        var m12Diff = MathF.Abs(m1.M12 - m2.M12);
        var m13Diff = MathF.Abs(m1.M13 - m2.M13);
        var m21Diff = MathF.Abs(m1.M21 - m2.M21);
        var m22Diff = MathF.Abs(m1.M22 - m2.M22);
        var m23Diff = MathF.Abs(m1.M23 - m2.M23);
        var m31Diff = MathF.Abs(m1.M31 - m2.M31);
        var m32Diff = MathF.Abs(m1.M32 - m2.M32);
        var m33DIff = MathF.Abs(m1.M33 - m2.M33);

        var sumDiff = m11Diff + m12Diff + m13Diff + m21Diff + m22Diff + m23Diff + m31Diff + m32Diff + m33DIff;
        return sumDiff;
    }

    protected static float CalcSumDiff(Quaternion q1, Quaternion q2)
    {
        if (q1.Equals(q2))
            return 0f;

        var xDiff1 = MathF.Abs(q1.X - q2.X);
        var yDiff1 = MathF.Abs(q1.Y - q2.Y);
        var zDiff1 = MathF.Abs(q1.Z - q2.Z);
        var wDiff1 = MathF.Abs(q1.W - q2.W);

        var xDiff2 = MathF.Abs(q1.X + q2.X);
        var yDiff2 = MathF.Abs(q1.Y + q2.Y);
        var zDiff2 = MathF.Abs(q1.Z + q2.Z);
        var wDiff2 = MathF.Abs(q1.W + q2.W);

        var sumDiff1 = xDiff1 + yDiff1 + zDiff1 + wDiff1;
        var sumDiff2 = xDiff2 + yDiff2 + zDiff2 + wDiff2;
        return MathF.Min(sumDiff1, sumDiff2);
    }
}
