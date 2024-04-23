namespace Rotation3D;

using Rotation3D.Double;
using System.Numerics;
using static MathF;

public static class Matrix4x4Extensions
{
    public static bool IsUnitAbout(this Matrix4x4 matrix)
    {
        var diff = matrix.DiffUnit();
        var isUnit = diff <= Constants.MatrixUnitMaxDiff;

        if (!isUnit)
            Console.WriteLine($"Matrix4x4 unit diff is {diff}");

        return isUnit;
    }

    public static float DiffUnit(this Matrix4x4 matrix)
    {
        return (float)matrix.ToDouble().DiffUnit();
    }

    public static float Diff(this Matrix4x4 m1, Matrix4x4 m2)
    {
        var diff1 = Abs(m1.M11 - m2.M11);
        var diff2 = Abs(m1.M12 - m2.M12);
        var diff3 = Abs(m1.M13 - m2.M13);
        var diff4 = Abs(m1.M21 - m2.M21);
        var diff5 = Abs(m1.M22 - m2.M22);
        var diff6 = Abs(m1.M23 - m2.M23);
        var diff7 = Abs(m1.M31 - m2.M31);
        var diff8 = Abs(m1.M32 - m2.M32);
        var diff9 = Abs(m1.M33 - m2.M33);

        var diffSum = diff1 + diff2 + diff3 + diff4 + diff5 + diff6 + diff7 + diff8 + diff9;
        return diffSum;
    }

    public static string Stringify(this Matrix4x4 m)
    {
        return $"{{ " +
            $"{m.M11.Stringify()} {m.M12.Stringify()} {m.M13.Stringify()} | " +
            $"{m.M21.Stringify()} {m.M22.Stringify()} {m.M23.Stringify()} | " +
            $"{m.M31.Stringify()} {m.M32.Stringify()} {m.M33.Stringify()} }}";
    }
}
