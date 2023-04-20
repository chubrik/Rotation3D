namespace Trigonometry;

using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

internal static class QuaternionExtensions
{
    [Obsolete("Not tested.")]
    public static Matrix4x4 ToMatrix(this Quaternion quaternion)
    {
        return Matrix4x4.CreateFromQuaternion(quaternion);
    }

    // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/
    // Changes: X -> Z, Z -> -X, 0.499 -> 0.4999
    public static EulerAngles ToEulerAngles(this Quaternion quaternion)
    {
        Debug.Assert(quaternion.IsNormalized());

        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var test = x * w - y * z;
        float yaw, pitch, roll;

        if (test > 0.4999) // Singularity at North pole
        {
            yaw = -2 * Atan2(z, w);
            pitch = POSITIVE_HALF_PI;
            roll = 0;
        }
        else if (test < -0.4999) // Singularity at South pole
        {
            yaw = 2 * Atan2(z, w);
            pitch = NEGATIVE_HALF_PI;
            roll = 0;
        }
        else
        {
            yaw = Atan2(2 * (x * z + y * w), 1 - 2 * (x * x + y * y));
            pitch = Asin(2 * test);
            roll = Atan2(2 * (x * y + z * w), 1 - 2 * (x * x + z * z));
        }

        return new EulerAngles(yaw: yaw, pitch: pitch, roll: roll);
    }

    // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/
    // Changes: X -> Z, Z -> -X, 0.499 -> 0.4999
    public static EulerAngles ToEulerAngles_NotNormalizedQuaternion(this Quaternion quaternion)
    {
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var ww = w * w;
        var unit = xx + yy + zz + ww; // if normalised is one, otherwise is correction factor
        var test = x * w - y * z;
        float yaw, pitch, roll;

        if (test > 0.4999 * unit) // Singularity at North pole
        {
            yaw = -2 * Atan2(z, w);
            pitch = POSITIVE_HALF_PI;
            roll = 0;
        }
        else if (test < -0.4999 * unit) // Singularity at South pole
        {
            yaw = 2 * Atan2(z, w);
            pitch = NEGATIVE_HALF_PI;
            roll = 0;
        }
        else
        {
            yaw = Atan2(2 * (x * z + y * w), ww - xx - yy + zz);
            pitch = Asin(2 * test / unit);
            roll = Atan2(2 * (x * y + z * w), ww - xx + yy - zz);
        }

        return new EulerAngles(yaw: yaw, pitch: pitch, roll: roll);
    }

    // https://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle/
    [Obsolete("Not tested.")]
    public static AxisAngle ToAxisAngle(this Quaternion quaternion)
    {
        var (qX, qY, qZ, qW) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        // if w > 1 acos and sqrt will produce errors, this cant happen if quaternion is normalised
        if (qW > 1)
            quaternion.Normalize();

        var angle = 2 * Acos(qW);
        var s = Sqrt(1 - qW * qW); // assuming quaternion normalised then w is less than 1, so term always positive.
        float aX, aY, aZ;

        if (s < 0.001)
        {
            // test to avoid divide by zero, s is always positive due to sqrt
            // if s close to zero then direction of axis not important
            aX = qX; // if it is important that axis is normalised then replace with x=1; y=z=0;
            aY = qY;
            aZ = qZ;
        }
        else
        {
            aX = qX / s; // normalise axis
            aY = qY / s;
            aZ = qZ / s;
        }

        return new AxisAngle(axis: new Vector3(aX, aY, aZ), angle: angle);
    }

    public static bool IsNormalized(this Quaternion quaternion)
    {
        var (x, y, z, w) = (quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        return Abs(x * x + y * y + z * z + w * w - 1) <= 0.00000025;
    }

    public static Quaternion Normalize(this Quaternion quaternion)
    {
        return Quaternion.Normalize(quaternion);
    }
}
