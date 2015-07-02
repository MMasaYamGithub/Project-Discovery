using UnityEngine;
using System.Collections;
using DefinitiveStudios.Discovery.Core.Entity.Components.Camera;
using DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactors;
using UnityEditor;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactables {

    public class CockpitInteractable : Interactable {


        public CockpitInteractable() : base("Cockpit") {}

        protected override void Enable(bool enable) {
            Cursor.visible = enable;
            GetComponentInChildren<Camera>().enabled = enable;
            GetComponentInChildren<Mouselook>().enabled = enable;
            GetComponent<ShipController>().active = enable;
        }

        

    }
}
