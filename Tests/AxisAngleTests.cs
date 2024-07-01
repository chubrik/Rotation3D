namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Double;
using System.Numerics;

[TestClass]
public sealed class AxisAngleTests : TestsBase
{
    [TestMethod]
    public void UnitToEulerAngles()
    {
        var result = Test(
            createSrc: Randomizer.CreateUnitAxisAngle,
            compare: (src, test) => src.ToDouble().UnitToQuaternion().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: a => a.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 0.0008810631f);
    }

    [TestMethod]
    public void UnitToMatrix()
    {
        var result = TestAB(
            createSrc: Randomizer.CreateUnitAxisAngle,
            compare: (exact, test) => exact.Diff(test.ToDouble()),
            calcExact: a => a.ToDouble().UnitToMatrix(),
            calcTestA: a => Matrix4x4.CreateFromAxisAngle(a.Axis, a.Angle),
            calcTestB: a => a.UnitToMatrix());

        Assert.IsTrue(result.AvgDiffB == result.AvgDiffA);
        Assert.IsTrue(result.MaxDiffB == result.MaxDiffA);
        Assert.IsTrue(result.MaxDiffB <= 6.5696815e-7f);
    }

    [TestMethod]
    public void UnitToQuaternion()
    {
        var result = TestAB(
            createSrc: Randomizer.CreateUnitAxisAngle,
            compare: (exact, test) => exact.Diff(test.ToDouble()),
            calcExact: a => a.ToDouble().UnitToQuaternion(),
            calcTestA: a => Quaternion.CreateFromAxisAngle(a.Axis, a.Angle),
            calcTestB: a => a.UnitToQuaternion());

        Assert.IsTrue(result.AvgDiffB == result.AvgDiffA);
        Assert.IsTrue(result.MaxDiffB == result.MaxDiffA);
        Assert.IsTrue(result.MaxDiffB <= 1.5170345e-7f);
    }
}
