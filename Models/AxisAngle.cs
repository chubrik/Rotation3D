namespace Trigonometry;

using System.Diagnostics;
using System.Numerics;
using static Constants;

[DebuggerDisplay("x: {Axis.X}, y: {Axis.Y}, z: {Axis.Z}, angle: {AngleDegrees}°")]
internal readonly struct AxisAngle
{
    public static AxisAngle Identity = default;

    public readonly Vector3 Axis;
    public readonly float Angle;

    public AxisAngle(Vector3 axis, float angle)
    {
        Debug.Assert(axis.IsNormalized());

        Axis = axis;
        Angle = angle;
    }

    public float AngleDegrees => Angle * RAD_TO_DEG;

    public static AxisAngle CreateFromDegrees(Vector3 axis, float angle)
    {
        Debug.Assert(axis.IsNormalized());
        return new AxisAngle(axis: axis, angle: angle * DEG_TO_RAD);
    }
}
