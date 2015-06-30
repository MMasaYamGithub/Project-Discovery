using UnityEngine;
using System.Collections;
using DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactables;
using UnityEngine.UI;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactors { 

    public abstract class Interactor : MonoBehaviour {

        public float maxDistance = 2;
        public Text txtInteract; // TODO Full Implementation: Change to MVC 
        protected GameObject currInteractable;
        private RaycastHit hit;


	    void Update () {
            txtInteract.text = ""; // TODO Full Implementation: Optimise

            Vector3 direction = transform.TransformDirection(Vector3.forward);
	        if (Physics.Raycast(transform.position, direction, out hit, maxDistance)) {
	            var interactable = hit.collider.gameObject.GetComponent<Interactable>();
                if (interactable != null) {
	                txtInteract.text = "E to Interact with "+interactable.objectName; // TODO Full Implementation: Get mapped key
	                if (Input.GetKeyDown(KeyCode.E)) {
                        // TODO Full Implementation: Switch to Observerable Input Mapper
                        Interact(interactable);
                    }
                }
	        }
        }

        /// <summary>
        /// Interacts with the target Interactable. Override to add extra behaviour before the interaction occurs.
        /// </summary>
        /// <param name="target"></param>
        protected virtual void Interact(Interactable target) {
            Enable(false);
            target.Interact(transform.root.gameObject);
            currInteractable = target.gameObject;
        }

        /// <summary>
        /// Exits interacting with the current Interactable.
        /// </summary>
        public virtual void Exit() {
            Enable(true);
            transform.root.position = currInteractable.transform.root.position;
            currInteractable = null;
        }

        /// <summary>
        /// Called when interaction starts/ends.
        /// </summary>
        /// <param name="enable">true if interaction has ended (object being re-enabled), otherwise false</param>
        protected abstract void Enable(bool enable);

    }
}
