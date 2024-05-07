namespace Rotation3D;

using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

public static class QuaternionFormulas
{
    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Quaternion.Normalize(Quaternion)"/>
    /// </summary>
    public static Quaternion Normalize(this Quaternion quaternion)
    {
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var invLen = 1f / Sqrt(x * x + y * y + z * z + w * w);
        return new Quaternion(x * invLen, y * invLen, z * invLen, w * invLen);
    }

    /// <summary>
    /// ✔ Proved by tests:
    /// <br/><see cref="Tests.QuaternionTests.UnitToAxisAngle_MaxAngle"/>
    /// <br/><see cref="Tests.QuaternionTests.UnitToAxisAngle_MidAngle"/>
    /// <br/><see cref="Tests.QuaternionTests.UnitToAxisAngle_MinAngle"/>
    /// </summary>
    public static AxisAngle UnitToAxisAngle(this Quaternion quaternion)
    {
        /// Reference: <see cref="Legacy.LegacyQuaternionFormulas.UnitToAxisAngle_Legacy(Quaternion)"/>

        Debug.Assert(quaternion.IsUnitAbout());
        var (qX, qY, qZ, qW) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        float x, y, z, angle;

        // New formula for angle <90° to prevent singularity and bad axis normalization
        if (Abs(qW) > 0.707106769f)
        {
            var axisLenSq = qX * qX + qY * qY + qZ * qZ;

            // Angle <3.6e-17° is set to zero with no axis to prevent bad axis normalization
            if (axisLenSq < 1e-37f)
            {
                x = y = z = angle = 0f;
            }
            else
            {
                var axisLen = Sqrt(axisLenSq);
                var axisInvLen = 1f / axisLen;
                x = qX * axisInvLen;
                y = qY * axisInvLen;
                z = qZ * axisInvLen;
                angle = Asin(axisLen) * 2f;

                if (qW < 0f)
                    angle = -angle;
            }
        }
        else
        {
            var invS = 1f / Sqrt(1f - qW * qW);
            x = qX * invS;
            y = qY * invS;
            z = qZ * invS;

            // Negate calculation for angle >180° for more accuracy
            if (qW < 0f)
                angle = Acos(-qW) * -2f;
            else
                angle = Acos(qW) * 2f;
        }

        return new AxisAngle(x, y, z, angle);
    }

    [Obsolete("Need to upgrade")]
    public static EulerAngles UnitToEulerAngles(this Quaternion quaternion)
    {
        #region Explanations

        // Reference: https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/
        //
        // /** assumes q1 is a normalised quaternion */
        //
        // public void set(Quat4d q1) {
        //     double test = q1.x*q1.y + q1.z*q1.w;
        //     if (test > 0.499) { // singularity at north pole
        //         heading = 2 * atan2(q1.x,q1.w);
        //         attitude = Math.PI/2;
        //         bank = 0;
        //         return;
        //     }
        //     if (test < -0.499) { // singularity at south pole
        //         heading = -2 * atan2(q1.x,q1.w);
        //         attitude = - Math.PI/2;
        //         bank = 0;
        //         return;
        //     }
        //     double sqx = q1.x*q1.x;
        //     double sqy = q1.y*q1.y;
        //     double sqz = q1.z*q1.z;
        //     heading = atan2(2*q1.y*q1.w-2*q1.x*q1.z , 1 - 2*sqy - 2*sqz);
        //     attitude = asin(2*test);
        //     bank = atan2(2*q1.x*q1.w-2*q1.y*q1.z , 1 - 2*sqx - 2*sqz)
        // }

        // Changes:
        // 1. Flip axes: X => -Z; Z => X; roll => -roll
        // 2. 0.499 => 0.4999999 (89.966°)
        // 3. Fix yaw in poles: Atan2(-m13, m11)
        // 4. Checked operations order

        #endregion

        Debug.Assert(quaternion.IsUnitAbout());
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var halfSinPitch = w * x - y * z;
        float yaw, pitch, roll;

        if (halfSinPitch > 0.4999999f)
        {
            yaw = Atan2(w * y - x * z, 0.5f - y * y - z * z);
            pitch = F_HALF_PI;
            roll = 0f;
        }
        else if (halfSinPitch < -0.4999999f)
        {
            yaw = Atan2(w * y - x * z, 0.5f - y * y - z * z);
            pitch = -F_HALF_PI;
            roll = 0f;
        }
        else
        {
            var xx = x * x;
            yaw = Atan2(w * y + x * z, 0.5f - xx - y * y);
            pitch = Asin(halfSinPitch * 2f);
            roll = Atan2(w * z + x * y, 0.5f - xx - z * z);
        }

        return new EulerAngles(yaw, pitch, roll);
    }

