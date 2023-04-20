namespace Trigonometry;

using System.Diagnostics;
using static Constants;

[DebuggerDisplay("yaw: {YawDegrees}°, pitch: {PitchDegrees}°, roll: {RollDegrees}°")]
internal readonly struct EulerAngles
{
    public static EulerAngles Identity = default;

    public readonly float Yaw;
    public readonly float Pitch;
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

    public static EulerAngles CreateFromDegrees(float yaw, float pitch, float roll)
    {
        return new EulerAngles(yaw: yaw * DEG_TO_RAD, pitch: pitch * DEG_TO_RAD, roll: roll * DEG_TO_RAD);
    }
}
