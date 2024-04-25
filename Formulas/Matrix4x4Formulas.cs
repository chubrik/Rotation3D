namespace Rotation3D;

using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

public static class Matrix4x4Formulas
{
    /// <summary>
    /// ✔ Proved by tests:
    /// <br/><see cref="Tests.Matrix4x4Tests.UnitToEulerAngles_MainZone"/>
    /// <br/><see cref="Tests.Matrix4x4Tests.UnitToEulerAngles_MiddleZone"/>
    /// <br/><see cref="Tests.Matrix4x4Tests.UnitToEulerAngles_PolarZone"/>
    /// </summary>
    public static EulerAngles UnitToEulerAngles(this Matrix4x4 matrix)
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
        // 2. 0.998 => 0.999999642 (0.05° from pole)

        #endregion

        Debug.Assert(matrix.IsUnitAbout());

        var (m11, m12, m13, m22, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M22, matrix.M31, matrix.M32, matrix.M33);

        float yaw, pitch, roll;

        if (m32 < -0.99999994f) // 89.980°
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

    [Obsolete("Draft")]
    public static EulerAngles ScaledToEulerAngles_Draft(this Matrix4x4 matrix)
    {
        // Similar to the previous one, but supports a scaled matrix.

        var (m11, m12, m13, m21, m22, m23, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M21, matrix.M22, matrix.M23, matrix.M31, matrix.M32, matrix.M33);

        var sinPitch = -m32 / Sqrt(m31 * m31 + m32 * m32 + m33 * m33); // forward length
        float yaw, pitch, roll;

        if (sinPitch > F_SIN_NEAR_90_UNAPPROVED)
        {
            yaw = Atan2(-m13, m11);
            pitch = F_HALF_PI;
            roll = 0f;
        }
        else if (sinPitch < -F_SIN_NEAR_90_UNAPPROVED)
        {
            yaw = Atan2(-m13, m11);
            pitch = -F_HALF_PI;
            roll = 0f;
        }
        else
        {
            yaw = Atan2(m31, m33);
            pitch = Asin(sinPitch);

            roll = Atan2(
                m12 / Sqrt(m11 * m11 + m12 * m12 + m13 * m13), // right length
                m22 / Sqrt(m21 * m21 + m22 * m22 + m23 * m23)  // up length
            );
        }

        return new EulerAngles(yaw, pitch, roll);
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Quaternion.CreateFromRotationMatrix(Matrix4x4)"/>
    /// </summary>
    public static Quaternion UnitToQuaternion(this Matrix4x4 matrix)
    {
        Debug.Assert(matrix.IsUnitAbout());

        var (m11, m12, m13, m21, m22, m23, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M21, matrix.M22, matrix.M23, matrix.M31, matrix.M32, matrix.M33);

        var trace = m11 + m22 + m33;
        float x, y, z, w;

        if (trace > 0f)
        {
            w = Sqrt(1f + trace) * 0.5f;
            var invS = 0.25f / w;
            x = (m23 - m32) * invS;
            y = (m31 - m13) * invS;
            z = (m12 - m21) * invS;
        }
        else if (m11 >= m22 && m11 >= m33)
        {
            x = Sqrt(1f + m11 - m22 - m33) * 0.5f;
            var invS = 0.25f / x;
            y = (m12 + m21) * invS;
            z = (m13 + m31) * invS;
            w = (m23 - m32) * invS;
        }
        else if (m22 > m33)
        {
            y = Sqrt(1f + m22 - m11 - m33) * 0.5f;
            var invS = 0.25f / y;
            x = (m21 + m12) * invS;
            z = (m32 + m23) * invS;
            w = (m31 - m13) * invS;
        }
        else
        {
            z = Sqrt(1f + m33 - m11 - m22) * 0.5f;
            var invS = 0.25f / z;
            x = (m31 + m13) * invS;
            y = (m32 + m23) * invS;
            w = (m12 - m21) * invS;
        }

        return new Quaternion(x, y, z, w);
    }
}
