namespace Trigonometry;

using System.Numerics;

public static class AxisAngleExtensions
{
    [Obsolete("Not tested.")]
    public static Matrix4x4 ToMatrix(this AxisAngle axisAngle)
    {
        return Matrix4x4.CreateFromAxisAngle(
            axis: axisAngle.Axis,
            angle: axisAngle.Angle);
    }

    [Obsolete("Not tested.")]
    public static Quaternion ToQuaternion(this AxisAngle axisAngle)
    {
        return Quaternion.CreateFromAxisAngle(
            axis: axisAngle.Axis.Normalize(), // Vector must be normalized
            angle: axisAngle.Angle);
    }

    [Obsolete("Not implemented.")]
    public static EulerAngles ToEulerAngles(this AxisAngle axisAngle)
    {
        throw new NotImplementedException();
    }

    public static AxisAngle Normalize(this AxisAngle axisAngle)
    {
        return new AxisAngle(axis: axisAngle.Axis.Normalize(), angle: axisAngle.Angle);
    }

    // For debug only
    public static bool IsNormalised(this AxisAngle axisAngle)
    {
        return axisAngle.Axis.IsNormalized();
    }
}
