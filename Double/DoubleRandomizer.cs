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
        var pitch = CreateUnitAngle() / 2.0;
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
        var w = CreateUnitValue();
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
        var w = CreateUnitValue() * scale;
        var quaternion = new DoubleQuaternion(x, y, z, w);
        return quaternion;
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

    private static double CreateFactor()
    {
        var factor = Pow(10, _random.NextDouble() * 6.0 - 3.0);
        return factor;
    }
}
