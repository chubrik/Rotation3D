namespace Rotation3D;

public static class AxisAngleExtensions
{
    public static bool IsUnit(this AxisAngle axisAngle)
    {
        return axisAngle.Axis.IsUnit()
            && axisAngle.Angle.IsUnitAngle();
    }

    public static string Stringify(this AxisAngle a)
    {
        return $"{{ {a.Axis.X.Stringify(",")} {a.Axis.Y.Stringify(",")} {a.Axis.Z.Stringify()} | {a.Angle.Stringify()} }} " +
            $"{{ {a.AngleDegrees.Stringify("°")} }}";
    }
}
