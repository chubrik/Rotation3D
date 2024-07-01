# Rotation3D
The reference of formulas for converting various 3D rotation formats.

Euler angles are right-handed with YXZ axis priority order,
where Y (up) is yaw, X (right) is pitch, and Z (back) is roll.
<br><br>

## Normalized source

| | To AxisAngle | To EulerAngles | To Matrix4x4 | To Quaternion |
| :-- | :-- | :-- | :-- | :-- |
| **AxisAngle** | &nbsp;– | ⏳ Updating | ✅ Proved | ✅ Proved |
| **EulerAngles** | ❌ Not yet | &nbsp;– | ✅ Proved | ✅ Proved |
| **Matrix4x4** | ❌ Not yet | ✅ Proved | &nbsp;– | ✅ Proved |
| **Quaternion** | ✅ Proved | ⏳ Updating | ✅ Proved | &nbsp;– |

<br>

## Not normalized source

| | To AxisAngle | To EulerAngles | To Matrix4x4 | To Quaternion |
| :-- | :-- | :-- | :-- | :-- |
| **AxisAngle** | &nbsp;– | ❌ Not yet | ❌ Not yet | ❌ Not yet |
| **EulerAngles** | ❌ Not yet | &nbsp;– | ❌ Not yet | ❌ Not yet |
| **Matrix4x4** | ❌ Not yet | ⚠️ Draft | &nbsp;– | ❌ Not yet |
| **Quaternion** | ❌ Not yet | ✅ Proved | ✅ Proved | &nbsp;– |
