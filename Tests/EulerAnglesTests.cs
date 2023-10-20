namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

[TestClass]
public sealed class EulerAnglesTests : TestsBase
{
    [TestMethod]
    public void ToMatrix4x4()
    {
        const float maxDiffAllowed = 3.0174851E-06f;

        var maxDiff = 0f;
        var maxDiffEulerAngles = default(EulerAngles);
        var maxDiffMatrixSystem = default(Matrix4x4);
        var maxDiffMatrixCustom = default(Matrix4x4);

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var eulerAngles = GetRandomNormalEulerAngles();
            var matrixSystem = Matrix4x4.CreateFromYawPitchRoll(eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);
            var matrixCustom = eulerAngles.ToMatrix4x4();
            var diff = CalcSumDiff(matrixSystem, matrixCustom);

            if (maxDiff < diff)
            {
                maxDiff = diff;
                maxDiffEulerAngles = eulerAngles;
                maxDiffMatrixSystem = matrixSystem;
                maxDiffMatrixCustom = matrixCustom;
            }
        }

        Console.WriteLine($"Max diff: {maxDiff}");

        if (maxDiff > 0)
        {
            Console.WriteLine($"EulerAngles: {maxDiffEulerAngles}");
            Console.WriteLine($"Matrix4x4 system: {maxDiffMatrixSystem}");
            Console.WriteLine($"Matrix4x4 custom: {maxDiffMatrixCustom}");

            if (maxDiff > maxDiffAllowed)
                Assert.Fail();
        }
    }

    [TestMethod]
    public void ToQuaternion()
    {
        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var eulerAngles = GetRandomNormalEulerAngles();
            var quaternionSystem = Quaternion.CreateFromYawPitchRoll(eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);
            var quaternionCustom = eulerAngles.ToQuaternion();
            Assert.AreEqual(quaternionSystem, quaternionCustom);
        }

        Console.WriteLine("Max diff: 0");
    }
}
