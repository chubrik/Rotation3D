namespace Rotation3D.Double;

using System.Diagnostics;
using System.Numerics;
using static DoubleConstants;
using static Math;

public readonly struct DoubleEulerAngles
{
    public static readonly DoubleEulerAngles Identity = default;

    /// <summary>Aka heading, azimuth, Y axis. Applied first.</summary>
    public readonly double Yaw;

    /// <summary>Aka attitude, elevation, X axis. Applied second.</summary>
    public readonly double Pitch;

    /// <summary>Aka bank, tilt, Z axis. Applied last.</summary>
    public readonly double Roll;

    public readonly double YawDegrees => Yaw * RAD_TO_DEG;
    public readonly double PitchDegrees => Pitch * RAD_TO_DEG;
    public readonly double RollDegrees => Roll * RAD_TO_DEG;

    public DoubleEulerAngles(double yaw, double pitch, double roll)
    {
        Yaw = yaw;
        Pitch = pitch;
        Roll = roll;
    }

    public static DoubleEulerAngles FromDegrees(double yaw, double pitch, double roll)
    {
        return new DoubleEulerAngles(yaw * DEG_TO_RAD, pitch * DEG_TO_RAD, roll * DEG_TO_RAD);
    }

    public bool IsValid_F() => Pitch >= -Constants.F_HALF_PI && Pitch <= Constants.F_HALF_PI;

    public bool IsUnit_F() => IsValid_F() && Yaw.IsUnitAngle_F() && Roll.IsUnitAngle_F();

    [Obsolete("Need to prove")]
    public DoubleEulerAngles NormalizeInvalidHard()
    {
        var normPitch = Pitch;

        if (normPitch < -HALF_PI)
            do normPitch += TWO_PI;
            while (normPitch < -HALF_PI);
        else if (normPitch > ONE_AND_HALF_PI)
            do normPitch -= TWO_PI;
            while (normPitch > ONE_AND_HALF_PI);

        var yaw = Yaw;
        var roll = Roll;

        if (normPitch > HALF_PI)
        {
            yaw += PI;
            normPitch = PI - normPitch;
            roll += PI;
        }

        var normYaw = yaw.NormalizeAngleHard();
        var normRoll = roll.NormalizeAngleHard();
        return new DoubleEulerAngles(normYaw, normPitch, normRoll);
    }

    [Obsolete("Need to prove")]
    public DoubleEulerAngles NormalizeValidHard()
    {
        Debug.Assert(IsValid_F());
        var normYaw = Yaw.NormalizeAngleHard();
        var normRoll = Roll.NormalizeAngleHard();
        return new DoubleEulerAngles(normYaw, Pitch, normRoll);
    }

    [Obsolete("Need to prove")]
    public DoubleAxisAngle UnitToAxisAngle()
    {
        Debug.Assert(IsUnit_F());
        throw new NotImplementedException();
    }

    /// <summary>
    /// ✔ Proved by test: <see cref="Tests.EulerAnglesTests.UnitToMatrix"/>
    /// </summary>
    public DoubleMatrix4x4 UnitToMatrix()
    {
        Debug.Assert(IsUnit_F());

        var sy = Sin(Yaw);
        var cy = Cos(Yaw);
        var sp = Sin(Pitch);
        var cp = Cos(Pitch);
        var sr = Sin(Roll);
        var cr = Cos(Roll);

        var sysp = sy * sp;
        var cysp = cy * sp;

        var matrix = DoubleMatrix4x4.Identity;

        matrix.M11 = sysp * sr + cy * cr;
        matrix.M12 = cp * sr;
        matrix.M13 = cysp * sr - sy * cr;

        matrix.M21 = sysp * cr - cy * sr;
        matrix.M22 = cp * cr;
        matrix.M23 = cysp * cr + sy * sr;

        matrix.M31 = sy * cp;
        matrix.M32 = -sp;
        matrix.M33 = cy * cp;

        return matrix;
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Quaternion.CreateFromYawPitchRoll(float, float, float)"/>
    /// </summary>
    public DoubleQuaternion UnitToQuaternion()
    {
        Debug.Assert(IsUnit_F());

        var halfYaw = Yaw * 0.5;
        var halfPitch = Pitch * 0.5;
        var halfRoll = Roll * 0.5;

        var sy = Sin(halfYaw);
        var cy = Cos(halfYaw);
        var sp = Sin(halfPitch);
        var cp = Cos(halfPitch);
        var sr = Sin(halfRoll);
        var cr = Cos(halfRoll);

        var x = cy * sp * cr + sy * cp * sr;
        var y = sy * cp * cr - cy * sp * sr;
        var z = cy * cp * sr - sy * sp * cr;
        var w = cy * cp * cr + sy * sp * sr;

        return new DoubleQuaternion(x, y, z, w);
    }
}
