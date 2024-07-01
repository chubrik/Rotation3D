namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Double;
using System.Numerics;

[TestClass]
public sealed class QuaternionTests : TestsBase
{
    [TestMethod]
    public void UnitToAxisAngle_MaxAngle()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitAxisAngle(90, 180)
                                 .UnitToQuaternion().RandomSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToAxisAngle());

        Assert.IsTrue(result.MaxDiff <= 3.307663e-7f);
    }

    [TestMethod]
    public void UnitToAxisAngle_MidAngle()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitAxisAngle(1e-16, 90)
                                 .UnitToQuaternion().RandomSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToAxisAngle());

        Assert.IsTrue(result.MaxDiff <= 2.467498e-7f);
    }

    [TestMethod]
    public void UnitToAxisAngle_MinAngle()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitAxisAngle(0, 1e-16)
                                 .UnitToQuaternion().RandomSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToAxisAngle());

        Assert.IsTrue(result.MaxDiff <= 5.477197E-19f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MainZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles(0, 45)
                                 .UnitToQuaternion().RandomSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 5.2514747e-7f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MiddleZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles(45, 89.8)
                                 .UnitToQuaternion().RandomSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 6.824013e-5f);
    }

    [TestMethod]
    public void UnitToEulerAngles_PolarZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles(89.8, 90)
                                 .UnitToQuaternion().RandomSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 0.0007669093f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_MainZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles(0, 45)
                                 .UnitToQuaternion().RandomScaleAndSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Normalize().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 3.9518693e-7f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_MiddleZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles(45, 89.8)
                                 .UnitToQuaternion().RandomScaleAndSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Normalize().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 3.0514471e-5f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_PolarZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles(89.8, 90)
                                 .UnitToQuaternion().RandomScaleAndSign().ToSystem(),
            compare: (src, test) => src.ToDouble().Normalize().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 0.00038348345f);
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
        Assert.IsTrue(result.MaxDiffB <= 1.1411221e-6f);
    }
}
