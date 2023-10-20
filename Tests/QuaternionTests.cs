namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

[TestClass]
public sealed class QuaternionTests : TestsBase
{
    [TestMethod]
    public void ToEulerAngles()
    {
        const float maxDiffAllowed = 0.0008600652f;

        var maxDiff = 0f;
        var maxDiffEulerAngles = default(EulerAngles);
        var maxDiffQuaternionSource = default(Quaternion);
        var maxDiffQuaternionRevert = default(Quaternion);

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var quaternionSource = GetRandomNormalQuaternion();
            var eulerAngles = quaternionSource.ToEulerAngles();
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
    public void ToMatrix4x4()
    {
        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var quaternion = GetRandomNormalQuaternion();
            var matrixSystem = Matrix4x4.CreateFromQuaternion(quaternion);
            var matrixCustom = quaternion.ToMatrix4x4();
            Assert.AreEqual(matrixSystem, matrixCustom);
        }

        Console.WriteLine("Max diff: 0");
    }
}