    /// <summary>
    /// ✔ Proved by tests:
    /// <br/><see cref="Tests.QuaternionTests.ScaledToEulerAngles_MainZone"/>
    /// <br/><see cref="Tests.QuaternionTests.ScaledToEulerAngles_MiddleZone"/>
    /// <br/><see cref="Tests.QuaternionTests.ScaledToEulerAngles_PolarZone"/>
    /// </summary>
    public static EulerAngles ScaledToEulerAngles(this Quaternion quaternion)
    {
        /// Reference: <see cref="Legacy.LegacyQuaternionFormulas.ScaledToEulerAngles_Legacy(Quaternion)"/>

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var ww = w * w;

        var m31 = (w * y + x * z) * 2f;
        var m33 = ww - yy - (xx - zz);
        var s = ww + yy + (xx + zz);
        var halfSinPitch = (w * x - y * z) / s;

        float yaw, pitch, roll;

        // Beyond 64° (20% of the sphere) the pitch error doubles
        if (Abs(halfSinPitch) > 0.45f)
        {
            var invS = 1f / s;
            var normM31 = m31 * invS;
            var normM33 = m33 * invS;
            var xzSinSq = normM31 * normM31 + normM33 * normM33;

            // Closer to 0.022° the yaw and roll error dominates
            if (xzSinSq < 1.47e-7f)
            {
                yaw = Atan2((w * y - x * z) * 2f, ww + xx - yy - zz);
                pitch = halfSinPitch < 0f ? -F_HALF_PI : F_HALF_PI;
                roll = 0f;

                return new EulerAngles(yaw, pitch, roll);
            }

            pitch = F_HALF_PI - Asin(Sqrt(xzSinSq));

            if (halfSinPitch < 0f)
                pitch = -pitch;
        }
        else
            pitch = Asin(halfSinPitch * 2f);

        yaw = Atan2(m31, m33);
        roll = Atan2((w * z + x * y) * 2f, ww - xx + (yy - zz));

        return new EulerAngles(yaw, pitch, roll);
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Matrix4x4.CreateFromQuaternion(Quaternion)"/>
    /// </summary>
    public static Matrix4x4 UnitToMatrix(this Quaternion quaternion)
    {
        // Tuned operations order

        Debug.Assert(quaternion.IsUnitAbout());
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var xx = x * x;
        var yy = y * y;
        var zz = z * z;

        var xy = x * y;
        var xz = x * z;
        var xw = x * w;
        var yz = y * z;
        var yw = y * w;
        var zw = z * w;

        var matrix = Matrix4x4.Identity;

        matrix.M11 = (0.5f - yy - zz) * 2f;
        matrix.M12 = (xy + zw) * 2f;
        matrix.M13 = (xz - yw) * 2f;

        matrix.M21 = (xy - zw) * 2f;
        matrix.M22 = (0.5f - xx - zz) * 2f;
        matrix.M23 = (yz + xw) * 2f;

        matrix.M31 = (xz + yw) * 2f;
        matrix.M32 = (yz - xw) * 2f;
        matrix.M33 = (0.5f - xx - yy) * 2f;

        return matrix;
    }

    /// <summary>
    /// ✔ Proved by test: <see cref="Tests.QuaternionTests.ScaledToMatrix"/>
    /// </summary>
    public static Matrix4x4 ScaledToMatrix(this Quaternion quaternion)
    {
        #region Explanations

        // Reference: https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToMatrix/
        //
        // public final void quatToMatrix(Quat4d q){
        //     double sqw = q.w*q.w;
        //     double sqx = q.x*q.x;
        //     double sqy = q.y*q.y;
        //     double sqz = q.z*q.z;
        // 
        //     // invs (inverse square length) is only required if quaternion is not already normalised
        //     double invs = 1 / (sqx + sqy + sqz + sqw)
        //     m00 = ( sqx - sqy - sqz + sqw)*invs ; // since sqw + sqx + sqy + sqz =1/invs*invs
        //     m11 = (-sqx + sqy - sqz + sqw)*invs ;
        //     m22 = (-sqx - sqy + sqz + sqw)*invs ;
        //     
        //     double tmp1 = q.x*q.y;
        //     double tmp2 = q.z*q.w;
        //     m10 = 2.0 * (tmp1 + tmp2)*invs ;
        //     m01 = 2.0 * (tmp1 - tmp2)*invs ;
        //     
        //     tmp1 = q.x*q.z;
        //     tmp2 = q.y*q.w;
        //     m20 = 2.0 * (tmp1 - tmp2)*invs ;
        //     m02 = 2.0 * (tmp1 + tmp2)*invs ;
        //     tmp1 = q.y*q.z;
        //     tmp2 = q.x*q.w;
        //     m21 = 2.0 * (tmp1 + tmp2)*invs ;
        //     m12 = 2.0 * (tmp1 - tmp2)*invs ;
        // }

        // Changes:
        //                  [00  01  02]    [11  21  31]
        // 1. Flip matrix:  [10  11  12] => [12  22  32]
        //                  [20  21  22]    [13  23  33]
        // 2. Tuned operations order

        #endregion

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var ww = w * w;

        var xy = x * y;
        var xz = x * z;
        var xw = x * w;
        var yz = y * z;
        var yw = y * w;
        var zw = z * w;

        var invS = 1f / (ww + xx + (yy + zz));
        var invSx2 = invS * 2f;

        var matrix = Matrix4x4.Identity;

        matrix.M11 = (ww - zz + (xx - yy)) * invS;
        matrix.M12 = (xy + zw) * invSx2;
        matrix.M13 = (xz - yw) * invSx2;

        matrix.M21 = (xy - zw) * invSx2;
        matrix.M22 = (ww - zz + (yy - xx)) * invS;
        matrix.M23 = (yz + xw) * invSx2;

        matrix.M31 = (xz + yw) * invSx2;
        matrix.M32 = (yz - xw) * invSx2;
        matrix.M33 = (ww - yy + (zz - xx)) * invS;

        return matrix;
    }
}
