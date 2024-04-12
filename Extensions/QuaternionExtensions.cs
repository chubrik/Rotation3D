namespace Rotation3D;

using Rotation3D.Double;
using System.Numerics;
using static MathF;

public static class QuaternionExtensions
{
    public static bool IsUnit(this Quaternion quaternion)
    {
        var diff = quaternion.DiffUnit();
        var isUnit = diff <= Constants.QuaternionUnitMaxDiff;

        if (!isUnit)
            Console.WriteLine($"Quaternion unit diff is {diff}");

        return isUnit;
    }

    public static float DiffUnit(this Quaternion quaternion)
    {
        return (float)quaternion.ToDouble().DiffUnit();
    }

    public static float Diff(this Quaternion q1, Quaternion q2)
    {
        var diffX1 = Abs(q1.X - q2.X);
        var diffY1 = Abs(q1.Y - q2.Y);
        var diffZ1 = Abs(q1.Z - q2.Z);
        var diffW1 = Abs(q1.W - q2.W);

        var diffX2 = Abs(q1.X + q2.X);
        var diffY2 = Abs(q1.Y + q2.Y);
        var diffZ2 = Abs(q1.Z + q2.Z);
        var diffW2 = Abs(q1.W + q2.W);

        var diffSum1 = diffX1 + diffY1 + diffZ1 + diffW1;
        var diffSum2 = diffX2 + diffY2 + diffZ2 + diffW2;
        var diffSum = Min(diffSum1, diffSum2);
        return diffSum;
    }

    public static string Stringify(this Quaternion q)
    {
        return $"{{ {q.X.Stringify(",")} {q.Y.Stringify(",")} {q.Z.Stringify(",")} {q.W.Stringify()} }}";
    }
}
