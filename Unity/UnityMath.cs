namespace Rotation3D.Unity;

using System.Runtime.InteropServices;

public static class Mathf
{
    //
    // Summary:
    //     A tiny floating point value (Read Only).
    public static readonly float Epsilon = (MathfInternal.IsFlushToZeroEnabled ? MathfInternal.FloatMinNormal : MathfInternal.FloatMinDenormal);

    //
    // Summary:
    //     Returns square root of f.
    //
    // Parameters:
    //   f:
    public static float Sqrt(float f)
    {
        return (float)Math.Sqrt(f);
    }
}

[StructLayout(LayoutKind.Sequential, Size = 1)]
//[Il2CppEagerStaticClassConstruction]
public struct MathfInternal
{
    public static volatile float FloatMinNormal = 1.17549435E-38f;

    public static volatile float FloatMinDenormal = float.Epsilon;

    public static bool IsFlushToZeroEnabled = FloatMinDenormal == 0f;
}
