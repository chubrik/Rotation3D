namespace Rotation3D.Legacy;

using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

[Obsolete("Legacy")]
public static class LegacyMatrix4x4Formulas
{
    public static EulerAngles UnitToEulerAngles_Legacy(this Matrix4x4 matrix)
    {
        #region Explanations

        // Reference: https://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToEuler/
        //
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
        //                  [00  01  02]    [ 33 -23 -13]
        // 1. Flip matrix:  [10  11  12] => [-32  22 -12]
        //                  [20  21  22]    [-31 -21  11]
        //
        // 2. 0.998 => 0.99999994 (89.980°)

        #endregion

        Debug.Assert(matrix.IsUnitAbout());

        var (m11, m12, m13, m22, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M22, matrix.M31, matrix.M32, matrix.M33);

        float yaw, pitch, roll;

        if (m32 < -0.99999994f)
        {
            yaw = Atan2(-m13, m11);
            pitch = F_HALF_PI;
            roll = 0f;
        }
        else if (m32 > 0.99999994f)
        {
            yaw = Atan2(-m13, m11);
            pitch = -F_HALF_PI;
            roll = 0f;
        }
        else
        {
            yaw = Atan2(m31, m33);
            pitch = Asin(-m32);
            roll = Atan2(m12, m22);
        }

        return new EulerAngles(yaw, pitch, roll);
    }
}
