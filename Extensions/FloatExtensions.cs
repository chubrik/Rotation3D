namespace Rotation3D;

using static Constants;

public static class FloatExtensions
{
    public static bool IsUnitAngle(this float angle) => angle >= -F_PI && angle <= F_PI;

    public static string Stringify(this float number, string end = "")
    {
        return (number.ToString().Replace("E+0", "e+").Replace("E-0", "e-") + end).PadRight(12);
    }
}
