namespace Rotation3D;

using Rotation3D.Double;
using System.Numerics;

public static class QuaternionExtensions
{
    public static bool IsUnitAbout(this Quaternion quaternion)
    {
        var diff = quaternion.UnitDiff();
        var isUnit = diff <= Constants.QuaternionUnitMaxDiff;

        if (!isUnit)
            Console.WriteLine($"Quaternion unit diff is {diff}");

        return isUnit;
    }

    public static float UnitDiff(this Quaternion quaternion)
    {
        return (float)quaternion.ToDouble().UnitDiff();
    }

    public static string Stringify(this Quaternion q)
    {
        return $"{{ {q.X.Stringify(",")} {q.Y.Stringify(",")} {q.Z.Stringify(",")} {q.W.Stringify()} }}";
    }
}
