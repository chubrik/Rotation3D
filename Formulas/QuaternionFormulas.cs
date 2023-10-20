namespace Rotation3D;

using Rotation3D.Debugging;
using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

public static class QuaternionFormulas
{
    public static Matrix4x4 ToMatrix(this Quaternion quaternion)
    {
        // Reference: Matrix4x4.CreateFromQuaternion(quaternion);

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

        var m11 = 1f - (yy + zz) * 2f;
        var m12 = (xy + zw) * 2f;
        var m13 = (xz - yw) * 2f;

        var m21 = (xy - zw) * 2f;
        var m22 = 1f - (xx + zz) * 2f;
        var m23 = (xw + yz) * 2f;

        var m31 = (xz + yw) * 2f;
        var m32 = (yz - xw) * 2f;
        var m33 = 1f - (xx + yy) * 2f;

        return new Matrix4x4(m11, m12, m13, 0f, m21, m22, m23, 0f, m31, m32, m33, 0f, 0f, 0f, 0f, 1f);
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
        // 2. 0.499 => SIN_89 (1° from pole)
        // 3. Fix yaw in poles
        // 4. Keep pitch in poles
        // 5. Atan2 reductions

        #endregion

        Debug.Assert(quaternion.IsUnit());

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var sinPitch = (x * w - y * z) * 2f;
        float yaw, pitch, roll;

        if (sinPitch > SIN_89)
        {
            yaw = Atan2(y * w - x * z, 0.5f - y * y - z * z);
            pitch = sinPitch < 1 ? Asin(sinPitch) : HALF_PI;
            roll = 0f;
        }
        else if (sinPitch < SIN_MINUS_89)
        {
            yaw = Atan2(y * w - x * z, 0.5f - y * y - z * z);
            pitch = sinPitch > -1 ? Asin(sinPitch) : MINUS_HALF_PI;
            roll = 0f;
        }
        else
        {
            var xx = x * x;
            yaw = Atan2(x * z + y * w, 0.5f - xx - y * y);
            pitch = Asin(sinPitch);
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
        // 2. 0.499 => SIN_89 (1° from pole)
        // 3. Fix yaw in poles
        // 4. Keep pitch in poles

        #endregion

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var ww = w * w;

        var normSinPitch = (x * w - y * z) * 2f / (xx + yy + zz + ww);
        float yaw, pitch, roll;

        if (normSinPitch > SIN_89)
        {
            yaw = Atan2(y * w - x * z, 0.5f - yy - zz);
            pitch = normSinPitch < 1 ? Asin(normSinPitch) : HALF_PI;
            roll = 0f;
        }
        else if (normSinPitch < SIN_MINUS_89)
        {
            yaw = Atan2(y * w - x * z, 0.5f - yy - zz);
            pitch = normSinPitch > -1 ? Asin(normSinPitch) : MINUS_HALF_PI;
            roll = 0f;
        }
        else
        {
            yaw = Atan2((x * z + y * w) * 2f, ww - xx - yy + zz);
            pitch = Asin(normSinPitch);
            roll = Atan2((x * y + z * w) * 2f, ww - xx + yy - zz);
        }

        return new EulerAngles(yaw, pitch, roll);
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

        if (quaternion.W > 1f) //todo Doubt
            quaternion = quaternion.Normalize();

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

        return new AxisAngle(axis: new Vector3(x, y, z), angle: angle);
    }

    public static Quaternion Normalize(this Quaternion quaternion)
    {
        // Reference: Quaternion.Normalize(quaternion);

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var invNorm = 1f / Sqrt(x * x + y * y + z * z + w * w);
        return new Quaternion(x * invNorm, y * invNorm, z * invNorm, w * invNorm);
    }
}
