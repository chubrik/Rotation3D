namespace Rotation3D;

using static Constants;

public static class EulerAnglesExtensions
{
    public static bool IsValid(this EulerAngles eulerAngles)
    {
        return eulerAngles.Pitch >= -F_HALF_PI &&
               eulerAngles.Pitch <= F_HALF_PI;
    }

    public static bool IsUnit(this EulerAngles eulerAngles)
    {
        return eulerAngles.IsValid() &&
               eulerAngles.Yaw.IsUnitAngle() &&
               eulerAngles.Roll.IsUnitAngle();
    }

    public static string Stringify(this EulerAngles e)
    {
        return $"{{ {e.Yaw.Stringify(",")} {e.Pitch.Stringify(",")} {e.Roll.Stringify()} }} " +
            $"{{ {e.YawDegrees.Stringify("°,")} {e.PitchDegrees.Stringify("°,")} {e.RollDegrees.Stringify("°")} }}";
    }
}
