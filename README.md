# Rotation3D
The reference of formulas for converting various 3D rotation formats.

Euler angles are right-handed with YXZ axis priority order,
where Y (up) is yaw, X (right) is pitch, and Z (back) is roll.
<br><br>

## Normalized converting

| | To AxisAngle | To EulerAngles | To Matrix4x4 | To Quaternion |
| :-- | :-- | :-- | :-- | :-- |
| **AxisAngle** | &nbsp;– | ✅ Tested | ✅ Tested | ✅ Tested |
| **EulerAngles** | ❌ Soon | &nbsp;– | ✅ Tested | ✅ Tested |
| **Matrix4x4** | ❌ Soon | ✅ Tested | &nbsp;– | ✅ Tested |
| **Quaternion** | ⚠️ Draft | ✅ Tested | ✅ Tested | &nbsp;– |

<br>

## Not normalized converting

| | To AxisAngle | To EulerAngles | To Matrix4x4 | To Quaternion |
| :-- | :-- | :-- | :-- | :-- |
| **AxisAngle** | &nbsp;– | ❌ Soon | ⚠️ Not tested | ❌ Soon |
| **EulerAngles** | ❌ Soon | &nbsp;– | ⚠️ Not tested | ⚠️ Not tested |
| **Matrix4x4** | ❌ Soon | ⚠️ Not tested | &nbsp;– | ⚠️ Not tested |
| **Quaternion** | ⚠️ Draft | ⚠️ Not tested | ⚠️ Not tested | &nbsp;– |
