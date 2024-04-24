namespace Rotation3D;

using Rotation3D.Double;
using System.Numerics;

public static class Vector3Extensions
{
    public static bool IsUnitAbout(this Vector3 vector)
    {
        var diff = vector.UnitDiff();
        var isUnit = diff <= Constants.Vector3UnitMaxDiff;

        if (!isUnit)
            Console.WriteLine($"Vector3 unit diff is {diff}");

        return isUnit;
    }

    public static float UnitDiff(this Vector3 vector)
    {
        return (float)vector.ToDouble().UnitDiff();
    }
}
