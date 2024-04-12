# Rotation3D
The reference of formulas for converting various 3D rotation formats.

Euler angles are right-handed with YXZ axis priority order,
where Y (up) is yaw, X (right) is pitch, and Z (back) is roll.
<br><br>

## Unit converting

| | To AxisAngle | To EulerAngles | To Matrix4x4 | To Quaternion |
| :-- | :-- | :-- | :-- | :-- |
| **AxisAngle** | &nbsp;– | ✅ Tested | ✅ Tested | ✅ Tested |
| **EulerAngles** | ❌ Not yet | &nbsp;– | ✅ Tested | ✅ Tested |
| **Matrix4x4** | ❌ Not yet | ✅ Tested | &nbsp;– | ✅ Tested |
| **Quaternion** | ⚠️ Draft | ✅ Tested | ✅ Tested | &nbsp;– |

<br>

## Scaled converting

| | To AxisAngle | To EulerAngles | To Matrix4x4 | To Quaternion |
| :-- | :-- | :-- | :-- | :-- |
| **AxisAngle** | &nbsp;– | ❌ Not yet | ❌ Not yet | ❌ Not yet |
| **EulerAngles** | &nbsp;– | &nbsp;– | &nbsp;– | &nbsp;– |
| **Matrix4x4** | ❌ Not yet | ⚠️ Ugly | &nbsp;– | ❌ Not yet |
| **Quaternion** | ❌ Not yet | ✅ Tested | ✅ Tested | &nbsp;– |
