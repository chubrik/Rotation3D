namespace Rotation3D;

using System.Numerics;
using static MathFConstants;

public struct AxisAngle
{
    private static readonly AxisAngle _identity = new(x: 1f, y: 0f, z: 0f, angle: 0f);
    public static AxisAngle Identity => _identity;

    public Vector3 Axis;

    public float Angle;

    public AxisAngle(Vector3 axis, float angle)
    {
        Axis = axis;
        Angle = angle;
    }

    public AxisAngle(float x, float y, float z, float angle)
    {
        Axis = new Vector3(x, y, z);
        Angle = angle;
    }

    public readonly float AngleDegrees => Angle * RAD_TO_DEG;

    public static AxisAngle CreateFromDegrees(Vector3 axis, float angle)
    {
        return new AxisAngle(axis, angle * DEG_TO_RAD);
    }

    public static AxisAngle CreateFromDegrees(float x, float y, float z, float angle)
    {
        return new AxisAngle(x, y, z, angle * DEG_TO_RAD);
    }

    public override readonly string ToString()
    {
        return $"{{X:{Axis.X} Y:{Axis.Y} Z:{Axis.Z} ANGLE:{Angle}}} {{ANGLE:{AngleDegrees}°}}";
    }
}
