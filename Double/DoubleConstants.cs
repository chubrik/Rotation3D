﻿namespace Rotation3D.Double;

using static Math;

public static class DoubleConstants
{
    public const double HALF_PI = PI * 0.5; // < float
    public const double TWO_PI = PI * 2.0; // < float
    public const double ONE_AND_HALF_PI = PI * 1.5;
    public const double DEG_TO_RAD = PI / 180.0; // > float
    public const double RAD_TO_DEG = 180.0 / PI; // < float

    public static void SelfCheck()
    {
        var checkDoublePi = 3.1415926535897931;
        var checkDoubleHalfPi = 1.5707963267948966;
        var checkDoubleTwoPi = 6.2831853071795862;
        var checkDoubleDegToRad = 0.017453292519943295;
        var checkDoubleRadToDeg = 57.295779513082323;

        if (PI != checkDoublePi)
            throw new InvalidOperationException();

        if (HALF_PI != checkDoubleHalfPi)
            throw new InvalidOperationException();

        if (TWO_PI != checkDoubleTwoPi)
            throw new InvalidOperationException();

        if (DEG_TO_RAD != checkDoubleDegToRad)
            throw new InvalidOperationException();

        if (RAD_TO_DEG != checkDoubleRadToDeg)
            throw new InvalidOperationException();
    }
}
