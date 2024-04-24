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
            toDouble: q => q,
            fromDouble: a => a,
            compare: (qSrc, aExp, aAct) => qSrc.Diff(aAct.ToDouble().UnitToQuaternion().ToSystem()),
            srcToString: q => q.Stringify(),
            resToString: a => a.Stringify(),
            calcDouble: q => AxisAngle.Identity, // No reason to convert
            calcSystem: q => AxisAngle.Identity, // System has no solution
            calcCustom: q => q.UnitToAxisAngle_Draft());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 0.00079226494f);
    }

    [TestMethod]
    public void UnitToEulerAngles()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitQuaternion,
            toDouble: q => q,
            fromDouble: e => e,
            compare: (qSrc, eExp, eAct) => qSrc.Diff(eAct.ToDouble().UnitToQuaternion().ToSystem()),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            calcDouble: q => EulerAngles.Identity, // No reason to convert
            calcSystem: q => EulerAngles.Identity, // System has no solution
            calcCustom: q => q.UnitToEulerAngles());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 0.0008571595f);
    }

    [TestMethod]
    public void ScaledToEulerAngles()
    {
        var result = Prepare(
            create: Randomizer.CreateScaledQuaternion,
            toDouble: q => q,
            fromDouble: e => e,
            compare: (qSrc, eExp, eAct) => qSrc.ToDouble().Normalize().ToSystem().Diff(
                                           eAct.ToDouble().UnitToQuaternion().ToSystem()),
            srcToString: q => q.Stringify(),
            resToString: e => e.Stringify(),
            calcDouble: q => EulerAngles.Identity, // No reason to convert
            calcSystem: q => EulerAngles.Identity, // System has no solution
            calcCustom: q => q.ScaledToEulerAngles());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 0.00086191297f);
    }

    [TestMethod]
    public void UnitToMatrix()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitQuaternion,
            toDouble: q => q.ToDouble(),
            fromDouble: m => m.ToSystem(),
            compare: (_, q1, q2) => q1.Diff(q2),
            srcToString: q => q.Stringify(),
            resToString: m => m.Stringify(),
            calcDouble: q => q.UnitToMatrix(),
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
            toDouble: q => q.ToDouble(),
            fromDouble: m => m.ToSystem(),
            compare: (_, q1, q2) => q1.Diff(q2),
            srcToString: q => q.Stringify(),
            resToString: m => m.Stringify(),
            calcDouble: q => q.Normalize().UnitToMatrix(),
            calcSystem: q => Matrix4x4.CreateFromQuaternion(Quaternion.Normalize(q)),
            calcCustom: q => q.ScaledToMatrix());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 1.1771917e-6f);
    }
}
