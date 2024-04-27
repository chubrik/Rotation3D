namespace Rotation3D.Unity;

using System.Numerics;

public static class UnityEulerAngles
{
    // Source: UnityEngine.Quaternion.Internal_MakePositive()
    public static Vector3 Internal_MakePositive(Vector3 euler)
    {
        float num = -0.005729578f; // 0.0001 rad
        float num2 = 360f + num;
        if (euler.X < num)
        {
            euler.X += 360f;
        }
        else if (euler.X > num2)
        {
            euler.X -= 360f;
        }

        if (euler.Y < num)
        {
            euler.Y += 360f;
        }
        else if (euler.Y > num2)
        {
            euler.Y -= 360f;
        }

        if (euler.Z < num)
        {
            euler.Z += 360f;
        }
        else if (euler.Z > num2)
        {
            euler.Z -= 360f;
        }

        return euler;
    }
}
