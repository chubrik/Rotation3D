namespace Rotation3D;

public static class MathFConstants
{
    public const float DEG_TO_RAD = (float)(Math.PI / 180); //  0.0174532924
    public const float RAD_TO_DEG = (float)(180 / Math.PI); // 57.29578
    public const float TWO_PI = (float)(Math.PI * 2);       //  6.28318548
    public const float MINUS_PI = -MathF.PI;                // -3.14159274
    public const float HALF_PI = (float)(Math.PI / 2);      //  1.57079637
    public const float MINUS_HALF_PI = -HALF_PI;            // -1.57079637

    public static readonly float SIN_NEAR_90 = (float)Math.Sin(89.95 * Math.PI / 180); // 0.999999642
    public static readonly float MINUS_SIN_NEAR_90 = -SIN_NEAR_90;

    public static readonly float HALF_SIN_NEAR_90 = (float)(Math.Sin(89.95 * Math.PI / 180) / 2); // 0.499999821
    public static readonly float MINUS_HALF_SIN_NEAR_90 = -HALF_SIN_NEAR_90;
}
