namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

[TestClass]
public sealed class AxisAngleTests : TestsBase
{
    [TestMethod]
    public void ToMatrix4x4()
    {
        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var axisAngle = GetRandomNormalAxisAngle();
            var matrixSystem = Matrix4x4.CreateFromAxisAngle(axisAngle.Axis, axisAngle.Angle);
            var matrixCustom = axisAngle.ToMatrix4x4();
            Assert.AreEqual(matrixSystem, matrixCustom);
        }

        Console.WriteLine("Max diff: 0");
    }

    [TestMethod]
    public void ToQuaternion()
    {
        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var axisAngle = GetRandomNormalAxisAngle();
            var quaternionSystem = Quaternion.CreateFromAxisAngle(axisAngle.Axis, axisAngle.Angle);
            var quaternionCustom = axisAngle.ToQuaternion();
            Assert.AreEqual(quaternionSystem, quaternionCustom);
        }

        Console.WriteLine("Max diff: 0");
    }
}
