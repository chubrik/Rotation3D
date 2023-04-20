using System.Numerics;
using Trigonometry;

//var matrix = Matrix4x4.Identity;
//var quaternion = Quaternion.Identity;
//var eulerAngles = EulerAngles.Identity;
//var axisAngle = AxisAngle.Identity;

//matrix.ToQuaternion();
//matrix.ToEulerAngles();
//matrix.ToAxisAngle();

//quaternion.ToMatrix();
//quaternion.ToEulerAngles();
//quaternion.ToEulerAngles_NotNormalizedQuaternion();
//quaternion.ToAxisAngle();
//quaternion.IsNormalized();
//quaternion.Normalize();

//eulerAngles.ToMatrix();
//eulerAngles.ToQuaternion();
//eulerAngles.ToAxisAngle();

//axisAngle.ToMatrix();
//axisAngle.ToQuaternion();
//axisAngle.ToEulerAngles();

var e1 = EulerAngles.CreateFromDegrees(yaw: 60, pitch: 45, roll: 30);
var q1 = e1.ToQuaternion();
var e1a = q1.ToEulerAngles();
var e1b = q1.ToEulerAngles_NotNormalizedQuaternion();

return;

// "euler": {
//     "x": 45,
//     "y": 60,
//     "z": 30
// },
// "localRotation": {
//     "x": 0.43967973954090955,
//     "y": 0.3604234056503559,
//     "z": 0.022260026714733816,
//     "w": 0.8223631719059994
// },
