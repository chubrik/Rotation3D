namespace Rotation3D;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Tests;
using System.Numerics;
using static MathF;
using static MathFConstants;

public static class QuaternionFormulas
{
    public static Quaternion Normalize(this Quaternion quaternion)
    {
        // Reference: Quaternion.Normalize(quaternion);

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var invNorm = 1f / Sqrt(x * x + y * y + z * z + w * w);
        return new Quaternion(x * invNorm, y * invNorm, z * invNorm, w * invNorm);
    }

    public static AxisAngle ToAxisAngle(this Quaternion quaternion)
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

        Assert.IsTrue(quaternion.IsNormal());
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

    public static EulerAngles ToEulerAngles(this Quaternion quaternion)
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
        // 2. 0.499 => 0.499999821 (0.05° from pole)
        // 3. Fix yaw in poles: Atan2(-m13, m11)

        #endregion

        Assert.IsTrue(quaternion.IsNormal());
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var halfSinPitch = x * w - y * z;
        float yaw, pitch, roll;

        if (halfSinPitch > HALF_SIN_NEAR_90)
        {
            yaw = Atan2(y * w - x * z, 0.5f - y * y - z * z);
            pitch = HALF_PI;
            roll = 0f;
        }
        else if (halfSinPitch < MINUS_HALF_SIN_NEAR_90)
        {
            yaw = Atan2(y * w - x * z, 0.5f - y * y - z * z);
            pitch = MINUS_HALF_PI;
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

    public static EulerAngles ToEulerAngles_NonUnitQuaternion(this Quaternion quaternion)
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
        // 2. 0.499 => 0.499999821 (0.05° from pole)
        // 3. Fix yaw in poles: Atan2(-m13, m11)

        #endregion

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var ww = w * w;

        var normHalfSinPitch = (x * w - y * z) / (xx + yy + zz + ww);
        float yaw, pitch, roll;

        if (normHalfSinPitch > HALF_SIN_NEAR_90)
        {
            yaw = Atan2(y * w - x * z, 0.5f - y * y - z * z);
            pitch = HALF_PI;
            roll = 0f;
        }
        else if (normHalfSinPitch < MINUS_HALF_SIN_NEAR_90)
        {
            yaw = Atan2(y * w - x * z, 0.5f - y * y - z * z);
            pitch = MINUS_HALF_PI;
            roll = 0f;
        }
        else
        {
            yaw = Atan2((x * z + y * w) * 2f, ww - xx - yy + zz);
            pitch = Asin(normHalfSinPitch * 2f);
            roll = Atan2((x * y + z * w) * 2f, ww - xx + yy - zz);
        }

        return new EulerAngles(yaw, pitch, roll);
    }

    public static Matrix4x4 ToMatrix4x4(this Quaternion quaternion)
    {
        // Reference: Matrix4x4.CreateFromQuaternion(quaternion);

        Assert.IsTrue(quaternion.IsNormal());
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

        matrix.M11 = 1f - (yy + zz) * 2f;
        matrix.M12 = (xy + zw) * 2f;
        matrix.M13 = (xz - yw) * 2f;

        matrix.M21 = (xy - zw) * 2f;
        matrix.M22 = 1f - (xx + zz) * 2f;
        matrix.M23 = (xw + yz) * 2f;

        matrix.M31 = (xz + yw) * 2f;
        matrix.M32 = (yz - xw) * 2f;
        matrix.M33 = 1f - (xx + yy) * 2f;

        return matrix;
    }
}
