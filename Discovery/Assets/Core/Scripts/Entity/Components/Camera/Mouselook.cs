using UnityEngine;
using System;
using UnityStandardAssets.CrossPlatformInput;


// Edited version of Mouselook Standard Asset to be standalone with parts from http://answers.unity3d.com/questions/29741/mouse-look-script.html
namespace DefinitiveStudios.Discovery.Core.Entity.Components.Camera {

    [Serializable]
    public class Mouselook : MonoBehaviour {

        public UnityEngine.Camera cameraCmp;
        public float xSensitivity = 2f;
        public float ySensitivity = 2f;

        public bool clampVerticalRotation = true;
        public float minimumX = -90f;
        public float maximumX = 90f;

        public bool clampHorizontalRotation = true;
        public float minimumY = -90f;
        public float maximumY = 90f;

        public bool smooth;
        public float smoothTime = 5f;

        private Quaternion cameraTargetRot;
        private Quaternion origRotation;
        private float yRot = 0;
        private float xRot = 0;


        public void Start() {
            cameraTargetRot = cameraCmp.transform.localRotation;
            origRotation = cameraTargetRot;
        }


        private void Update() {
            yRot += CrossPlatformInputManager.GetAxis("Mouse X") * xSensitivity;
            xRot -= CrossPlatformInputManager.GetAxis("Mouse Y") * ySensitivity;  // - for mouse inversion

            //cameraTargetRot *= Quaternion.Euler(-xRot, yRot, 0f);
            //cameraTargetRot *= Quaternion.AngleAxis(yRot, Vector3.up) * Quaternion.AngleAxis(xRot, -Vector3.right);
            //cameraTargetRot = Quaternion.Euler(cameraTargetRot.eulerAngles.x, cameraTargetRot.eulerAngles.y, 0);  // Prevent Z rotation

            if (clampVerticalRotation)
                //cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);
                xRot = ClampAxis(xRot, minimumX, maximumX);
            if (clampHorizontalRotation)
                //cameraTargetRot = ClampAxisY(cameraTargetRot, minimumY, maximumY);
                yRot = ClampAxis(yRot, minimumY, maximumY);

            cameraTargetRot = origRotation * Quaternion.AngleAxis(yRot, Vector3.up) * Quaternion.AngleAxis(xRot, Vector3.right);

            if (smooth) {
                cameraCmp.transform.localRotation = Quaternion.Slerp(cameraCmp.transform.localRotation, cameraTargetRot,
                    smoothTime * Time.deltaTime);
            } else {
                cameraCmp.transform.localRotation = cameraTargetRot;
            }
        }

        float ClampAxis(float angle, float min, float max) {
            if (angle < -360) angle += 360;
            else if (angle > 360) angle -= 360;

            return Mathf.Clamp(angle, min, max);
        }

        /*Quaternion ClampAxisY(Quaternion q, float min, float max) {
            float angleY = q.eulerAngles.y;
            angleY = Mathf.Clamp(angleY, min, max);
            //q.y = Quaternion.Euler(0, angleY, 0).y;
            q.y = Mathf.Tan(Mathf.Deg2Rad * angleY);

            return q;
        }

        Quaternion ClampAxisX(Quaternion q, float min, float max) {
            float angleX = q.eulerAngles.x;
            angleX = Mathf.Clamp(angleX, min, max);
            q.x = Quaternion.Euler(angleX, 0, 0).x;

            return q;
        }*/


        /*Quaternion ClampRotationAroundXAxis(Quaternion q) {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, minimumX, maximumX);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        Quaternion ClampRotationAroundYAxis(Quaternion q) {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
            angleY = Mathf.Clamp(angleY, minimumY, maximumY);
            q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

            return q;
        }*/
    }
}
