using System;
using UnityEngine;
using System.Collections;
using DefinitiveStudios.Discovery.Core.Utils;
using DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactors;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactables {

    public abstract class Interactable : MonoBehaviour {

        protected GameObject currInteractor;
        public string objectName { get; protected set; }


        protected Interactable(string objectName) {
            this.objectName = objectName;
        }

        public virtual void Update() {
            if (currInteractor == null) return;

            // TODO Full Implementation: Switch to Observerable Input Mapper
            if (Input.GetKeyDown(KeyCode.F)) {
                Exit();
            }
        }

        /// <summary>
        /// Executes the interact sequence on the target GameObject. Make sure to call base.
        /// </summary>
        /// <param name="target">GameObject interacting with this object</param>
        public virtual void Interact(GameObject target) {
            // Only one entity can use an interactable at once (for now)
            if (currInteractor != null) {
                // TODO Full Implementation: MVC UI Errors
                print("Something else is interacting with that");
                return;
            }
            Enable(true);
            currInteractor = target;
        }

        /// <summary>
        /// Exits interaction with this Interactable. Make sure to call base.
        /// </summary>
        public virtual void Exit() {
            Enable(false);
            Utils.GetComponentInChildren<Interactor>(currInteractor).Exit();
            currInteractor = null;
        }

        /// <summary>
        /// Called when interaction starts/ends.
        /// </summary>
        /// <param name="enable">true if interaction has started (object being enabled), otherwise false</param>
        protected abstract void Enable(bool enable);
    }

}
