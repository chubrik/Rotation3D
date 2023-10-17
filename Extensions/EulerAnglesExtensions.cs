namespace Trigonometry;

using System.Numerics;
using static System.MathF;

public static class EulerAnglesExtensions
{
    [Obsolete("Not direct.")]
    public static Matrix4x4 ToMatrix(this EulerAngles eulerAngles)
    {
        // Reference:
        // return Matrix4x4.CreateFromYawPitchRoll(eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);

        var (yaw, pitch, roll) = (eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);

        var q = Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
        return Matrix4x4.CreateFromQuaternion(q);
    }

    public static Quaternion ToQuaternion(this EulerAngles eulerAngles)
    {
        // Reference:
        // return Quaternion.CreateFromYawPitchRoll(eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);

        var (yaw, pitch, roll) = (eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);

        var halfYaw = yaw * 0.5f;
        var halfPitch = pitch * 0.5f;
        var halfRoll = roll * 0.5f;

        var sy = Sin(halfYaw);
        var cy = Cos(halfYaw);
        var sp = Sin(halfPitch);
        var cp = Cos(halfPitch);
        var sr = Sin(halfRoll);
        var cr = Cos(halfRoll);

        var sysp = sy * sp;
        var sycp = sy * cp;
        var cysp = cy * sp;
        var cycp = cy * cp;

        var qX = cysp * cr + sycp * sr;
        var qY = sycp * cr - cysp * sr;
        var qZ = cycp * sr - sysp * cr;
        var qW = cycp * cr + sysp * sr;

        return new Quaternion(qX, qY, qZ, qW);
    }

    [Obsolete("Not implemented.")]
    public static AxisAngle ToAxisAngle(this EulerAngles eulerAngles)
    {
        throw new NotImplementedException();
    }
}
