﻿namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Double;
using System.Numerics;

[TestClass]
public sealed class EulerAnglesTests : TestsBase
{
    [TestMethod]
    public void UnitToMatrix()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitEulerAngles,
            toDouble: e => e.ToDouble(),
            fromDouble: m => m.ToSystem(),
            compare: (_, m1, m2) => m1.Diff(m2),
            srcToString: e => e.Stringify(),
            resToString: m => m.Stringify(),
            calcDouble: e => e.UnitToMatrix(),
            calcSystem: e => Matrix4x4.CreateFromYawPitchRoll(e.Yaw, e.Pitch, e.Roll),
            calcCustom: e => e.UnitToMatrix());

        Assert.IsTrue(result.AvgDiffCustom < result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom < result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 6.03497e-7f);
    }

    [TestMethod]
    public void UnitToQuaternion()
    {
        var result = Prepare(
            create: Randomizer.CreateUnitEulerAngles,
            toDouble: e => e.ToDouble(),
            fromDouble: q => q.ToSystem(),
            compare: (_, q1, q2) => q1.Diff(q2),
            srcToString: e => e.Stringify(),
            resToString: q => q.Stringify(),
            calcDouble: e => e.UnitToQuaternion(),
            calcSystem: e => Quaternion.CreateFromYawPitchRoll(e.Yaw, e.Pitch, e.Roll),
            calcCustom: e => e.UnitToQuaternion());

        Assert.IsTrue(result.AvgDiffCustom == result.AvgDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom == result.MaxDiffSystem);
        Assert.IsTrue(result.MaxDiffCustom <= 4.172325e-7f);
    }
}
