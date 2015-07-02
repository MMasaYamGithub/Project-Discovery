using UnityEngine;
using System.Collections;

namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    public class EventSystem : MonoBehaviour {

        public delegate void ThrustChanged(Axis axis, float value);
        public static event ThrustChanged OnThrustChange;


        public static void ApplyThrust(Axis axis, float value) {
            if (OnThrustChange != null) OnThrustChange(axis, value);
        }
    }

}
