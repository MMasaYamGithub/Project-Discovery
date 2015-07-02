using System;
using UnityEngine;
using System.Collections;


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

            body.AddRelativeForce(Vector3.left * 1 * stats.thrustX, ForceMode.Impulse);
            body.AddRelativeForce(Vector3.up * 1 * stats.thrustY, ForceMode.Impulse);
            body.AddRelativeForce(-Vector3.forward * 1 * stats.thrustZ, ForceMode.Impulse); // Models need to be facing forward, my prototype isn't
            body.AddTorque(-Vector3.forward * 1 * stats.thrustRoll, ForceMode.Impulse);  // Models need to be facing forward, my prototype isn't
            body.AddTorque(Vector3.left * 1 * stats.thrustPitch, ForceMode.Impulse);
            body.AddTorque(Vector3.up * 1 * stats.thrustYaw, ForceMode.Impulse);
        }

        private void ApplyAxis(Axis axis) {
            // TODO: Option to toggle joystick
            string axisName = "Thrust" + axis;
            if (!Input.GetAxis(axisName+"btn").Equals(0)) {
                EventSystem.ApplyThrust(axis, Input.GetAxis(axisName+"btn") * 0.05f);
            }
            if (!Input.GetJoystickNames()[0].Equals("")) {
                EventSystem.SetThrust(axis, Input.GetAxis(axisName));
            }
        }

    }

}
