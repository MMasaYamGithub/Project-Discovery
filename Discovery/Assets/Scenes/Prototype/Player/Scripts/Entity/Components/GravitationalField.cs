using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DefinitiveStudios.Discovery.Core.Utils;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    /// <author>
    ///  Andy Palmer (ALPSquid)
    /// </author>
    /// <summary>
    /// Component for simulating gravitational fields. The collider is the max gravitational field range and will be set to be a trigger.
    /// Works with most (all?) 3D objects.
    /// </summary>
    //[RequireComponent(typeof(Mesh))]
    public class GravitationalField : MonoBehaviour {

        private const float EARTH_MASS = 5.976f; // * 10^24
        private const float EARTH_RADIUS = 6371f;
        private const float EARTH_SURFACE_GRAVITY = 9.8f;

        public float surfaceGravity = EARTH_SURFACE_GRAVITY;
        public float massMultiple = 1;  // Mass in relation to Earth
        public float radiusMultiple = 1;  // Radius in relation to Earth
        public bool useRelationalValues = true;  // Whether to calculate surface gravity and max gravitational distance based on the above relational values.
                                                 // If false, the collider radius will be used as the max field distance and gravity will not be changed.
                                                 // If true, gravity and max field distance will be calculated based on the relational value. 
        private float gravityThreshold = 0.5f;  // Falloff for max gravity distance. The gravitational field will stop when gravity is smaller than this value.
                                               // Only used when useRelationalValue sis enabled
        public Collider fieldCollider;
        private readonly List<Rigidbody> contactBodies = new List<Rigidbody>();
        private float radius;
        private float mass;
        // optimisation vars
        private Vector3 gravVector;
        private RaycastHit hit;


        protected void Start() {
            if (fieldCollider == null) throw new UnityException("Gravitational Field requires a field collider!");
            fieldCollider.isTrigger = true;

            mass = EARTH_MASS * massMultiple;
            radius = EARTH_RADIUS * radiusMultiple;
            if (useRelationalValues) {
                //surfaceGravity = 9.8f;

                // Set max field distance to something realistic
                // TODO: This might actually affect gameplay in a negative way (things apparently moving on their own due to a distant planet)
                //float gravityThresholdMultiplier = (gravityThreshold * gravityThreshold) * 10000; // Distance required to reach a gravitational force of 0.5
                //if (fieldCollider is SphereCollider) {
                //    ((SphereCollider)fieldCollider).radius = gravityThresholdMultiplier;
                //} else if (fieldCollider is CapsuleCollider) {
                //    ((CapsuleCollider)fieldCollider).radius = gravityThresholdMultiplier;
                //    ((CapsuleCollider)fieldCollider).height = gravityThresholdMultiplier;
                //}else if (fieldCollider is BoxCollider) {
                //    ((BoxCollider)fieldCollider).size = new Vector3(gravityThresholdMultiplier, gravityThresholdMultiplier, gravityThresholdMultiplier);
                //}
            }
        }

        protected void FixedUpdate() {
            foreach (Rigidbody body in contactBodies) {
                // TODO: Layer index manager (layer 8)
                if (Physics.Raycast(body.position, transform.position - body.position, out hit, Vector3.Distance(transform.position, body.position), 1 << 8)) {
                    Debug.DrawRay(body.position, transform.position - body.position);
                    // TODO: Don't interpolate local y
                    Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    targetRotation.y = body.rotation.y;
                    body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.fixedDeltaTime * 5);
                    body.AddForce(-hit.normal * GetGravityAt(hit.distance));
                    if (fieldCollider is SphereCollider) print(GetGravityAt(hit.distance));
                }
            }
        }

        protected float GetGravityAt(float distance) {
            // K = Distance from center / Radius
            // force = surface gravity * (1/K^2)
            return (float)(surfaceGravity * (1 / Math.Pow((distance+radius) / radius, 2)));
        }

        protected void OnTriggerEnter(Collider other) {
            contactBodies.Add(other.attachedRigidbody);
        }

        protected void OnTriggerExit(Collider other) {
            contactBodies.Remove(other.attachedRigidbody);
        }
    }
}
