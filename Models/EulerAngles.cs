namespace Trigonometry;

using System.Diagnostics;
using static Constants;

// Reference:
// https://www.euclideanspace.com/maths/geometry/rotations/euler/

[DebuggerDisplay("yaw (y): {YawDegrees}°, pitch (x): {PitchDegrees}°, roll (z): {RollDegrees}°")]
public readonly struct EulerAngles
{
    public static readonly EulerAngles Identity = default;

    /// <summary>
    /// Aka heading, azimuth, Y axis. Applied first.
    /// </summary>
    public readonly float Yaw;

    /// <summary>
    /// Aka attitude, elevation, X axis. Applied second.
    /// </summary>
    public readonly float Pitch;

    /// <summary>
    /// Aka bank, tilt, Z axis. Applied last.
    /// </summary>
    public readonly float Roll;

    public EulerAngles(float yaw, float pitch, float roll)
    {
        Yaw = yaw;
        Pitch = pitch;
        Roll = roll;
    }

    public float YawDegrees => Yaw * RAD_TO_DEG;
    public float PitchDegrees => Pitch * RAD_TO_DEG;
    public float RollDegrees => Roll * RAD_TO_DEG;

    public float X => Pitch;
    public float Y => Yaw;
    public float Z => Roll;

    public float XDegrees => Pitch * RAD_TO_DEG;
    public float YDegrees => Yaw * RAD_TO_DEG;
    public float ZDegrees => Roll * RAD_TO_DEG;

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
}
