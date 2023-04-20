namespace Trigonometry;

using System.Diagnostics;
using System.Numerics;

internal static class AxisAngleExtensions
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
        Debug.Assert(axisAngle.Axis.IsNormalized());

        return Quaternion.CreateFromAxisAngle(
            axis: axisAngle.Axis,
            angle: axisAngle.Angle);
    }

    [Obsolete("Not implemented.")]
    public static EulerAngles ToEulerAngles(this AxisAngle axisAngle)
    {
        throw new NotImplementedException();
    }
}
