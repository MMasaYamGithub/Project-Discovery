using UnityEngine;
using System.Collections;

namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    public class EventSystem : MonoBehaviour {

        public delegate void ThrustIncremented(Axis axis, float value);
        public static event ThrustIncremented OnThrustIncremented;

        public delegate void ThrustSet(Axis axis, float value);
        public static event ThrustSet OnThrustSet;

        public delegate void ThrustUpdated(Axis axis, float value);
        public static event ThrustUpdated OnThrustUpdated;


        public static void IncrementThrust(Axis axis, float value) {
            if (OnThrustIncremented != null) OnThrustIncremented(axis, value);
        }

        public static void SetThrust(Axis axis, float value) {
            if (OnThrustSet != null) OnThrustSet(axis, value);
        }

        public static void UpdateThrust(Axis axis, float value) {
            if (OnThrustUpdated != null) OnThrustUpdated(axis, value);
        }
    }

}
