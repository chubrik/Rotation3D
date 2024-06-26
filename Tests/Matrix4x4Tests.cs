﻿namespace Rotation3D.Tests;

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
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles(0, 45)
                                 .UnitToMatrix().ToSystem(),
            compare: (src, test) => src.ToDouble().UnitToQuaternion().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: m => m.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 2.7856484e-7f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MiddleZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles(45, 80)
                                 .UnitToMatrix().ToSystem(),
            compare: (src, test) => src.ToDouble().UnitToQuaternion().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: m => m.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 3.818611e-7f);
    }

    [TestMethod]
    public void UnitToEulerAngles_PolarZone()
    {
        var result = Test(
            createSrc: () => DoubleRandomizer.CreateUnitEulerAngles(80, 90)
                                 .UnitToMatrix().ToSystem(),
            compare: (src, test) => src.ToDouble().UnitToQuaternion().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: m => m.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiff <= 3.4429473e-7f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_Draft()
    {
        var result = Test(
            createSrc: Randomizer.CreateScaledMatrix,
            compare: (src, test) => src.ToDouble().Normalize().UnitToQuaternion().Diff(
                                    test.ToDouble().UnitToQuaternion()),
            calcTest: m => m.ScaledToEulerAngles_Draft());

        Assert.IsTrue(result.MaxDiff <= 0.000885636f);
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
        Assert.IsTrue(result.MaxDiffB <= result.MaxDiffA);
        Assert.IsTrue(result.MaxDiffB <= 3.4299015e-7f);
    }
}
