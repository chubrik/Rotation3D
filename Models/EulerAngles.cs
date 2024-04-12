namespace Rotation3D;

using static Constants;

// Reference:
// https://www.euclideanspace.com/maths/geometry/rotations/euler/

public readonly struct EulerAngles
{
    public static readonly EulerAngles Identity = default;

    /// <summary>Aka heading, azimuth, Y axis. Applied first.</summary>
    public readonly float Yaw;

    /// <summary>Aka attitude, elevation, X axis. Applied second.</summary>
    public readonly float Pitch;

    /// <summary>Aka bank, tilt, Z axis. Applied last.</summary>
    public readonly float Roll;

    public readonly float YawDegrees => Yaw * F_RAD_TO_DEG;
    public readonly float PitchDegrees => Pitch * F_RAD_TO_DEG;
    public readonly float RollDegrees => Roll * F_RAD_TO_DEG;

    public EulerAngles(float yaw, float pitch, float roll)
    {
        Yaw = yaw;
        Pitch = pitch;
        Roll = roll;
    }

    public static EulerAngles FromDegrees(float yaw, float pitch, float roll)
    {
        return new EulerAngles(yaw * F_DEG_TO_RAD, pitch * F_DEG_TO_RAD, roll * F_DEG_TO_RAD);
    }

    public override readonly string ToString()
    {
        return $"{{YAW:{Yaw} PITCH:{Pitch} ROLL:{Roll}}} {{YAW:{YawDegrees}° PITCH:{PitchDegrees}° ROLL:{RollDegrees}°}}";
    }
}
