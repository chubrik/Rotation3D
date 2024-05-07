namespace Rotation3D.Legacy;

using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

[Obsolete("Legacy")]
public static class LegacyQuaternionFormulas
{
    public static AxisAngle UnitToAxisAngle_Legacy(this Quaternion quaternion)
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

    public static EulerAngles ScaledToEulerAngles_Legacy(this Quaternion quaternion)
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
        // 4. Tuned operations order

        #endregion

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var ww = w * w;

        var halfSinPitch = (x * w - y * z) / (ww + yy + (xx + zz));
        float yaw, pitch, roll;

        if (halfSinPitch > 0.499999851f)
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
            yaw = Atan2((x * z + y * w) * 2f, ww - yy - (xx - zz));
            pitch = Asin(halfSinPitch * 2f);
            roll = Atan2((x * y + z * w) * 2f, ww - xx + (yy - zz));
        }

        return new EulerAngles(yaw, pitch, roll);
    }
}
