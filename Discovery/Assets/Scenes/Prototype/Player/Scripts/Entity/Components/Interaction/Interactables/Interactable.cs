using UnityEngine;
using System.Collections;
using DefinitiveStudios.Discovery.Core.Utils;
using DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactors;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactables {

    public abstract class Interactable : MonoBehaviour {

        protected GameObject currInteractor;


        void Update() {
            // TODO Full Implementation: Switch to Observerable Input Mapper
            if (Input.GetKeyDown(KeyCode.E)) {
                if (currInteractor) Exit();
            }
        }

        /// <summary>
        /// Executes the interact sequence on the target GameObject. Make sure to call base.
        /// </summary>
        /// <param name="target">GameObject interacting with this object</param>
        public virtual void Interact(GameObject target) {
            currInteractor = target;
        }

        /// <summary>
        /// Exits interaction with this Interactable. Make sure to call base.
        /// </summary>
        public virtual void Exit() {
            Utils.GetComponentInChildren<Interactor>(currInteractor).Exit();
            currInteractor = null;
        }
    }

}
