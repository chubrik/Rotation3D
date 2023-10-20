namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

[TestClass]
public sealed class Matrix4x4Tests : TestsBase
{
    [TestMethod]
    public void ToEulerAngles()
    {
        const float maxDiffAllowed = 0.0022386455f;

        var maxDiff = 0f;
        var maxDiffEulerAngles = default(EulerAngles);
        var maxDiffMatrixSource = default(Matrix4x4);
        var maxDiffMatrixRevert = default(Matrix4x4);

        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var matrixSource = GetRandomNormalMatrix4x4();
            var eulerAngles = matrixSource.ToEulerAngles();
            var matrixRevert = Matrix4x4.CreateFromYawPitchRoll(eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);
            var diff = CalcSumDiff(matrixSource, matrixRevert);

            if (maxDiff < diff)
            {
                maxDiff = diff;
                maxDiffEulerAngles = eulerAngles;
                maxDiffMatrixSource = matrixSource;
                maxDiffMatrixRevert = matrixRevert;
            }
        }

        Console.WriteLine($"Max diff: {maxDiff}");

        if (maxDiff > 0)
        {
            Console.WriteLine($"EulerAngles: {maxDiffEulerAngles}");
            Console.WriteLine($"Matrix4x4 source: {maxDiffMatrixSource}");
            Console.WriteLine($"Matrix4x4 revert: {maxDiffMatrixRevert}");

            if (maxDiff > maxDiffAllowed)
                Assert.Fail();
        }
    }

    [TestMethod]
    public void ToQuaternion()
    {
        var iteration = 0;

        while (++iteration <= _iterationCount)
        {
            var matrix = GetRandomNormalMatrix4x4();
            var quaternionSystem = Quaternion.CreateFromRotationMatrix(matrix);
            var quaternionCustom = matrix.ToQuaternion();
            Assert.AreEqual(quaternionSystem, quaternionCustom);
        }

        Console.WriteLine("Max diff: 0");
    }
}
