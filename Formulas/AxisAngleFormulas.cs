namespace Rotation3D;

using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

public static class AxisAngleFormulas
{
    [Obsolete("Need to prove")]
    public static AxisAngle NormalizeSoft(this AxisAngle axisAngle)
    {
        var (x, y, z, angle) = (axisAngle.X, axisAngle.Y, axisAngle.Z, axisAngle.Angle);

        var sqLenAxis = x * x + y * y + z * z;

        if (sqLenAxis == 0)
            return AxisAngle.Zero;

        var invLenAxis = 1f / Sqrt(sqLenAxis);
        var normAngle = angle.NormalizeAngleSoft();
        return new AxisAngle(x: x * invLenAxis, y: y * invLenAxis, z: z * invLenAxis, angle: normAngle);
    }

    [Obsolete("Need to prove")]
    public static EulerAngles UnitToEulerAngles(this AxisAngle axisAngle)
    {
        #region Explanations

        // Reference: https://www.euclideanspace.com/maths/geometry/rotations/conversions/angleToEuler/
        //
        // public void toEuler(double x,double y,double z,double angle) {
        //     double s=Math.sin(angle);
        //     double c=Math.cos(angle);
        //     double t=1-c;
        //     //  if axis is not already normalised then uncomment this
        //     // double magnitude = Math.sqrt(x*x + y*y + z*z);
        //     // if (magnitude==0) throw error;
        //     // x /= magnitude;
        //     // y /= magnitude;
        //     // z /= magnitude;
        //     if ((x*y*t + z*s) > 0.998) { // north pole singularity detected
        //         heading = 2*atan2(x*Math.sin(angle/2),Math.cos(angle/2));
        //         attitude = Math.PI/2;
        //         bank = 0;
        //         return;
        //     }
        //     if ((x*y*t + z*s) < -0.998) { // south pole singularity detected
        //         heading = -2*atan2(x*Math.sin(angle/2),Math.cos(angle/2));
        //         attitude = -Math.PI/2;
        //         bank = 0;
        //         return;
        //     }
        //     heading = Math.atan2(y * s- x * z * t , 1 - (y*y+ z*z ) * t);
        //     attitude = Math.asin(x * y * t + z * s) ;
        //     bank = Math.atan2(x * s - y * z * t , 1 - (x*x + z*z) * t);
        // }

        // Changes:
        // 1. Flip axes: X => -Z; Z => X; roll => -roll
        // 2. 0.998 => 0.999999642 (0.05° from pole)
        // 3. Fix yaw in poles: Atan2(-m13, m11)

        #endregion

        Debug.Assert(axisAngle.IsUnitAbout());
        var (x, y, z, angle) = (axisAngle.X, axisAngle.Y, axisAngle.Z, axisAngle.Angle);

        var sa = Sin(angle);
        var _ca = 1 - Cos(angle);
        var xx = x * x;
        var sinPitch = x * sa - z * y * _ca;
        float yaw, pitch, roll;

        if (sinPitch > F_SIN_NEAR_90)
        {
            yaw = Atan2(y * sa - x * z * _ca, xx + (1f - xx) * (1f - _ca));
            pitch = F_HALF_PI;
            roll = 0f;
        }
        else if (sinPitch < -F_SIN_NEAR_90)
        {
            yaw = Atan2(y * sa - x * z * _ca, xx + (1f - xx) * (1f - _ca));
            pitch = -F_HALF_PI;
            roll = 0f;
        }
        else
        {
            yaw = Atan2(y * sa + x * z * _ca, 1f - (xx + y * y) * _ca);
            pitch = Asin(sinPitch);
            roll = Atan2(z * sa + x * y * _ca, 1f - (xx + z * z) * _ca);
        }

        return new EulerAngles(yaw, pitch, roll);
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Matrix4x4.CreateFromAxisAngle(Vector3, float)"/>
    /// </summary>
    public static Matrix4x4 UnitToMatrix(this AxisAngle axisAngle)
    {
        Debug.Assert(axisAngle.IsUnitAbout());
        var (x, y, z, angle) = (axisAngle.X, axisAngle.Y, axisAngle.Z, axisAngle.Angle);

        var sa = Sin(angle);
        var ca = Cos(angle);
        var xx = x * x;
        var yy = y * y;
        var zz = z * z;
        var xy = x * y;
        var xz = x * z;
        var yz = y * z;

        var xy_caxy = xy - xy * ca;
        var xz_caxz = xz - xz * ca;
        var yz_cayz = yz - yz * ca;
        var xsa = x * sa;
        var ysa = y * sa;
        var zsa = z * sa;

        var matrix = Matrix4x4.Identity;

        matrix.M11 = xx + ca * (1f - xx);
        matrix.M12 = xy_caxy + zsa;
        matrix.M13 = xz_caxz - ysa;

        matrix.M21 = xy_caxy - zsa;
        matrix.M22 = yy + ca * (1f - yy);
        matrix.M23 = yz_cayz + xsa;

        matrix.M31 = xz_caxz + ysa;
        matrix.M32 = yz_cayz - xsa;
        matrix.M33 = zz + ca * (1f - zz);

        return matrix;
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Quaternion.CreateFromAxisAngle(Vector3, float)"/>
    /// </summary>
    public static Quaternion UnitToQuaternion(this AxisAngle axisAngle)
    {
        Debug.Assert(axisAngle.IsUnitAbout());
        var (x, y, z, angle) = (axisAngle.X, axisAngle.Y, axisAngle.Z, axisAngle.Angle);

        var halfAngle = angle * 0.5f;
        var sa = Sin(halfAngle);

        var qX = x * sa;
        var qY = y * sa;
        var qZ = z * sa;
        var qW = Cos(halfAngle);

        return new Quaternion(qX, qY, qZ, qW);
    }
}
