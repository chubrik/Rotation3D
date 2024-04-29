namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Double;
using System.Numerics;

[TestClass]
public sealed class QuaternionTests : TestsBase
{
    [TestMethod]
    public void UnitToAxisAngle_Draft()
    {
        var result = Test(
            createSrc: Randomizer.CreateUnitQuaternion,
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToAxisAngle_Draft());

        Assert.IsTrue(result.MaxDiff <= 1.5067726e-6f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MainZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles_MainZone()
                                 .UnitToQuaternion().RandomSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 5.044793e-7f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MiddleZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles_MiddleZone()
                                 .UnitToQuaternion().RandomSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 6.35474e-5f);
    }

    [TestMethod]
    public void UnitToEulerAngles_PolarZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles_PolarZone()
                                 .UnitToQuaternion().RandomSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 0.0007375982f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_MainZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles_MainZone()
                                 .UnitToQuaternion().RandomScaleAndSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Normalize().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 4.045536e-7f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_MiddleZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles_MiddleZone()
                                 .UnitToQuaternion().RandomScaleAndSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Normalize().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 7.3590425e-5f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_PolarZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles_PolarZone()
                                 .UnitToQuaternion().RandomScaleAndSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Normalize().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 0.0010101415f);
    }

    [TestMethod]
    public void UnitToMatrix()
    {
        var result = TestAB(
            createSrc: Randomizer.CreateUnitQuaternion,
            compare: (exact, test) => exact.Diff(test.ToDouble()),
            calcExact: q => q.ToDouble().UnitToMatrix(),
            calcTestA: Matrix4x4.CreateFromQuaternion,
            calcTestB: q => q.UnitToMatrix());

        Assert.IsTrue(result.AvgDiffB < result.AvgDiffA);
        Assert.IsTrue(result.MaxDiffB < result.MaxDiffA);
        Assert.IsTrue(result.MaxDiffB <= 4.949115e-7f);
    }

    [TestMethod]
    public void ScaledToMatrix()
    {
        var result = TestAB(
            createSrc: Randomizer.CreateScaledQuaternion,
            compare: (exact, test) => exact.Diff(test.ToDouble()),
            calcExact: q => q.ToDouble().Normalize().UnitToMatrix(),
            calcTestA: q => Matrix4x4.CreateFromQuaternion(Quaternion.Normalize(q)),
            calcTestB: q => q.ScaledToMatrix());

        Assert.IsTrue(result.AvgDiffB < result.AvgDiffA);
        Assert.IsTrue(result.MaxDiffB < result.MaxDiffA);
        Assert.IsTrue(result.MaxDiffB <= 1.1111016e-6f);
    }
}
