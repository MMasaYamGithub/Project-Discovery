using UnityEngine;
using System.Collections;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    public enum Axis {
        X, Y, Z, Roll, Yaw, Pitch
    }

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
            EventSystem.OnThrustChange += ApplyThrust;
        }

        void OnDisable() {
            EventSystem.OnThrustChange -= ApplyThrust;
        }

        public void ApplyThrust(Axis axis, float value) {
            string stickTag = "ThrustStick"+axis;
            var thrust = 0f;
            switch (axis) {
                case Axis.Z:
                    thrustZ = Mathf.Clamp(thrustZ += value, -1f, 1f);
                    thrust = thrustZ;
                    break;
                case Axis.X:
                    thrustX = Mathf.Clamp(thrustX += value, -1f, 1f);
                    thrust = thrustX;
                    break;
                case Axis.Y:
                    thrustY = Mathf.Clamp(thrustY += value, -1f, 1f);
                    thrust = thrustY;
                    break;
                case Axis.Roll:
                    thrustRoll = Mathf.Clamp(thrustRoll += value, -1f, 1f);
                    thrust = thrustRoll;
                    break;
                case Axis.Yaw:
                    thrustYaw = Mathf.Clamp(thrustYaw += value, -1f, 1f);
                    thrust = thrustYaw;
                    break;
                case Axis.Pitch:
                    thrustPitch = Mathf.Clamp(thrustPitch += value, -1f, 1f);
                    thrust = thrustPitch;
                    break;

            }
            if (!stickTag.Equals("")) GameObject.FindWithTag(stickTag).transform.localEulerAngles = new Vector3(-56 * thrust, 0, 0);
        }

    }

}
