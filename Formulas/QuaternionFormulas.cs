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

    [Obsolete("Draft")]
    public static AxisAngle UnitToAxisAngle_Draft(this Quaternion quaternion)
    {
        #region Explanations

        // Reference: https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle/
        //
        // public void set(Quat4d q1) {
        //     if (q1.w > 1) q1.normalise(); // if w>1 acos and sqrt will produce errors, this cant happen if quaternion is normalised
        //     angle = 2 * Math.acos(q1.w);
        //     double s = Math.sqrt(1-q1.w*q1.w); // assuming quaternion normalised then w is less than 1, so term always positive.
        //     if (s < 0.001) { // test to avoid divide by zero, s is always positive due to sqrt
        //         // if s close to zero then direction of axis not important
        //         x = q1.x; // if it is important that axis is normalised then replace with x=1; y=z=0;
        //         y = q1.y;
        //         z = q1.z;
        //     } else {
        //         x = q1.x / s; // normalise axis
        //         y = q1.y / s;
        //         z = q1.z / s;
        //     }
        // }

        #endregion

        Debug.Assert(quaternion.IsUnitAbout());
        var (qX, qY, qZ, qW) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var angle = Acos(qW) * 2f;
        var s = Sqrt(1f - qW * qW);
        float x, y, z;

        if (s < 0.001f)
        {
            x = qX;
            y = qY;
            z = qZ;
        }
        else
        {
            var invS = 1f / s;
            x = qX * invS;
            y = qY * invS;
            z = qZ * invS;
        }

        return new AxisAngle(x, y, z, angle);
    }

    /// <summary>
    /// ✔ Proved by tests:
    /// <br/><see cref="Tests.QuaternionTests.UnitToEulerAngles_MainZone"/>
    /// <br/><see cref="Tests.QuaternionTests.UnitToEulerAngles_MiddleZone"/>
    /// <br/><see cref="Tests.QuaternionTests.UnitToEulerAngles_PolarZone"/>
    /// </summary>
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

        #endregion

        // todo Tune operations order

        Debug.Assert(quaternion.IsUnitAbout());
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var halfSinPitch = x * w - y * z;
        float yaw, pitch, roll;

        if (halfSinPitch > 0.4999999f) //todo
        {
            yaw = Atan2(y * w - x * z, 0.5f - y * y - z * z);
            pitch = F_HALF_PI;
            roll = 0f;
        }
        else if (halfSinPitch < -0.4999999f)
        {
            yaw = Atan2(y * w - x * z, 0.5f - y * y - z * z);
            pitch = -F_HALF_PI;
            roll = 0f;
        }
        else
        {
            var xx = x * x;
            yaw = Atan2(x * z + y * w, 0.5f - xx - y * y);
            pitch = Asin(halfSinPitch * 2f);
            roll = Atan2(x * y + z * w, 0.5f - xx - z * z);
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
        #region Explanations

        // Reference: https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/
        //
        // /** q1 can be non-normalised quaternion */
        // 
        // public void set(Quat4d q1) {
        //     double sqw = q1.w*q1.w;
        //     double sqx = q1.x*q1.x;
        //     double sqy = q1.y*q1.y;
        //     double sqz = q1.z*q1.z;
        //     double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
        //     double test = q1.x*q1.y + q1.z*q1.w;
        //     if (test > 0.499*unit) { // singularity at north pole
        //         heading = 2 * atan2(q1.x,q1.w);
        //         attitude = Math.PI/2;
        //         bank = 0;
        //         return;
        //     }
        //     if (test < -0.499*unit) { // singularity at south pole
        //         heading = -2 * atan2(q1.x,q1.w);
        //         attitude = -Math.PI/2;
        //         bank = 0;
        //         return;
        //     }
        //     heading = atan2(2*q1.y*q1.w-2*q1.x*q1.z , sqx - sqy - sqz + sqw);
        //     attitude = asin(2*test/unit);
        //     bank = atan2(2*q1.x*q1.w-2*q1.y*q1.z , -sqx + sqy - sqz + sqw)
        // }

        // Changes:
        // 1. Flip axes: X => -Z; Z => X; roll => -roll
        // 2. 0.499 => 0.499999851 (89.956°)
        // 3. Fix yaw in poles: Atan2(-m13, m11)

        #endregion

        // todo Tune operations order

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var ww = w * w;

        var halfSinPitch = (x * w - y * z) / (xx + yy + zz + ww);
        float yaw, pitch, roll;

        if (halfSinPitch > 0.499999851f) //todo
        {
            yaw = Atan2((y * w - x * z) * 2f, ww + xx - yy - zz);
            pitch = F_HALF_PI;
            roll = 0f;
        }
        else if (halfSinPitch < -0.499999851f)
        {
            yaw = Atan2((y * w - x * z) * 2f, ww + xx - yy - zz);
            pitch = -F_HALF_PI;
            roll = 0f;
        }
        else
        {
            yaw = Atan2((x * z + y * w) * 2f, ww - xx - yy + zz);
            pitch = Asin(halfSinPitch * 2f);
            roll = Atan2((x * y + z * w) * 2f, ww - xx + yy - zz);
        }

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
        matrix.M23 = (xw + yz) * 2f;

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
        matrix.M23 = (xw + yz) * invSx2;

        matrix.M31 = (xz + yw) * invSx2;
        matrix.M32 = (yz - xw) * invSx2;
        matrix.M33 = (ww - yy + (zz - xx)) * invS;

        return matrix;
    }
}
