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
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MainZone().UnitToMatrix().ToSystem(),
            srcToString: m => m.Stringify(),
            resToString: e => e.Stringify(),
            compare: (mSrc, eAct) => mSrc.ToDouble().UnitToQuaternion().ToSystem().Diff(
                                     eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: m => m.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 3.5762787e-7f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MiddleZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MiddleZone().UnitToMatrix().ToSystem(),
            srcToString: m => m.Stringify(),
            resToString: e => e.Stringify(),
            compare: (mSrc, eAct) => mSrc.ToDouble().UnitToQuaternion().ToSystem().Diff(
                                     eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: m => m.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 8.6426735e-6f);
    }

    [TestMethod]
    public void UnitToEulerAngles_PolarZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_PolarZone().UnitToMatrix().ToSystem(),
            srcToString: m => m.Stringify(),
            resToString: e => e.Stringify(),
            compare: (mSrc, eAct) => mSrc.ToDouble().UnitToQuaternion().ToSystem().Diff(
                                     eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: m => m.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 0.00024423003f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_Draft()
    {
        var result = Prepare(
            create: Randomizer.CreateScaledMatrix,
            srcToString: m => m.Stringify(),
            resToString: e => e.Stringify(),
            compare: (mSrc, eExp, eAct) => mSrc.ToDouble().Normalize().UnitToQuaternion().ToSystem().Diff(
                                           eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcExact: m => EulerAngles.Identity,  // No reason to convert
            calcSystem: m => EulerAngles.Identity, // System has no solution
            calcCustom: m => m.ScaledToEulerAngles_Draft());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 0.00091201067f);
    }

    [TestMethod]
    public void UnitToQuaternion()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitMatrix,
            srcToString: m => m.Stringify(),
            resToString: q => q.Stringify(),
            compare: (_, m1, m2) => m1.Diff(m2),
            calcExact: m => m.ToDouble().UnitToQuaternion().ToSystem(),
            calcSystem: Quaternion.CreateFromRotationMatrix,
            calcCustom: m => m.UnitToQuaternion());

        Assert.IsTrue(result.AvgDiffCustom == result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom == result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 3.874302e-7f);
    }
}
