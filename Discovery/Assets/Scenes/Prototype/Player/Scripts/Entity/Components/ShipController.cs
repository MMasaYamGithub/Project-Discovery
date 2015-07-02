using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    public enum Axis {
        X, Y, Z, Roll, Yaw, Pitch
    }

    /// <summary>
    /// Controls ship functionality including reacting to stats
    /// </summary>
    [RequireComponent(typeof(ShipStats))]
    [RequireComponent(typeof(Rigidbody))]
    public class ShipController : MonoBehaviour {

        public bool active; // Whether the ship is controllable
        public bool flightAssist = true; // Whether opposite thrust should be applied when an axis is set to 0 to assist manual flight
        public float assistDeadZone = 0.05f; // Threshold for when to apply assist
        private ShipStats stats;
        private Rigidbody body;

        void Start() {
            stats = GetComponent<ShipStats>();
            body = GetComponent<Rigidbody>();
        }

        void Update() {
            if (active) {
                ApplyAxis(Axis.X);
                ApplyAxis(Axis.Y);
                ApplyAxis(Axis.Z);
                ApplyAxis(Axis.Roll);
                ApplyAxis(Axis.Pitch);
                ApplyAxis(Axis.Yaw);
            }

            ApplyThrust(Axis.X, stats.thrustX);
            ApplyThrust(Axis.Y, stats.thrustY);
            ApplyThrust(Axis.Z, stats.thrustZ);
            ApplyThrust(Axis.Roll, stats.thrustRoll);
            ApplyThrust(Axis.Pitch, stats.thrustPitch);
            ApplyThrust(Axis.Yaw, stats.thrustYaw);
        }

        /// <summary>
        /// Applies thrust value to an axis, applying flight assist instead if necessary
        /// </summary>
        /// <param name="axis">Axis to apply thrust to</param>
        /// <param name="thrustValue">Thrust value - use stats.thrustAxis</param>
        private void ApplyThrust(Axis axis, float thrustValue) {
            Vector3 assistForce;
            Vector3 force;
            var rotation = false;
            switch (axis) {
                default:
                case Axis.X:
                    assistForce = Vector3.left * -body.velocity.x;
                    force = Vector3.left * stats.thrustX;
                    break;
                case Axis.Y:
                    assistForce = Vector3.up * -body.velocity.y;
                    force = Vector3.up * stats.thrustY;
                    break;
                case Axis.Z:
                    // Models need to be facing forward, my prototype isn't hence the forward inversion
                    assistForce = -Vector3.forward * -body.velocity.z;
                    force = -Vector3.forward * stats.thrustZ;
                    break;
                case Axis.Roll:
                    // Models need to be facing forward, my prototype isn't hence the forward inversion
                    assistForce = -Vector3.forward * -body.angularVelocity.z;
                    force = -Vector3.forward * stats.thrustRoll;
                    rotation = true;
                    break;
                case Axis.Pitch:
                    assistForce = Vector3.left * -body.angularVelocity.x;
                    force = Vector3.left * stats.thrustPitch;
                    rotation = true;
                    break;
                case Axis.Yaw:
                    assistForce = Vector3.up * -body.angularVelocity.y;
                    force = Vector3.up * stats.thrustYaw;
                    rotation = true;
                    break;
            }
            if (flightAssist && (thrustValue <= assistDeadZone && thrustValue >= -assistDeadZone)) {
                // Apply flight assist
                force = assistForce;
            }

            if (rotation) {
                body.AddRelativeTorque(force, ForceMode.Impulse);
            } else {
                body.AddRelativeForce(force, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// Triggers thrust change event using input axis values
        /// </summary>
        /// <param name="axis">Axis to get value for</param>
        private void ApplyAxis(Axis axis) {
            // TODO: Option to toggle joystick
            string axisName = "Thrust" + axis;
            EventSystem.IncrementThrust(axis, Input.GetAxis(axisName+"btn") * 0.05f);
            if (!Input.GetJoystickNames()[0].Equals("")) {
                EventSystem.SetThrust(axis, Input.GetAxis(axisName));
            }
        }
    }
}
