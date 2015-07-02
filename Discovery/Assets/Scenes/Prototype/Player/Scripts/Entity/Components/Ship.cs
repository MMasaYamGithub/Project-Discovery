using UnityEngine;
using System.Collections;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    [RequireComponent(typeof(ShipStats))]
    [RequireComponent(typeof(Rigidbody))]
    public class Ship : MonoBehaviour {

        private ShipStats stats;
        private Rigidbody body;

        void Start() {
            stats = GetComponent<ShipStats>();
            body = GetComponent<Rigidbody>();
        }

        void Update() {
            body.AddRelativeForce(Vector3.left * 10 * stats.thrustX);
            body.AddRelativeForce(Vector3.up * 10 * stats.thrustY);
            body.AddRelativeForce(-Vector3.forward * 10 * stats.thrustZ); // Models need to be facing forward, my prototype isn't
            body.AddTorque(-Vector3.forward * 10 * stats.thrustRoll);  // Models need to be facing forward, my prototype isn't
            body.AddTorque(Vector3.left * 10 * stats.thrustPitch);
            body.AddTorque(Vector3.up * 10 * stats.thrustYaw);
        }

    }

}
