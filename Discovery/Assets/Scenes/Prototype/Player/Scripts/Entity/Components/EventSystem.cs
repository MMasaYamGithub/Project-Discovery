using UnityEngine;
using System.Collections;

namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    public class EventSystem : MonoBehaviour {

        public delegate void ThrustChanged(Axis axis, float value);
        public static event ThrustChanged OnThrustChanged;
        public delegate void ThrustSet(Axis axis, float value);
        public static event ThrustSet OnThrustSet;


        public static void ApplyThrust(Axis axis, float value) {
            if (OnThrustChanged != null) OnThrustChanged(axis, value);
        }

        public static void SetThrust(Axis axis, float value) {
            if (OnThrustSet != null) OnThrustSet(axis, value);
        }
    }

}
