namespace Rotation3D.Double;

using System.Numerics;

public static class DoubleExtensions
{
    public static DoubleAxisAngle ToDouble(this AxisAngle axisAngle) => new(axisAngle);

    public static DoubleEulerAngles ToDouble(this EulerAngles eulerAngles) => new(eulerAngles);

    public static DoubleMatrix4x4 ToDouble(this Matrix4x4 matrix) => new(matrix);

    public static DoubleQuaternion ToDouble(this Quaternion quaternion) => new(quaternion);

    public static DoubleVector3 ToDouble(this Vector3 vector) => new(vector);

    //

    public static AxisAngle ToSystem(this DoubleAxisAngle axisAngle)
    {
        return new(axisAngle.Axis.Tosystem(), (float)axisAngle.Angle);
    }

    public static EulerAngles ToSystem(this DoubleEulerAngles eulerAngles)
    {
        return new((float)eulerAngles.Yaw, (float)eulerAngles.Pitch, (float)eulerAngles.Roll);
    }

    public static Matrix4x4 ToSystem(this DoubleMatrix4x4 matrix)
    {
        return new((float)matrix.M11, (float)matrix.M12, (float)matrix.M13, (float)matrix.M14,
                   (float)matrix.M21, (float)matrix.M22, (float)matrix.M23, (float)matrix.M24,
                   (float)matrix.M31, (float)matrix.M32, (float)matrix.M33, (float)matrix.M34,
                   (float)matrix.M41, (float)matrix.M42, (float)matrix.M43, (float)matrix.M44);
    }

    public static Quaternion ToSystem(this DoubleQuaternion quaternion)
    {
        return new((float)quaternion.X, (float)quaternion.Y, (float)quaternion.Z, (float)quaternion.W);
    }

    public static Vector3 Tosystem(this DoubleVector3 vector)
    {
        return new((float)vector.X, (float)vector.Y, (float)vector.Z);
    }
}
