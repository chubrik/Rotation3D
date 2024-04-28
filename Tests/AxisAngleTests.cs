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
            srcToString: a => a.Stringify(),
            resToString: e => e.Stringify(),
            compare: (aSrc, eExp, eAct) => aSrc.ToDouble().UnitToQuaternion().ToSystem().Diff(
                                           eAct.ToDouble().UnitToQuaternion().ToSystem()),
            calcExact: a => EulerAngles.Identity,  // No reason to convert
            calcSystem: a => EulerAngles.Identity, // System has no solution
            calcCustom: a => a.UnitToEulerAngles());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 0.00086560845f);
    }

    [TestMethod]
    public void UnitToMatrix()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitAxisAngle,
            srcToString: a => a.Stringify(),
            resToString: m => m.Stringify(),
            compare: (_, m1, m2) => m1.Diff(m2),
            calcExact: a => a.ToDouble().UnitToMatrix().ToSystem(),
            calcSystem: a => Matrix4x4.CreateFromAxisAngle(a.Axis, a.Angle),
            calcCustom: a => a.UnitToMatrix());

        Assert.IsTrue(result.AvgDiffCustom == result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom == result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 7.4505806e-7f);
    }

    [TestMethod]
    public void UnitToQuaternion()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitAxisAngle,
            srcToString: a => a.Stringify(),
            resToString: m => m.Stringify(),
            compare: (_, m1, m2) => m1.Diff(m2),
            calcExact: a => a.ToDouble().UnitToQuaternion().ToSystem(),
            calcSystem: a => Quaternion.CreateFromAxisAngle(a.Axis, a.Angle),
            calcCustom: a => a.UnitToQuaternion());

        Assert.IsTrue(result.AvgDiffCustom == result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom == result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 2.0861626e-7f);
    }
}
