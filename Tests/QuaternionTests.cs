namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

[TestClass]
public sealed class QuaternionTests : TestsBase
{
    //[TestMethod]
    public void ToAxisAngle_NotPassed()
    {
        // Truth: Quaternion
        // Test:  Quaternion => AxisAngle (custom) => Quaternion (system)

        const float maxDiffAllowed = 1.417473E-06f;

        var maxDiff = 0f;
        var maxDiffAxisAngle = default(AxisAngle);
        var maxDiffQuaternionSource = default(Quaternion);
        var maxDiffQuaternionRevert = default(Quaternion);

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var quaternionSource = GetRandomNormalQuaternion();
            var axisAngle = quaternionSource.ToAxisAngle();
            Assert.IsTrue(axisAngle.IsNormal());
            var quaternionRevert = Quaternion.CreateFromAxisAngle(axisAngle.Axis, axisAngle.Angle);
            var diff = CalcSumDiff(quaternionSource, quaternionRevert);

            if (maxDiff < diff)
            {
                maxDiff = diff;
                maxDiffAxisAngle = axisAngle;
                maxDiffQuaternionSource = quaternionSource;
                maxDiffQuaternionRevert = quaternionRevert;
            }
        }

        Console.WriteLine($"Max diff: {maxDiff}");

