namespace Rotation3D;

using System.Numerics;
using static Constants;

public readonly struct AxisAngle
{
    public static AxisAngle Identity { get; } = new(x: 1f, y: 0f, z: 0f, angle: 0f);

    public readonly Vector3 Axis;
    public readonly float Angle;

    public readonly float X => Axis.X;
    public readonly float Y => Axis.Y;
    public readonly float Z => Axis.Z;
    public readonly float AngleDegrees => Angle * F_RAD_TO_DEG;

    public AxisAngle(Vector3 axis, float angle)
    {
        Axis = axis;
        Angle = angle;
    }

    public AxisAngle(float x, float y, float z, float angle)
        : this(new Vector3(x, y, z), angle)
    { }

    public static AxisAngle FromDegrees(Vector3 axis, float angle)
    {
        return new AxisAngle(axis, angle * F_DEG_TO_RAD);
    }

    public static AxisAngle FromDegrees(float x, float y, float z, float angle)
    {
        return new AxisAngle(x, y, z, angle * F_DEG_TO_RAD);
    }

    public override readonly string ToString()
    {
        return $"{{X:{X} Y:{Y} Z:{Z} ANGLE:{Angle}}} {{ANGLE:{AngleDegrees}°}}";
    }
}
