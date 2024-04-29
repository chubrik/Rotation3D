namespace Rotation3D;

using Rotation3D.Double;
using System.Numerics;

public static class Matrix4x4Extensions
{
    public static bool IsUnitAbout(this Matrix4x4 matrix)
    {
        var diff = matrix.UnitDiff();
        var isUnit = diff <= Constants.MatrixUnitMaxDiff;

        if (!isUnit)
            Console.WriteLine($"Matrix4x4 unit diff is {diff}");

        return isUnit;
    }

    public static float UnitDiff(this Matrix4x4 matrix)
    {
        return (float)matrix.ToDouble().UnitDiff();
    }

    public static string Stringify(this Matrix4x4 m)
    {
        return $"{{ " +
            $"{m.M11.Stringify()} {m.M12.Stringify()} {m.M13.Stringify()} | " +
            $"{m.M21.Stringify()} {m.M22.Stringify()} {m.M23.Stringify()} | " +
            $"{m.M31.Stringify()} {m.M32.Stringify()} {m.M33.Stringify()} }}";
    }
}