        if (maxDiff > 0)
        {
            Console.WriteLine($"AxisAngle: {maxDiffAxisAngle}");
            Console.WriteLine($"Quaternion source: {maxDiffQuaternionSource}");
            Console.WriteLine($"Quaternion revert: {maxDiffQuaternionRevert}");

            if (maxDiff > maxDiffAllowed)
                Assert.Fail();
        }
    }

    [TestMethod]
    public void ToEulerAngles()
    {
        // Truth: Quaternion
        // Test:  Quaternion => EulerAngles (custom) => Quaternion (system)

        const float maxDiffAllowed = 0.0008686781f;

        var maxDiff = 0f;
        var maxDiffEulerAngles = default(EulerAngles);
        var maxDiffQuaternionSource = default(Quaternion);
        var maxDiffQuaternionRevert = default(Quaternion);

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var quaternionSource = GetRandomNormalQuaternion();
            var eulerAngles = quaternionSource.ToEulerAngles();
            Assert.IsTrue(eulerAngles.IsNormal());
            var quaternionRevert = Quaternion.CreateFromYawPitchRoll(eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);
            var diff = CalcSumDiff(quaternionSource, quaternionRevert);

            if (maxDiff < diff)
            {
                maxDiff = diff;
                maxDiffEulerAngles = eulerAngles;
                maxDiffQuaternionSource = quaternionSource;
                maxDiffQuaternionRevert = quaternionRevert;
            }
        }

        Console.WriteLine($"Max diff: {maxDiff}");

        if (maxDiff > 0)
        {
            Console.WriteLine($"EulerAngles: {maxDiffEulerAngles}");
            Console.WriteLine($"Quaternion source: {maxDiffQuaternionSource}");
            Console.WriteLine($"Quaternion revert: {maxDiffQuaternionRevert}");

            if (maxDiff > maxDiffAllowed)
                Assert.Fail();
        }
    }

    [TestMethod]
    public void ToEulerAngles_NonUnit()
    {
        // Truth: Quaternion_NonUnit => Quaternion (system)
        // Test:  Quaternion_NonUnit => EulerAngles (custom) => Quaternion (system)

        const float maxDiffAllowed = 0.0009267777f;

        var maxDiff = 0f;
        var maxDiffEulerAngles = default(EulerAngles);
        var maxDiffEulerAnglesFromNormal = default(EulerAngles);
        var maxDiffQuaternionSource = default(Quaternion);
        var maxDiffQuaternionNormal = default(Quaternion);
        var maxDiffQuaternionRevert = default(Quaternion);

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var quaternionSource = GetRandomNonUnitQuaternion();
            var quaternionNormal = Quaternion.Normalize(quaternionSource);
            var eulerAngles = quaternionSource.ToEulerAngles_NonUnitQuaternion();
            var eulerAnglesFromNormal = quaternionNormal.ToEulerAngles();
            Assert.IsTrue(quaternionNormal.IsNormal());
            Assert.IsTrue(eulerAngles.IsNormal());
            Assert.IsTrue(eulerAnglesFromNormal.IsNormal());
            var quaternionRevert = Quaternion.CreateFromYawPitchRoll(eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);
            var diff = CalcSumDiff(quaternionNormal, quaternionRevert);

            if (maxDiff < diff)
            {
                maxDiff = diff;
                maxDiffEulerAngles = eulerAngles;
                maxDiffEulerAnglesFromNormal = eulerAnglesFromNormal;
                maxDiffQuaternionSource = quaternionSource;
                maxDiffQuaternionNormal = quaternionNormal;
                maxDiffQuaternionRevert = quaternionRevert;
            }
        }

        Console.WriteLine($"Max diff: {maxDiff}");

        if (maxDiff > 0)
        {
            Console.WriteLine($"EulerAngles:             {maxDiffEulerAngles}");
            Console.WriteLine($"EulerAngles from normal: {maxDiffEulerAnglesFromNormal}");
            Console.WriteLine($"Quaternion source: {maxDiffQuaternionSource}");
            Console.WriteLine($"Quaternion normal: {maxDiffQuaternionNormal}");
            Console.WriteLine($"Quaternion revert: {maxDiffQuaternionRevert}");

            if (maxDiff > maxDiffAllowed)
                Assert.Fail();
        }
    }

    [TestMethod]
    public void ToMatrix4x4()
    {
        // Truth: Quaternion => Matrix4x4 (system)
        // Test:  Quaternion => Matrix4x4 (custom)

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var quaternion = GetRandomNormalQuaternion();
            var matrixSystem = Matrix4x4.CreateFromQuaternion(quaternion);
            var matrixCustom = quaternion.ToMatrix4x4();
            Assert.IsTrue(matrixCustom.IsNormal());
            Assert.AreEqual(matrixSystem, matrixCustom);
        }

        Console.WriteLine("Max diff: 0");
    }

    [TestMethod]
    public void ToMatrix4x4_NonUnit()
    {
        // Truth: Quaternion_NonUnit => Quaternion => Matrix4x4 (system)
        // Test:  Quaternion_NonUnit => Matrix4x4 (custom)

        const float maxDiffAllowed = 2.65240669E-06f;

        var maxDiff = 0f;
        var maxDiffQuaternionSource = default(Quaternion);
        var maxDiffQuaternionNormal = default(Quaternion);
        var maxDiffMatrixSystem = default(Matrix4x4);
        var maxDiffMatrixCustom = default(Matrix4x4);

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var quaternionSource = GetRandomNonUnitQuaternion();
            var quaternionNormal = Quaternion.Normalize(quaternionSource);
            var matrixSystem = Matrix4x4.CreateFromQuaternion(quaternionNormal);
            var matrixCustom = quaternionSource.ToMatrix4x4_NonUnitQuaternion();
            Assert.IsTrue(matrixCustom.IsNormal());

            var diff = CalcSumDiff(matrixSystem, matrixCustom);

            if (maxDiff < diff)
            {
                maxDiff = diff;
                maxDiffQuaternionSource = quaternionSource;
                maxDiffQuaternionNormal = quaternionNormal;
                maxDiffMatrixSystem = matrixSystem;
                maxDiffMatrixCustom = matrixCustom;
            }
        }

        Console.WriteLine($"Max diff: {maxDiff}");

        if (maxDiff > 0)
        {
            Console.WriteLine($"Quaternion source: {maxDiffQuaternionSource}");
            Console.WriteLine($"Quaternion normal: {maxDiffQuaternionNormal}");
            Console.WriteLine($"Matrix4x4 system: {maxDiffMatrixSystem}");
            Console.WriteLine($"Matrix4x4 custom: {maxDiffMatrixCustom}");

            if (maxDiff > maxDiffAllowed)
                Assert.Fail();
        }
    }
}
