namespace Rotation3D;

using static Constants;

public static class NumberExtensions
{
    public static float NormalizeAngleSoft(this float angle)
    {
        return angle < -F_PI ? angle + F_TWO_PI
            : angle > F_PI ? angle - F_TWO_PI
            : angle;
    }

    public static bool IsUnitAngle(this float angle) => angle >= -F_PI && angle <= F_PI;

    public static string Stringify(this float number, string end = "")
    {
        return (number.ToString().Replace("E+0", "e+").Replace("E-0", "e-") + end).PadRight(12);
    }
}
