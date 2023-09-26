namespace Trigonometry;

using System.Numerics;

public static class EulerAnglesExtensions
{
    public static Matrix4x4 ToMatrix(this EulerAngles eulerAngles)
    {
        return Matrix4x4.CreateFromYawPitchRoll(
            yaw: eulerAngles.Yaw,
            pitch: eulerAngles.Pitch,
            roll: eulerAngles.Roll);
    }

    public static Quaternion ToQuaternion(this EulerAngles eulerAngles)
    {
        return Quaternion.CreateFromYawPitchRoll(
            yaw: eulerAngles.Yaw,
            pitch: eulerAngles.Pitch,
            roll: eulerAngles.Roll);
    }

    [Obsolete("Not implemented.")]
    public static AxisAngle ToAxisAngle(this EulerAngles eulerAngles)
    {
        throw new NotImplementedException();
    }
}
