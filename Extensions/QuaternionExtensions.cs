namespace Trigonometry;

using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

public static class QuaternionExtensions
{
    public static Matrix4x4 ToMatrix(this Quaternion quaternion)
    {
        return Matrix4x4.CreateFromQuaternion(quaternion);
    }

    //

    // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/

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
    // 1. X => -Z
    // 2. Z => X
    // 3. 0.499 => 0.49999927 (< 0.1° from pole)
    // 4. yaw => -yaw
    public static EulerAngles ToEulerAngles(this Quaternion quaternion)
    {
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var ww = w * w;
        var unit = xx + yy + zz + ww;
        var test = x * w - y * z;
        float yaw, pitch, roll;

        if (test > 0.49999927f * unit)
        {
            yaw = -2 * Atan2(z, w); //todo Looks bad
            pitch = POSITIVE_HALF_PI;
            roll = 0;
        }
        else if (test < -0.49999927f * unit)
        {
            yaw = 2 * Atan2(z, w); //todo Looks bad
            pitch = NEGATIVE_HALF_PI;
            roll = 0;
        }
        else
        {
            var ww_xx = ww - xx;
            var yy_zz = yy - zz;
            yaw = Atan2(2 * (x * z + y * w), ww_xx - yy_zz);
            pitch = Asin(2 * test / unit);
            roll = Atan2(2 * (x * y + z * w), ww_xx + yy_zz);
        }

        return new EulerAngles(yaw: yaw, pitch: pitch, roll: roll);
    }

    //

    // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/

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
    // 1. X => -Z
    // 2. Z => X
    // 3. 0.499 => 0.49999927 (< 0.1° from pole)
    // 4. Atan2 reductions
    // 5. yaw => -yaw
    public static EulerAngles ToEulerAngles_NormalizedQuaternion(this Quaternion quaternion)
    {
        Debug.Assert(quaternion.IsNormalized());

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var test = x * w - y * z;
        float yaw, pitch, roll;

        if (test > 0.49999927f)
        {
            yaw = -2 * Atan2(z, w); //todo Looks bad
            pitch = POSITIVE_HALF_PI;
            roll = 0;
        }
        else if (test < -0.49999927f)
        {
            yaw = 2 * Atan2(z, w); //todo Looks bad
            pitch = NEGATIVE_HALF_PI;
            roll = 0;
        }
        else
        {
            var xx = x * x;
            yaw = Atan2(x * z + y * w, 0.5f - xx - y * y);
            pitch = Asin(2 * test);
            roll = Atan2(x * y + z * w, 0.5f - xx - z * z);
        }

        return new EulerAngles(yaw: yaw, pitch: pitch, roll: roll);
    }

    //

    // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle/

    //  public void set(Quat4d q1) {
    //    if (q1.w > 1) q1.normalise(); // if w>1 acos and sqrt will produce errors, this cant happen if quaternion is normalised
    //    angle = 2 * Math.acos(q1.w);
    //    double s = Math.sqrt(1-q1.w*q1.w); // assuming quaternion normalised then w is less than 1, so term always positive.
    //    if (s < 0.001) { // test to avoid divide by zero, s is always positive due to sqrt
    //      // if s close to zero then direction of axis not important
    //      x = q1.x; // if it is important that axis is normalised then replace with x=1; y=z=0;
    //      y = q1.y;
    //      z = q1.z;
    //    } else {
    //      x = q1.x / s; // normalise axis
    //      y = q1.y / s;
    //      z = q1.z / s;
    //    }
    // }

    [Obsolete("Not tested.")]
    public static AxisAngle ToAxisAngle(this Quaternion quaternion)
    {
        var (qX, qY, qZ, qW) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        if (qW > 1)
            quaternion.Normalize();

        var angle = 2 * Acos(qW);
        var s = Sqrt(1 - qW * qW);
        float aX, aY, aZ;

        if (s < 0.001)
        {
            aX = qX;
            aY = qY;
            aZ = qZ;
        }
        else
        {
            aX = qX / s;
            aY = qY / s;
            aZ = qZ / s;
        }

        return new AxisAngle(axis: new Vector3(aX, aY, aZ), angle: angle);
    }

    public static Quaternion Normalize(this Quaternion quaternion)
    {
        return Quaternion.Normalize(quaternion);
    }

    // For debug only
    public static bool IsNormalized(this Quaternion quaternion)
    {
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        return Abs(x * x + y * y + z * z + w * w - 1) <= 0.00000048;
    }
}
