namespace Rotation3D.Unity;

using System.Numerics;
using System.Runtime.CompilerServices;

public static class UnityVector3
{
    //
    // Summary:
    //     Returns a normalized vector based on the given vector. The normalized vector
    //     has a magnitude of 1 and is in the same direction as the given vector. Returns
    //     a zero vector If the given vector is too small to be normalized.
    //
    // Parameters:
    //   value:
    //     The vector to be normalized.
    //
    // Returns:
    //     A new vector with the same direction as the original vector but with a magnitude
    //     of 1.0.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Normalize(Vector3 value)
    {
        float num = Magnitude(value);
        if (num > 1E-05f)
        {
            return value / num;
        }

        return zero;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Magnitude(Vector3 vector)
    {
        return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
    }

    //
    // Summary:
    //     Shorthand for writing Vector3(0, 0, 0).
    public static Vector3 zero
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return zeroVector;
        }
    }

    private static readonly Vector3 zeroVector = new Vector3(0f, 0f, 0f);
}
