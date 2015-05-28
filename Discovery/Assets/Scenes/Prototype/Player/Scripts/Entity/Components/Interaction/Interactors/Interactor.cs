using UnityEngine;
using System.Collections;
using DefinitiveStudios.Discovery.Prototype.Entity.Components.Interaction.Interactables;
using UnityEngine.UI;


namespace DefinitiveStudios.Discovery.Prototype.Entity.Components.Interaction.Interactors { 

    public class Interactor : MonoBehaviour {

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
	                txtInteract.text = "E to Interact"; // TODO Full Implementation: Get mapped key
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
            target.Interact(transform.parent.gameObject); // TODO Full Implementation: Set root GameObject
            currInteractable = target.gameObject;
        }

        /// <summary>
        /// Exits interacting with the current Interactable.
        /// </summary>
        public virtual void Exit() {
            currInteractable = null;
        }
    }
}
