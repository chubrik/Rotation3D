namespace Rotation3D;

using static MathFConstants;

// Reference:
// https://www.euclideanspace.com/maths/geometry/rotations/euler/

public struct EulerAngles
{
    public static readonly EulerAngles Identity = default;

    /// <summary>
    /// Aka heading, azimuth, Y axis. Applied first.
    /// </summary>
    public float Yaw;

    /// <summary>
    /// Aka attitude, elevation, X axis. Applied second.
    /// </summary>
    public float Pitch;

    /// <summary>
    /// Aka bank, tilt, Z axis. Applied last.
    /// </summary>
    public float Roll;

    public EulerAngles(float yaw, float pitch, float roll)
    {
        Yaw = yaw;
        Pitch = pitch;
        Roll = roll;
    }

    public readonly float YawDegrees => Yaw * RAD_TO_DEG;
    public readonly float PitchDegrees => Pitch * RAD_TO_DEG;
    public readonly float RollDegrees => Roll * RAD_TO_DEG;

    public readonly float X => Pitch;
    public readonly float Y => Yaw;
    public readonly float Z => Roll;

    public readonly float XDegrees => Pitch * RAD_TO_DEG;
    public readonly float YDegrees => Yaw * RAD_TO_DEG;
    public readonly float ZDegrees => Roll * RAD_TO_DEG;

    public static EulerAngles CreateFromDegrees(float yaw, float pitch, float roll)
    {
        return new EulerAngles(yaw * DEG_TO_RAD, pitch * DEG_TO_RAD, roll * DEG_TO_RAD);
    }

    public static EulerAngles CreateFromXYZ(float x, float y, float z)
    {
        return new EulerAngles(yaw: y, pitch: x, roll: z);
    }

    public static EulerAngles CreateFromXYZDegrees(float x, float y, float z)
    {
        return new EulerAngles(yaw: y * DEG_TO_RAD, pitch: x * DEG_TO_RAD, roll: z * DEG_TO_RAD);
    }

    public override readonly string ToString()
    {
        return $"{{YAW:{Yaw} PITCH:{Pitch} ROLL:{Roll}}} {{YAW:{YawDegrees}° PITCH:{PitchDegrees}° ROLL:{RollDegrees}°}}";
    }
}
