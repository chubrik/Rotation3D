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
        var result = Prepare(
            create: Randomizer.CreateUnitQuaternion,
            srcToString: q => q.Stringify(),
            resToString: a => a.Stringify(),
            compare: (qSrc, aExp, aAct) => qSrc.Diff(aAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcExact: q => AxisAngle.Identity,  // No reason to convert
            calcSystem: q => AxisAngle.Identity, // System has no solution
            calcCustom: q => q.UnitToAxisAngle_Draft());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 0.00079226494f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MainZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MainZone()
                                          .UnitToQuaternion().RandomSign().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.Diff(eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 5.438924e-7f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MiddleZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MiddleZone()
                                          .UnitToQuaternion().RandomSign().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.Diff(eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 6.502867e-5f);
    }

    [TestMethod]
    public void UnitToEulerAngles_PolarZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_PolarZone()
                                          .UnitToQuaternion().RandomSign().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.Diff(eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 0.00076410174f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_MainZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MainZone()
                                          .UnitToQuaternion().RandomScaleAndSign().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.ToDouble().Normalize().ToSystem().Diff(
                                     eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 4.4703484e-7f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_MiddleZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MiddleZone()
                                          .UnitToQuaternion().RandomScaleAndSign().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.ToDouble().Normalize().ToSystem().Diff(
                                     eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 7.22229e-5f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_PolarZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_PolarZone()
                                          .UnitToQuaternion().RandomScaleAndSign().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.ToDouble().Normalize().ToSystem().Diff(
                                     eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 0.0010174275f);
    }

    [TestMethod]
    public void UnitToMatrix()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitQuaternion,
            srcToString: q => q.Stringify(),
            resToString: m => m.Stringify(),
            compare: (_, q1, q2) => q1.Diff(q2),
            calcExact: q => q.ToDouble().UnitToMatrix().ToSystem(),
            calcSystem: Matrix4x4.CreateFromQuaternion,
            calcCustom: q => q.UnitToMatrix());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 5.438924e-7f);
    }

    [TestMethod]
    public void ScaledToMatrix()
    {
        var result = Prepare(
            create: Randomizer.CreateScaledQuaternion,
            srcToString: q => q.Stringify(),
            resToString: m => m.Stringify(),
            compare: (_, q1, q2) => q1.Diff(q2),
            calcExact: q => q.ToDouble().Normalize().UnitToMatrix().ToSystem(),
            calcSystem: q => Matrix4x4.CreateFromQuaternion(Quaternion.Normalize(q)),
            calcCustom: q => q.ScaledToMatrix());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 1.1026859e-6f);
    }
}
