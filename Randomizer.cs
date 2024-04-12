namespace Rotation3D;

using Rotation3D.Double;
using System.Numerics;

public static class Randomizer
{
    public static AxisAngle CreateUnitAxisAngle() => DoubleRandomizer.CreateUnitAxisAngle().ToSystem();

    public static AxisAngle CreateScaledAxisAngle() => DoubleRandomizer.CreateScaledAxisAngle().ToSystem();

    public static EulerAngles CreateUnitEulerAngles() => DoubleRandomizer.CreateUnitEulerAngles().ToSystem();

    public static Matrix4x4 CreateUnitMatrix() => DoubleRandomizer.CreateUnitMatrix().ToSystem();

    public static Matrix4x4 CreateScaledMatrix() => DoubleRandomizer.CreateScaledMatrix().ToSystem();

    public static Quaternion CreateUnitQuaternion() => DoubleRandomizer.CreateUnitQuaternion().ToSystem();

    public static Quaternion CreateScaledQuaternion() => DoubleRandomizer.CreateScaledQuaternion().ToSystem();
}
