using UnityEngine;
using System.Collections;
using DefinitiveStudios.Discovery.Prototype.Entity.Components.Interaction.Interactables;


namespace DefinitiveStudios.Discovery.Prototype.Entity.Components.Interaction.Interactors {

    public class PlayerInteractor : Interactor {


        protected override void Interact(Interactable target) {
            Enable(false);
            base.Interact(target);
        }

        public override void Exit() {
            Enable(true);
            base.Exit();
        }

        private void Enable(bool enable) {
            GetComponent<Camera>().enabled = enable;
            transform.parent.gameObject.SetActive(enable);
        }

    }

}
