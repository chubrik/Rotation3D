namespace Rotation3D;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Tests;
using System.Numerics;
using static MathF;
using static MathFConstants;

public static class Matrix4x4Formulas
{
    public static EulerAngles ToEulerAngles(this Matrix4x4 matrix)
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

        Assert.IsTrue(matrix.IsNormal());

        var (m11, m12, m13, m22, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M22, matrix.M31, matrix.M32, matrix.M33);

        float yaw, pitch, roll;

        if (m32 < MINUS_SIN_NEAR_90)
        {
            yaw = Atan2(-m13, m11);
            pitch = HALF_PI;
            roll = 0f;
        }
        else if (m32 > SIN_NEAR_90)
        {
            yaw = Atan2(-m13, m11);
            pitch = MINUS_HALF_PI;
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

    public static EulerAngles ToEulerAngles_ScaledMatrix(this Matrix4x4 matrix)
    {
        // Similar to the previous one, but supports a uniformly scaled matrix.

        Assert.IsTrue(matrix.IsUniformScaled());

        var (m11, m12, m13, m22, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M22, matrix.M31, matrix.M32, matrix.M33);

        var normM32 = m32 / Sqrt(m12 * m12 + m22 * m22 + m32 * m32);
        float yaw, pitch, roll;

        if (normM32 < MINUS_SIN_NEAR_90)
        {
            yaw = Atan2(-m13, m11);
            pitch = HALF_PI;
            roll = 0f;
        }
        else if (normM32 > SIN_NEAR_90)
        {
            yaw = Atan2(-m13, m11);
            pitch = MINUS_HALF_PI;
            roll = 0f;
        }
        else
        {
            yaw = Atan2(m31, m33);
            pitch = Asin(-normM32);
            roll = Atan2(m12, m22);
        }

        return new EulerAngles(yaw, pitch, roll);
    }

    public static Quaternion ToQuaternion(this Matrix4x4 matrix)
    {
        // Reference: Quaternion.CreateFromRotationMatrix(matrix);

        Assert.IsTrue(matrix.IsNormal());

        var (m11, m12, m13, m21, m22, m23, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M21, matrix.M22, matrix.M23, matrix.M31, matrix.M32, matrix.M33);

        var trace = m11 + m22 + m33;
        float qX, qY, qZ, qW;

        if (trace > 0f)
        {
            qW = Sqrt(1f + trace) * 0.5f;
            var invS = 0.25f / qW;
            qX = (m23 - m32) * invS;
            qY = (m31 - m13) * invS;
            qZ = (m12 - m21) * invS;
        }
        else if (m11 >= m22 && m11 >= m33)
        {
            qX = Sqrt(1f + m11 - m22 - m33) * 0.5f;
            var invS = 0.25f / qX;
            qY = (m12 + m21) * invS;
            qZ = (m13 + m31) * invS;
            qW = (m23 - m32) * invS;
        }
        else if (m22 > m33)
        {
            qY = Sqrt(1f + m22 - m11 - m33) * 0.5f;
            var invS = 0.25f / qY;
            qX = (m21 + m12) * invS;
            qZ = (m32 + m23) * invS;
            qW = (m31 - m13) * invS;
        }
        else
        {
            qZ = Sqrt(1f + m33 - m11 - m22) * 0.5f;
            var invS = 0.25f / qZ;
            qX = (m31 + m13) * invS;
            qY = (m32 + m23) * invS;
            qW = (m12 - m21) * invS;
        }

        return new Quaternion(qX, qY, qZ, qW);
    }
}
