namespace Rotation3D;

using System.Diagnostics;
using System.Numerics;
using static MathF;

public static class EulerAnglesFormulas
{
    /// <summary>
    /// ✔ Proved by test: <see cref="Tests.EulerAnglesTests.UnitToMatrix"/>
    /// </summary>
    public static Matrix4x4 UnitToMatrix(this EulerAngles eulerAngles)
    {
        #region Explanations

        // Reference: https://www.euclideanspace.com/maths/geometry/rotations/conversions/eulerToMatrix/
        //
        // /** this conversion uses NASA standard aeroplane conventions as described on page:
        // *   https://www.euclideanspace.com/maths/geometry/rotations/euler/index.htm
        // *   Coordinate System: right hand
        // *   Positive angle: right hand
        // *   Order of euler angles: heading first, then attitude, then bank
        // *   matrix row column ordering:
        // *   [m00 m01 m02]
        // *   [m10 m11 m12]
        // *   [m20 m21 m22]*/
        // public final void rotate(double heading, double attitude, double bank) {
        //     // Assuming the angles are in radians.
        //     double ch = Math.cos(heading);
        //     double sh = Math.sin(heading);
        //     double ca = Math.cos(attitude);
        //     double sa = Math.sin(attitude);
        //     double cb = Math.cos(bank);
        //     double sb = Math.sin(bank);
        //
        //     m00 = ch * ca;
        //     m01 = sh*sb - ch*sa*cb;
        //     m02 = ch*sa*sb + sh*cb;
        //     m10 = sa;
        //     m11 = ca*cb;
        //     m12 = -ca*sb;
        //     m20 = -sh*ca;
        //     m21 = sh*sa*cb + ch*sb;
        //     m22 = -sh*sa*sb + ch*cb;
        // }

        // Changes:
        //                  [00  01  02]    [ 33 -23 -13]
        // 1. Flip matrix:  [10  11  12] => [-32  22 -12]
        //                  [20  21  22]    [-31 -21  11]
        // 2. Flip roll

        #endregion

        Debug.Assert(eulerAngles.IsUnit());
        var (yaw, pitch, roll) = (eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);

        var sy = Sin(yaw);
        var cy = Cos(yaw);
        var sp = Sin(pitch);
        var cp = Cos(pitch);
        var sr = Sin(roll);
        var cr = Cos(roll);

        var sy_sp = sy * sp;
        var cy_sp = cy * sp;

        var matrix = Matrix4x4.Identity;

        matrix.M11 = sy_sp * sr + cy * cr;
        matrix.M12 = cp * sr;
        matrix.M13 = cy_sp * sr - sy * cr;

        matrix.M21 = sy_sp * cr - cy * sr;
        matrix.M22 = cp * cr;
        matrix.M23 = cy_sp * cr + sy * sr;

        matrix.M31 = sy * cp;
        matrix.M32 = -sp;
        matrix.M33 = cy * cp;

        return matrix;
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Quaternion.CreateFromYawPitchRoll(float, float, float)"/>
    /// </summary>
    public static Quaternion UnitToQuaternion(this EulerAngles eulerAngles)
    {
        Debug.Assert(eulerAngles.IsUnit());
        var (yaw, pitch, roll) = (eulerAngles.Yaw, eulerAngles.Pitch, eulerAngles.Roll);

        var halfYaw = yaw * 0.5f;
        var halfPitch = pitch * 0.5f;
        var halfRoll = roll * 0.5f;

        var sy = Sin(halfYaw);
        var cy = Cos(halfYaw);
        var sp = Sin(halfPitch);
        var cp = Cos(halfPitch);
        var sr = Sin(halfRoll);
        var cr = Cos(halfRoll);

        var sy_sp = sy * sp;
        var sy_cp = sy * cp;
        var cy_sp = cy * sp;
        var cy_cp = cy * cp;

        var x = cy_sp * cr + sy_cp * sr;
        var y = sy_cp * cr - cy_sp * sr;
        var z = cy_cp * sr - sy_sp * cr;
        var w = cy_cp * cr + sy_sp * sr;

        return new Quaternion(x, y, z, w);
    }
}
