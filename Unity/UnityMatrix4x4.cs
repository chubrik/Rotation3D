using System.Numerics;

namespace Rotation3D.Unity;

public static class UnityMatrix4x4
{
    //
    // Summary:
    //     Creates a rotation matrix.
    //
    // Parameters:
    //   q:
    public static Matrix4x4 Rotate(Quaternion q)
    {
        float num = q.X * 2f;
        float num2 = q.Y * 2f;
        float num3 = q.Z * 2f;
        float num4 = q.X * num;
        float num5 = q.Y * num2;
        float num6 = q.Z * num3;
        float num7 = q.X * num2;
        float num8 = q.X * num3;
        float num9 = q.Y * num3;
        float num10 = q.W * num;
        float num11 = q.W * num2;
        float num12 = q.W * num3;
        Matrix4x4 result = default(Matrix4x4);
        result.M11 = 2f - (num5 + num6);
        result.M12 = num7 + num12;
        result.M13 = num8 - num11;
        result.M14 = 0f;
        result.M21 = num7 - num12;
        result.M22 = 1f - (num4 + num6);
        result.M23 = num9 + num10;
        result.M24 = 0f;
        result.M31 = num8 + num11;
        result.M32 = num9 - num10;
        result.M33 = 1f - (num4 + num5);
        result.M34 = 0f;
        result.M41 = 0f;
        result.M42 = 0f;
        result.M43 = 0f;
        result.M44 = 1f;
        return result;
    }
}
