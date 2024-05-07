namespace Rotation3D.Double;

using static DoubleConstants;
using static Math;

public static class DoubleRandomizer
{
    private static readonly Random _random = new();

    public static DoubleAxisAngle CreateUnitAxisAngle()
    {
        var axis = CreateUnitVector3();
        var angle = CreateUnitAngle();
        var axisAngle = new DoubleAxisAngle(axis, angle);
        return axisAngle;
    }

    public static DoubleAxisAngle CreateUnitAxisAngle(double minAngleDeg, double maxAngleDeg)
    {
        var angleDiffDeg = maxAngleDeg - minAngleDeg;
        var rawAngleDeg = _random.NextDouble() * angleDiffDeg * 2 - angleDiffDeg;
        var angleDeg = rawAngleDeg > 0 ? rawAngleDeg + minAngleDeg : rawAngleDeg - minAngleDeg;
        var angle = angleDeg * DEG_TO_RAD;

        var axis = CreateUnitVector3();
        var axisAngle = new DoubleAxisAngle(axis, angle);
        return axisAngle;
    }

    public static DoubleAxisAngle CreateScaledAxisAngle()
    {
        var axis = CreateScaledVector3();
        var angle = CreateUnitAngle() * CreateFactor();
        var axisAngle = new DoubleAxisAngle(axis, angle);
        return axisAngle;
    }

    public static DoubleEulerAngles CreateUnitEulerAngles()
    {
        var yaw = CreateUnitAngle();
        var pitch = CreateUnitHalfAngle_F();
        var roll = CreateUnitAngle();
        var eulerAngles = new DoubleEulerAngles(yaw, pitch, roll);
        return eulerAngles;
    }

    public static DoubleEulerAngles CreateUnitEulerAngles(double minPitchDeg, double maxPitchDeg)
    {
        var pitchDiffDeg = maxPitchDeg - minPitchDeg;
        var rawPitchDeg = _random.NextDouble() * pitchDiffDeg * 2 - pitchDiffDeg;

        var pitch = rawPitchDeg > 0
            ? Min((rawPitchDeg + minPitchDeg) * DEG_TO_RAD, Constants.F_HALF_PI)
            : Max((rawPitchDeg - minPitchDeg) * DEG_TO_RAD, -Constants.F_HALF_PI);

        var yaw = CreateUnitAngle();
        var roll = CreateUnitAngle();
        var eulerAngles = new DoubleEulerAngles(yaw, pitch, roll);
        return eulerAngles;
    }

    public static DoubleMatrix4x4 CreateUnitMatrix()
    {
        var quaternion = CreateUnitQuaternion();
        var matrix = quaternion.UnitToMatrix();
        return matrix;
    }

    public static DoubleMatrix4x4 CreateScaledMatrix()
    {
        var matrix = CreateUnitMatrix();
        var scaledMatrix = matrix.RandomScale();
        return scaledMatrix;
    }

    private static DoubleMatrix4x4 RandomScale(this DoubleMatrix4x4 matrix)
    {
        var scaleX = CreateFactor();
        var scaleY = CreateFactor();
        var scaleZ = CreateFactor();
        matrix.M11 *= scaleX;
        matrix.M12 *= scaleX;
        matrix.M13 *= scaleX;
        matrix.M21 *= scaleY;
        matrix.M22 *= scaleY;
        matrix.M23 *= scaleY;
        matrix.M31 *= scaleZ;
        matrix.M32 *= scaleZ;
        matrix.M33 *= scaleZ;
        return matrix;
    }

    public static DoubleQuaternion CreateUnitQuaternion()
    {
        var x = CreateUnitValue();
        var y = CreateUnitValue();
        var z = CreateUnitValue();
        var w = Sin(CreateUnitValue());
        var quaternion = new DoubleQuaternion(x, y, z, w);
        var unitQuaternion = quaternion.Normalize();
        return unitQuaternion;
    }

    public static DoubleQuaternion CreateScaledQuaternion()
    {
        var scale = CreateFactor();
        var x = CreateUnitValue() * scale;
        var y = CreateUnitValue() * scale;
        var z = CreateUnitValue() * scale;
        var w = Sin(CreateUnitValue()) * scale;
        var quaternion = new DoubleQuaternion(x, y, z, w);
        return quaternion;
    }

    public static DoubleQuaternion RandomSign(this DoubleQuaternion quaternion)
    {
        var sign = _random.NextSingle();
        return sign < 0.5f ? quaternion : quaternion.Negate();
    }

    public static DoubleQuaternion RandomScaleAndSign(this DoubleQuaternion quaternion)
    {
        var scale = CreateFactor();
        var sign = _random.NextSingle();
        var x = quaternion.X * scale;
        var y = quaternion.Y * scale;
        var z = quaternion.Z * scale;
        var w = quaternion.W * scale;

        if (sign < 0.5f)
            return new DoubleQuaternion(x, y, z, w);
        else
            return new DoubleQuaternion(-x, -y, -z, -w);
    }

    private static DoubleVector3 CreateUnitVector3()
    {
        var x = CreateUnitValue();
        var y = CreateUnitValue();
        var z = CreateUnitValue();
        var vector3 = new DoubleVector3(x, y, z);
        var unitVector3 = vector3.Normalize();
        return unitVector3;
    }

    private static DoubleVector3 CreateScaledVector3()
    {
        var scale = CreateFactor();
        var x = CreateUnitValue() * scale;
        var y = CreateUnitValue() * scale;
        var z = CreateUnitValue() * scale;
        var vector3 = new DoubleVector3(x, y, z);
        return vector3;
    }

    private static double CreateUnitValue()
    {
        var value = _random.NextDouble() * 2.0 - 1.0;
        return value;
    }

    private static double CreateUnitAngle()
    {
        var angle = _random.NextDouble() * TWO_PI - PI;
        return angle;
    }

    private static double CreateUnitHalfAngle_F()
    {
        var angle = _random.NextDouble() * Constants.F_HALF_PI * 2.0 - Constants.F_HALF_PI;
        return angle;
    }

    // 0.000 001 ... 1 000 000
    private static double CreateFactor()
    {
        var factor = Pow(10, _random.NextDouble() * 12.0 - 6.0);
        return factor;
    }
}
