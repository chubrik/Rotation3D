Formulas reference for converting different rotation representations.
<br><br>

| | To Matrix | To Quaternion | To EulerAngles | To AxisAngle |
| :-- | :-- | :-- | :-- | :-- |
| **Matrix** | | ✅ Native | ✅ Custom \* | ❌ not implemented |
| **Quaternion** | ✅ Native | | ✅ Custom \*\* | ⚠️ Custom<br>(not tested) |
| **EulerAngles** | ✅ Native | ✅ Native | | ❌ not implemented |
| **AxisAngle** | ⚠️ Native<br>(not tested) | ⚠️ Native<br>(not tested) | ❌ not implemented | |

<br>\* Supports a uniformly scaled matrix. Has an additional optimized implementation for the unscaled one.

\** Has an additional optimized implementation for the normalized quaternion.
