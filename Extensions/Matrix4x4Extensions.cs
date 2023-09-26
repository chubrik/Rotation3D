namespace Trigonometry;

using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

public static class Matrix4x4Extensions
{
    public static Quaternion ToQuaternion(this Matrix4x4 matrix)
    {
        return Quaternion.CreateFromRotationMatrix(matrix);
    }

    // https://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToEuler/

    // /** this conversion uses conventions as described on page:
    // *   https://www.euclideanspace.com/maths/geometry/rotations/euler/index.htm
    // *   Coordinate System: right hand
    // *   Positive angle: right hand
    // *   Order of euler angles: heading first, then attitude, then bank
    // *   matrix row column ordering:
    // *   [m00 m01 m02]
    // *   [m10 m11 m12]
    // *   [m20 m21 m22]*/
    // public final void rotate(matrix  m) {
    //     // Assuming the angles are in radians.
    //     if (m.m10 > 0.998) { // singularity at north pole
    //         heading = Math.atan2(m.m02,m.m22);
    //         attitude = Math.PI/2;
    //         bank = 0;
    //         return;
    //     }
    //     if (m.m10 < -0.998) { // singularity at south pole
    //         heading = Math.atan2(m.m02,m.m22);
    //         attitude = -Math.PI/2;
    //         bank = 0;
    //         return;
    //     }
    //     heading = Math.atan2(-m.m20,m.m00);
    //     bank = Math.atan2(-m.m12,m.m11);
    //     attitude = Math.asin(m.m10);
    // }

    // Changes:
    //                  [11  12  13]    [ 33 -23 -13]
    // 1. Flip matrix:  [21  22  23] => [-32  22 -12]
    //                  [31  32  33]    [-31 -21  11]
    //
    // 2. 0.998 => 0.9999986 (< 0.1° from pole)
    public static EulerAngles ToEulerAngles_UnscaledMatrix(this Matrix4x4 matrix)
    {
        Debug.Assert(!matrix.IsScaled());

        var (m11, m12, m13, m22, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M22, matrix.M31, matrix.M32, matrix.M33);

        float yaw, pitch, roll;

        if (m32 < -0.9999986)
        {
            yaw = Atan2(-m13, m11);
            pitch = POSITIVE_HALF_PI;
            roll = 0;
        }
        else if (m32 > 0.9999986)
        {
            yaw = Atan2(-m13, m11);
            pitch = NEGATIVE_HALF_PI;
            roll = 0;
        }
        else
        {
            yaw = Atan2(m31, m33);
            pitch = Asin(-m32);
            roll = Atan2(m12, m22);
        }

        return new EulerAngles(yaw: yaw, pitch: pitch, roll: roll);
    }

    // Similar to the previous one, but supports a uniformly scaled matrix.
    public static EulerAngles ToEulerAngles(this Matrix4x4 matrix)
    {
        Debug.Assert(matrix.HasUniformScale());

        var (m11, m12, m13, m22, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M22, matrix.M31, matrix.M32, matrix.M33);

        var scale = Sqrt(m12 * m12 + m22 * m22 + m32 * m32);
        float yaw, pitch, roll;

        if (m32 < -0.9999986 * scale)
        {
            yaw = Atan2(-m13, m11);
            pitch = POSITIVE_HALF_PI;
            roll = 0;
        }
        else if (m32 > 0.9999986 * scale)
        {
            yaw = Atan2(-m13, m11);
            pitch = NEGATIVE_HALF_PI;
            roll = 0;
        }
        else
        {
            yaw = Atan2(m31, m33);
            pitch = Asin(-m32 / scale);
            roll = Atan2(m12, m22);
        }

        return new EulerAngles(yaw: yaw, pitch: pitch, roll: roll);
    }

    [Obsolete("Not implemented.")]
    public static AxisAngle ToAxisAngle(this Matrix4x4 matrix)
    {
        throw new NotImplementedException();
    }

    // For debug only
    private static bool IsScaled(this Matrix4x4 matrix)
    {
        Matrix4x4.Decompose(matrix, out var scale, out _, out _);

        return Abs(scale.X - 1) >= 0.00000012
            && Abs(scale.Y - 1) >= 0.00000012
            && Abs(scale.Z - 1) >= 0.00000012;
    }

    // For debug only
    private static bool HasUniformScale(this Matrix4x4 matrix)
    {
        Matrix4x4.Decompose(matrix, out var scale, out _, out _);

        return Abs(scale.X - scale.Y) < 0.00000024
            && Abs(scale.Y - scale.Z) < 0.00000024
            && Abs(scale.Z - scale.X) < 0.00000024;
    }
}
