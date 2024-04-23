namespace Rotation3D;

public static class AxisAngleExtensions
{
    public static bool IsUnitAbout(this AxisAngle axisAngle)
    {
        return axisAngle.Axis.IsUnitAbout()
            && axisAngle.Angle.IsUnitAngle();
    }

    public static string Stringify(this AxisAngle a)
    {
        return $"{{ {a.Axis.X.Stringify(",")} {a.Axis.Y.Stringify(",")} {a.Axis.Z.Stringify()} | {a.Angle.Stringify()} }} " +
            $"{{ {a.AngleDegrees.Stringify("°")} }}";
    }
}
