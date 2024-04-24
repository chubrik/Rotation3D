namespace Rotation3D.Double;

using System.Numerics;
using static DoubleConstants;
using static Math;

public static class DoubleExtensions
{
    [Obsolete("Need to prove")]
    public static double NormalizeAngleHard(this double angle)
    {
        var normAngle = angle;

        if (normAngle < -PI)
            do normAngle += TWO_PI;
            while (normAngle < -PI);
        else if (normAngle > PI)
            do normAngle -= TWO_PI;
            while (normAngle > PI);

        return normAngle;
    }

    public static bool IsUnitAngle_F(this double angle) => angle >= -Constants.F_PI && angle <= Constants.F_PI;

    #region System to Double

    public static DoubleAxisAngle ToDouble(this AxisAngle axisAngle)
    {
        return new(axisAngle.Axis.ToDouble(), axisAngle.Angle);
    }

    public static DoubleEulerAngles ToDouble(this EulerAngles eulerAngles)
    {
        return new(eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);
    }

    public static DoubleMatrix4x4 ToDouble(this Matrix4x4 matrix)
    {
        return new(matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                   matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                   matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                   matrix.M41, matrix.M42, matrix.M43, matrix.M44);
    }

    public static DoubleQuaternion ToDouble(this Quaternion quaternion)
    {
        return new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }

    public static DoubleVector3 ToDouble(this Vector3 vector)
    {
        return new(vector.X, vector.Y, vector.Z);
    }

    #endregion

    #region Double to System

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

    #endregion
}
