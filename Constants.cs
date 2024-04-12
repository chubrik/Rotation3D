namespace Rotation3D;

using Rotation3D.Double;

public static class Constants
{
    public const float F_PI = (float)Math.PI;
    public const float F_HALF_PI = (float)DoubleConstants.HALF_PI;
    public const float F_TWO_PI = (float)DoubleConstants.TWO_PI;
    public const float F_DEG_TO_RAD = (float)DoubleConstants.DEG_TO_RAD;
    public const float F_RAD_TO_DEG = (float)DoubleConstants.RAD_TO_DEG;
    public const float F_SIN_NEAR_90 = (float)DoubleConstants.SIN_NEAR_90;
    public const float F_HALF_SIN_NEAR_90 = (float)DoubleConstants.HALF_SIN_NEAR_90;

    public const int TestIterationCount = 10_000_000;

    public const float MatrixUnitMaxDiff = 5.645605E-7f;
    public const float QuaternionUnitMaxDiff = 2.0706733E-7f;
    public const float Vector3UnitMaxDiff = 1.7881393E-7f;

    public static void SelfCheck()
    {
        var checkPi = 3.14159274f;
        var checkHalfPi = 1.57079637f;
        var checkTwoPi = 6.28318548f;
        var checkDegToRad = 0.0174532924f;
        var checkRadToDeg = 57.29578f;
        var checkSinNear90 = 0.999999642f;
        var checkHalfSinNear90 = 0.499999821f;

        if (F_PI != checkPi)
            throw new InvalidOperationException();

        if (F_HALF_PI != checkHalfPi)
            throw new InvalidOperationException();

        if (F_TWO_PI != checkTwoPi)
            throw new InvalidOperationException();

        if (F_DEG_TO_RAD != checkDegToRad)
            throw new InvalidOperationException();

        if (F_RAD_TO_DEG != checkRadToDeg)
            throw new InvalidOperationException();

        if (F_SIN_NEAR_90 != checkSinNear90)
            throw new InvalidOperationException();

        if (F_HALF_SIN_NEAR_90 != checkHalfSinNear90)
            throw new InvalidOperationException();
    }
}
