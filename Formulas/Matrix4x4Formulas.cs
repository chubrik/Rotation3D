namespace Rotation3D;

using System.Diagnostics;
using System.Numerics;
using static Constants;
using static MathF;

public static class Matrix4x4Formulas
{
    /// <summary>
    /// ✔ Proved by tests:
    /// <br/><see cref="Tests.Matrix4x4Tests.UnitToEulerAngles_MainZone"/>
    /// <br/><see cref="Tests.Matrix4x4Tests.UnitToEulerAngles_MiddleZone"/>
    /// <br/><see cref="Tests.Matrix4x4Tests.UnitToEulerAngles_PolarZone"/>
    /// </summary>
    public static EulerAngles UnitToEulerAngles(this Matrix4x4 matrix)
    {
        /// Reference: <see cref="Legacy.LegacyMatrix4x4Formulas.UnitToEulerAngles_Legacy(Matrix4x4)"/>

        Debug.Assert(matrix.IsUnitAbout());

        var (m11, m12, m13, m22, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M22, matrix.M31, matrix.M32, matrix.M33);

        float yaw, pitch, roll;

        // Beyond 80° (3% of the sphere) the pitch error doubles
        if (Abs(m32) > 0.9848077f)
        {
            var xzSinSq = m31 * m31 + m33 * m33;

            // Closer to 1.2e-5° the yaw and roll error dominates
            if (xzSinSq < 4.5e-14f)
            {
                yaw = Atan2(-m13, m11);
                pitch = m32 > 0f ? -F_HALF_PI : F_HALF_PI;
                roll = 0f;

                return new EulerAngles(yaw, pitch, roll);
            }

            pitch = F_HALF_PI - Asin(Sqrt(xzSinSq));

            if (m32 > 0f)
                pitch = -pitch;
        }
        else
            pitch = Asin(-m32);

        yaw = Atan2(m31, m33);
        roll = Atan2(m12, m22);

        return new EulerAngles(yaw, pitch, roll);
    }

    [Obsolete("Draft")]
    public static EulerAngles ScaledToEulerAngles_Draft(this Matrix4x4 matrix)
    {
        var (m11, m12, m13, m21, m22, m23, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M21, matrix.M22, matrix.M23, matrix.M31, matrix.M32, matrix.M33);

        var sinPitch = -m32 / Sqrt(m31 * m31 + m32 * m32 + m33 * m33); // forward length
        float yaw, pitch, roll;

        if (sinPitch > 0.999999642f) //todo
        {
            yaw = Atan2(-m13, m11);
            pitch = F_HALF_PI;
            roll = 0f;
        }
        else if (sinPitch < -0.999999642f)
        {
            yaw = Atan2(-m13, m11);
            pitch = -F_HALF_PI;
            roll = 0f;
        }
        else
        {
            yaw = Atan2(m31, m33);
            pitch = Asin(sinPitch);

            roll = Atan2(
                m12 / Sqrt(m11 * m11 + m12 * m12 + m13 * m13), // right length
                m22 / Sqrt(m21 * m21 + m22 * m22 + m23 * m23)  // up length
            );
        }

        return new EulerAngles(yaw, pitch, roll);
    }

    /// <summary>
    /// ✔ Proved by Microsoft: <see cref="Quaternion.CreateFromRotationMatrix(Matrix4x4)"/>
    /// </summary>
    public static Quaternion UnitToQuaternion(this Matrix4x4 matrix)
    {
        // Tuned operations order

        Debug.Assert(matrix.IsUnitAbout());

        var (m11, m12, m13, m21, m22, m23, m31, m32, m33) =
            (matrix.M11, matrix.M12, matrix.M13, matrix.M21, matrix.M22, matrix.M23, matrix.M31, matrix.M32, matrix.M33);

        var trace = m11 + m22 + m33;
        float x, y, z, w;

        if (trace > 0f)
        {
            w = Sqrt(trace + 1f) * 0.5f;
            var invS = 0.25f / w;
            x = (m23 - m32) * invS;
            y = (m31 - m13) * invS;
            z = (m12 - m21) * invS;
        }
        else if (m11 >= m22 && m11 >= m33)
        {
            x = Sqrt(m11 - m22 - m33 + 1f) * 0.5f;
            var invS = 0.25f / x;
            y = (m12 + m21) * invS;
            z = (m31 + m13) * invS;
            w = (m23 - m32) * invS;
        }
        else if (m22 > m33)
        {
            y = Sqrt(m22 - m33 - m11 + 1f) * 0.5f;
            var invS = 0.25f / y;
            x = (m12 + m21) * invS;
            z = (m23 + m32) * invS;
            w = (m31 - m13) * invS;
        }
        else
        {
            z = Sqrt(m33 - m11 - m22 + 1f) * 0.5f;
            var invS = 0.25f / z;
            x = (m31 + m13) * invS;
            y = (m23 + m32) * invS;
            w = (m12 - m21) * invS;
        }

        return new Quaternion(x, y, z, w);
    }
}
