using System;
using UnityEngine;
using System.Collections;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    /// <summary>
    /// Holds and updates ship stats
    /// </summary>
    public class ShipStats : MonoBehaviour {

        public float thrustZ { get; private set; }
        public float thrustX { get; private set; }
        public float thrustY { get; private set; }
        public float thrustRoll { get; private set; }
        public float thrustYaw { get; private set; }
        public float thrustPitch { get; private set; }


        public ShipStats() {
        }


        void OnEnable() {
            EventSystem.OnThrustChanged += ApplyThrust;
            EventSystem.OnThrustSet += SetThrust;
        }

        void OnDisable() {
            EventSystem.OnThrustChanged -= ApplyThrust;
            EventSystem.OnThrustSet -= SetThrust;
        }

        /// <summary>
        /// Adds the thrust percentage to the current thrust percentage
        /// </summary>
        /// <param name="axis">Axis to change</param>
        /// <param name="value">Percentage value to add (-1 - 1)</param>
        public void ApplyThrust(Axis axis, float value) {
            ChangeThrust(axis, value, false);
        }

        /// <summary>
        /// Sets the thrust percentage for an axis
        /// </summary>
        /// <param name="axis">Axis to change</param>
        /// <param name="value">Percentage value to set (-1 - 1)</param>
        public void SetThrust(Axis axis, float value) {
            ChangeThrust(axis, value, true);
        }

        /// <summary>
        /// Changes thrust value for the specified axis. If set is false, the value will be added to the current thrust value.
        /// </summary>
        /// <param name="axis">Axis to change</param>
        /// <param name="value">Percentage value to change (-1 - 1)</param>
        /// <param name="set">Whether to set or add the value</param>
        private void ChangeThrust(Axis axis, float value, bool set) {
            string stickTag = "ThrustStick"+axis;
            var thrust = 0f;
            switch (axis) {
                case Axis.Z:
                    thrustZ = Mathf.Clamp((set)? value : thrustZ += value, -1f, 1f);
                    thrust = thrustZ;
                    break;
                case Axis.X:
                    thrustX = Mathf.Clamp((set)? value : thrustX += value, -1f, 1f);
                    thrust = thrustX;
                    break;
                case Axis.Y:
                    thrustY = Mathf.Clamp((set)? value : thrustY += value, -1f, 1f);
                    thrust = thrustY;
                    break;
                case Axis.Roll:
                    thrustRoll = Mathf.Clamp((set)? value : thrustRoll += value, -1f, 1f);
                    thrust = thrustRoll;
                    break;
                case Axis.Yaw:
                    thrustYaw = Mathf.Clamp((set)? value : thrustYaw += value, -1f, 1f);
                    thrust = thrustYaw;
                    break;
                case Axis.Pitch:
                    thrustPitch = Mathf.Clamp((set)? value : thrustPitch += value, -1f, 1f);
                    thrust = thrustPitch;
                    break;

            }
            if (!stickTag.Equals("")) GameObject.FindWithTag(stickTag).transform.localEulerAngles = new Vector3(-56 * thrust, 0, 0);
        }

    }

}
