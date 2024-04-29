namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Double;
using System.Numerics;

[TestClass]
public sealed class Matrix4x4Tests : TestsBase
{
    [TestMethod]
    public void UnitToEulerAngles_MainZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles_MainZone()
                                 .UnitToMatrix().ToSystem(),
            compare: (src, test) => src.ToDouble().UnitToQuaternion().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: m => m.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 2.768764e-7f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MiddleZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles_MiddleZone()
                                 .UnitToMatrix().ToSystem(),
            compare: (src, test) => src.ToDouble().UnitToQuaternion().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: m => m.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 8.631746e-6f);
    }

    [TestMethod]
    public void UnitToEulerAngles_PolarZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles_PolarZone()
                                 .UnitToMatrix().ToSystem(),
            compare: (src, test) => src.ToDouble().UnitToQuaternion().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: m => m.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 0.0002441789f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_Draft()
    {
        var result = Test(
            createSrc: Randomizer.CreateScaledMatrix,
            compare: (src, test) => src.ToDouble().Normalize().UnitToQuaternion().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: m => m.ScaledToEulerAngles_Draft());

        Assert.IsTrue(result.MaxDiff <= 0.0008673568f);
    }

    [TestMethod]
    public void UnitToQuaternion()
    {
        var result = TestAB(
            createSrc: Randomizer.CreateUnitMatrix,
            compare: (exact, test) => exact.Diff(test.ToDouble()),
            calcExact: m => m.ToDouble().UnitToQuaternion(),
            calcTestA: Quaternion.CreateFromRotationMatrix,
            calcTestB: m => m.UnitToQuaternion());

        Assert.IsTrue(result.AvgDiffB < result.AvgDiffA);
        Assert.IsTrue(result.MaxDiffB < result.MaxDiffA);
        Assert.IsTrue(result.MaxDiffB <= 3.4299015e-7f);
    }
}
