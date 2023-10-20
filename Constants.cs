namespace Rotation3D;

public static class Constants
{
    public const float DEG_TO_RAD = (float)(Math.PI / 180); //  0.0174532924
    public const float RAD_TO_DEG = (float)(180 / Math.PI); // 57.29578
    public const float TWO_PI = (float)(Math.PI * 2);       //  6.28318548
    public const float MINUS_PI = -MathF.PI;                // -3.14159274
    public const float HALF_PI = (float)(Math.PI / 2);      //  1.57079637
    public const float MINUS_HALF_PI = -HALF_PI;            // -1.57079637

    public const float SIN_89 = 0.9998477f; // (float)Math.Sin(89 * Math.PI / 180);
    public const float SIN_MINUS_89 = -SIN_89;
}
