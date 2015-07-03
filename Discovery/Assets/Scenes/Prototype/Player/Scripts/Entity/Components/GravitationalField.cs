using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DefinitiveStudios.Discovery.Core.Utils;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    /// <summary>
    /// Component for simulating gravitational fields. The collider is the max gravitational field range and will be set to be a trigger.
    /// Works with planes and spheres.
    /// TODO: Use simple normal calculation for spheres (transform.position - target.transform.position)
    /// TODO: Multiply gravity based on distance from origin
    /// </summary>
    [RequireComponent(typeof(Mesh))]
    public class GravitationalField : MonoBehaviour {

        public float gravity = 9.8f;
        public Collider fieldCollider;
        private readonly List<Rigidbody> contactBodies = new List<Rigidbody>();
        // optimisation vars
        private Vector3 currPosition;
        private Vector3 prevPosition;
        private Quaternion currRotation;
        private Quaternion prevRotation;
        private Vector3 gravVector;
        private Mesh mesh;
        private bool isSphere;


        private void Start() {
            mesh = GetComponent<MeshFilter>().mesh;
            if (fieldCollider == null) throw new UnityException("Gravitational Field requires a field collider!");
            fieldCollider.isTrigger = true;
            isSphere = fieldCollider is SphereCollider;
        }

        private void FixedUpdate() {
            if (!isSphere) {
                prevPosition = currPosition;
                prevRotation = currRotation;
                currPosition = transform.position;
                currRotation = transform.rotation;
                if (currPosition != prevPosition || currRotation != prevRotation) gravVector = Utils.GetNormal(mesh);
            }
            foreach (Rigidbody body in contactBodies) {
                if (isSphere) gravVector = (transform.position - body.transform.position).normalized;
                else {
                    // Reverse the normal if the target is below the plane
                    gravVector = (Vector3.Dot(transform.position, body.transform.position) > 0)? -gravVector : gravVector;
                }
                body.AddForce(gravVector * gravity);
            }
        }

        private void OnTriggerEnter(Collider other) {
            contactBodies.Add(other.attachedRigidbody);
        }

        private void OnTriggerExit(Collider other) {
            contactBodies.Remove(other.attachedRigidbody);
        }
    }
}
