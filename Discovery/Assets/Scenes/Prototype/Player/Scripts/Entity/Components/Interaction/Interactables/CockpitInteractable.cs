using UnityEngine;
using System.Collections;
using DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactors;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactables {

    public class CockpitInteractable : Interactable {

        public override void Interact(GameObject target) {
            // Only one entity can use the cockpit at once
            if (currInteractor != null) {
                // TODO Full Implementation: MVC UI Errors
                print("Something else is interacting with that");
                return;
            }

            Cursor.visible = true; // Placeholder for interacting with screens
            GetComponentInChildren<Camera>().enabled = true;
            base.Interact(target);
        }

        public override void Exit() {
            Cursor.visible = false;
            base.Exit();
        }

    }
}
