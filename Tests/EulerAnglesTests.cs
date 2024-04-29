namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Double;
using System.Numerics;

[TestClass]
public sealed class EulerAnglesTests : TestsBase
{
    [TestMethod]
    public void UnitToMatrix()
    {
        var result = TestAB(
            createSrc: Randomizer.CreateUnitEulerAngles,
            compare: (exact, test) => exact.Diff(test.ToDouble()),
            calcExact: e => e.ToDouble().UnitToMatrix(),
            calcTestA: e => Matrix4x4.CreateFromYawPitchRoll(e.Yaw, e.Pitch, e.Roll),
            calcTestB: e => e.UnitToMatrix());

        Assert.IsTrue(result.AvgDiffB < result.AvgDiffA);
        Assert.IsTrue(result.MaxDiffB < result.MaxDiffA);
        Assert.IsTrue(result.MaxDiffB <= 5.614007e-7f);
    }

    [TestMethod]
    public void UnitToQuaternion()
    {
        var result = TestAB(
            createSrc: Randomizer.CreateUnitEulerAngles,
            compare: (exact, test) => exact.Diff(test.ToDouble()),
            calcExact: e => e.ToDouble().UnitToQuaternion(),
            calcTestA: e => Quaternion.CreateFromYawPitchRoll(e.Yaw, e.Pitch, e.Roll),
            calcTestB: e => e.UnitToQuaternion());

        Assert.IsTrue(result.AvgDiffB < result.AvgDiffA);
        //Assert.IsTrue(result.MaxDiffB <= result.MaxDiffA); // Sometimes possible
        Assert.IsTrue(result.MaxDiffB <= 3.8816358e-7f);
    }
}
