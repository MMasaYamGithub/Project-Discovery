using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DefinitiveStudios.Discovery.Core.Utils;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components {

    /// <summary>
    /// Component for simulating gravitational fields. The collider is the max gravitational field range and will be set to be a trigger.
    /// Works with most 3D objects.
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
        private bool flipNormal;
        private RaycastHit hit;


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
                if (!currPosition.Equals(prevPosition) || !currRotation.Equals(prevRotation)) gravVector = Utils.GetNormal(mesh);
            }
            foreach (Rigidbody body in contactBodies) {
                //if (isSphere) gravVector = (transform.position - body.transform.position).normalized;
                //else {
                //    // Reverse the normal if the target is below the mesh
                //    flipNormal = (Vector3.Dot(Vector3.up, body.transform.position - transform.position) < 0);
                //}
                //body.AddForce(((flipNormal)? gravVector : -gravVector) * gravity);
                if (Physics.Raycast(body.position, transform.position - body.position, out hit, Vector3.Distance(transform.position, body.position), 1 << 8)) {
                    Debug.DrawRay(body.position, transform.position - body.position);
                    body.rotation = Quaternion.Slerp(body.rotation, Quaternion.FromToRotation(Vector3.up, hit.normal), Time.fixedDeltaTime * 5);
                    body.AddForce(-hit.normal * gravity); // TODO: SCale gravity based on distance from center
                }
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
