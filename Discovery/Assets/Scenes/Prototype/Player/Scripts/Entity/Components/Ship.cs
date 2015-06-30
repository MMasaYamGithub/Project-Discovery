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
            body.AddForce(0, 0, 10 * stats.zThrust);
        }

    }

}
