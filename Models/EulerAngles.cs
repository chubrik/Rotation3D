namespace Trigonometry;

using System.Diagnostics;
using static Constants;

// https://www.euclideanspace.com/maths/geometry/rotations/euler/

[DebuggerDisplay("yaw (y): {YawDegrees}°, pitch (x): {PitchDegrees}°, roll (z): {RollDegrees}°")]
public readonly struct EulerAngles
{
    public static readonly EulerAngles Identity = default;

    // Aka heading, azimuth, Y axis. Applied first.
    public readonly float Yaw;

    // Aka attitude, elevation, X axis. Applied second.
    public readonly float Pitch;

    // Aka bank, tilt, Z axis. Applied last.
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
        return new EulerAngles(yaw: yaw * DEG_TO_RAD, pitch: pitch * DEG_TO_RAD, roll: roll * DEG_TO_RAD);
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
