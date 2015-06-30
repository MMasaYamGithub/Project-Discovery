using UnityEngine;
using System.Collections;

namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    public class EventSystem : MonoBehaviour {

        public delegate void ThrustChanged(float value);
        public static event ThrustChanged OnThrustChange;


        public static void ApplyThrust(float value) {
            if (OnThrustChange != null) OnThrustChange(value);
        }
    }

}
