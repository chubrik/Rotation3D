namespace Trigonometry;

using System.Numerics;
using static Constants;
using static MathF;

internal static class Matrix4x4Extensions
{
    [Obsolete("Not tested.")]
    public static Quaternion ToQuaternion(this Matrix4x4 matrix)
    {
        return Quaternion.CreateFromRotationMatrix(matrix);
    }

    // https://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToEuler/
    [Obsolete("Not tested.")]
    public static EulerAngles ToEulerAngles(this Matrix4x4 matrix)
    {
        var (m11, m13, m21, m22, m23, m31, m33) =
            (matrix.M11, matrix.M13, matrix.M21, matrix.M22, matrix.M23, matrix.M31, matrix.M33);
        
        float yaw, pitch, roll;

        if (m21 > 0.998)
        {
            // singularity at north pole
            yaw = Atan2(m13, m33);
            pitch = POSITIVE_HALF_PI;
            roll = 0;
        }
        else if (m21 < -0.998)
        {
            // singularity at south pole
            yaw = Atan2(m13, m33);
            pitch = NEGATIVE_HALF_PI;
            roll = 0;
        }
        else
        {
            yaw = Atan2(-m31, m11);
            pitch = Asin(m21);
            roll = Atan2(-m23, m22);
        }

        return new EulerAngles(yaw: yaw, pitch: pitch, roll: roll);
    }

    [Obsolete("Not implemented.")]
    public static AxisAngle ToAxisAngle(this Matrix4x4 matrix)
    {
        throw new NotImplementedException();
    }
}
