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
        var result = Prepare(
            create: Randomizer.CreateUnitAxisAngle,
            toDouble: a => a,
            fromDouble: e => e,
            compare: (aSrc, eExp, eAct) => aSrc.ToDouble().UnitToQuaternion().ToSystem().Diff(eAct.ToDouble().UnitToQuaternion().ToSystem()),
            srcToString: a => a.Stringify(),
            resToString: e => e.Stringify(),
            calcDouble: a => EulerAngles.Identity, // No reason to convert
            calcSystem: a => EulerAngles.Identity, // System has no solution
            calcCustom: a => a.UnitToEulerAngles());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 0.00085264444f);
    }

    [TestMethod]
    public void UnitToMatrix()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitAxisAngle,
            toDouble: a => a.ToDouble(),
            fromDouble: m => m.ToSystem(),
            compare: (_, m1, m2) => m1.Diff(m2),
            srcToString: a => a.Stringify(),
            resToString: m => m.Stringify(),
            calcDouble: a => a.UnitToMatrix(),
            calcSystem: a => Matrix4x4.CreateFromAxisAngle(a.Axis, a.Angle),
            calcCustom: a => a.UnitToMatrix());

        Assert.IsTrue(result.AvgDiffCustom == result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom == result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 7.1525574e-7f);
    }

    [TestMethod]
    public void UnitToQuaternion()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitAxisAngle,
            toDouble: a => a.ToDouble(),
            fromDouble: m => m.ToSystem(),
            compare: (_, m1, m2) => m1.Diff(m2),
            srcToString: a => a.Stringify(),
            resToString: m => m.Stringify(),
            calcDouble: a => a.UnitToQuaternion(),
            calcSystem: a => Quaternion.CreateFromAxisAngle(a.Axis, a.Angle),
            calcCustom: a => a.UnitToQuaternion());

        Assert.IsTrue(result.AvgDiffCustom == result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom == result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 2.0861626e-7f);
    }
}
