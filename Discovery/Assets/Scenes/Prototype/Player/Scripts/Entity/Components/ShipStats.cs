using UnityEngine;
using System.Collections;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    public class ShipStats : MonoBehaviour {

        public float zThrust { get; private set; }


        public ShipStats() {
            zThrust = 0;
        }


        void OnEnable() {
            EventSystem.OnThrustChange += ApplyThrust;
        }

        void OnDisable() {
            EventSystem.OnThrustChange -= ApplyThrust;
        }

        public void ApplyThrust(float value) {
            zThrust = Mathf.Clamp(zThrust += value, -1f, 1f);
            GameObject.FindWithTag("ThrustStick").transform.localEulerAngles = new Vector3(-56 * zThrust, 0, 0);
        }

    }

}
