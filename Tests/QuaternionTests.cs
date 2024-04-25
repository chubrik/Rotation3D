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
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MainZone().UnitToQuaternion().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.Diff(eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 5.4296106e-7f);
    }

    [TestMethod]
    public void UnitToEulerAngles_MiddleZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MiddleZone().UnitToQuaternion().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.Diff(eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 6.0617924e-5f);
    }

    [TestMethod]
    public void UnitToEulerAngles_PolarZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_PolarZone().UnitToQuaternion().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.Diff(eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 0.00073614717f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_MainZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MainZone().UnitToQuaternion().RandomScale().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.ToDouble().Normalize().ToSystem().Diff(
                                     eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 4.3213367e-7f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_MiddleZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_MiddleZone().UnitToQuaternion().RandomScale().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.ToDouble().Normalize().ToSystem().Diff(
                                     eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 6.5336004e-5f);
    }

    [TestMethod]
    public void ScaledToEulerAngles_PolarZone()
    {
        var result = Prepare(
            create: () => DoubleRandomizer.CreateUnitEulerAngles_PolarZone().UnitToQuaternion().RandomScale().ToSystem(),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            compare: (qSrc, eAct) => qSrc.ToDouble().Normalize().ToSystem().Diff(
                                     eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcCustom: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.MaxDiffCustom <= 0.0010098591f);
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

        Assert.IsTrue(result.AvgDiffCustom == result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom == result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 5.8859587e-7f);
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
        Assert.IsTrue(result.MaxDiffCustom <= 1.180917e-6f);
    }
}
