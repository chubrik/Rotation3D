namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

[TestClass]
public sealed class AxisAngleTests : TestsBase
{
    [TestMethod]
    public void ToEulerAngles()
    {
        // Truth: AxisAngle             =>             Quaternion (system)
        // Test:  AxisAngle => EulerAngles (custom) => Quaternion (system)

        const float maxDiffAllowed = 0.0008648336f;

        var maxDiff = 0f;
        var maxDiffAxisAngle = default(AxisAngle);
        var maxDiffEulerAngles = default(EulerAngles);
        var maxDiffQuaternionDirect = default(Quaternion);
        var maxDiffQuaternionResult = default(Quaternion);

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var axisAngle = GetRandomNormalAxisAngle();
            var eulerAngles = axisAngle.ToEulerAngles();
            Assert.IsTrue(eulerAngles.IsNormal());
            var quaternionDirect = Quaternion.CreateFromAxisAngle(axisAngle.Axis, axisAngle.Angle);
            var quaternionResult = Quaternion.CreateFromYawPitchRoll(eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);
            var diff = CalcSumDiff(quaternionDirect, quaternionResult);

            if (maxDiff < diff)
            {
                maxDiff = diff;
                maxDiffAxisAngle = axisAngle;
                maxDiffEulerAngles = eulerAngles;
                maxDiffQuaternionDirect = quaternionDirect;
                maxDiffQuaternionResult = quaternionResult;
            }
        }

        Console.WriteLine($"Max diff: {maxDiff}");

        if (maxDiff > 0)
        {
            Console.WriteLine($"AxisAngle: {maxDiffAxisAngle}");
            Console.WriteLine($"EulerAngles: {maxDiffEulerAngles}");
            Console.WriteLine($"Quaternion direct: {maxDiffQuaternionDirect}");
            Console.WriteLine($"Quaternion result: {maxDiffQuaternionResult}");

            if (maxDiff > maxDiffAllowed)
                Assert.Fail();
        }
    }

    [TestMethod]
    public void ToMatrix4x4()
    {
        // Truth: AxisAngle => Matrix4x4 (system)
        // Test:  AxisAngle => Matrix4x4 (custom)

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var axisAngle = GetRandomNormalAxisAngle();
            var matrixSystem = Matrix4x4.CreateFromAxisAngle(axisAngle.Axis, axisAngle.Angle);
            var matrixCustom = axisAngle.ToMatrix4x4();
            Assert.IsTrue(matrixCustom.IsNormal());
            Assert.AreEqual(matrixSystem, matrixCustom);
        }

        Console.WriteLine("Max diff: 0");
    }

    [TestMethod]
    public void ToQuaternion()
    {
        // Truth: AxisAngle => Quaternion (system)
        // Test:  AxisAngle => Quaternion (custom)

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var axisAngle = GetRandomNormalAxisAngle();
            var quaternionSystem = Quaternion.CreateFromAxisAngle(axisAngle.Axis, axisAngle.Angle);
            var quaternionCustom = axisAngle.ToQuaternion();
            Assert.IsTrue(quaternionCustom.IsNormal());
            Assert.AreEqual(quaternionSystem, quaternionCustom);
        }

        Console.WriteLine("Max diff: 0");
    }
}
