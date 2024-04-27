namespace Rotation3D.Unity;

using System.Numerics;
using System.Runtime.CompilerServices;

public static class UnityQuaternion
{
    //
    // Summary:
    //     Converts this quaternion to a quaternion with the same orientation but with a
    //     magnitude of 1.0.
    //
    // Parameters:
    //   q:
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion Normalize(Quaternion q)
    {
        float num = Mathf.Sqrt(Dot(q, q));
        if (num < Mathf.Epsilon)
        {
            return identity;
        }

        return new Quaternion(q.X / num, q.Y / num, q.Z / num, q.W / num);
    }

    //
    // Summary:
    //     The dot product between two rotations.
    //
    // Parameters:
    //   a:
    //
    //   b:
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Quaternion a, Quaternion b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
    }

    //
    // Summary:
    //     The identity rotation (Read Only).
    public static Quaternion identity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return identityQuaternion;
        }
    }

    private static readonly Quaternion identityQuaternion = new Quaternion(0f, 0f, 0f, 1f);
}
