namespace Rotation3D;

using System.Diagnostics;
using System.Numerics;
using static Constants;

[DebuggerDisplay("x: {Axis.X}, y: {Axis.Y}, z: {Axis.Z}, angle: {AngleDegrees}°")]
public struct AxisAngle
{
    private static readonly AxisAngle _identity = new AxisAngle(new Vector3(1f, 0f, 0f), 0f);
    public static AxisAngle Identity => _identity;

    public Vector3 Axis;

    public float Angle;

    public AxisAngle(Vector3 axis, float angle)
    {
        Axis = axis;
        Angle = angle;
    }

    public readonly float AngleDegrees => Angle * RAD_TO_DEG;

    public static AxisAngle CreateFromDegrees(Vector3 axis, float angle)
    {
        return new AxisAngle(axis, angle * DEG_TO_RAD);
    }
}
