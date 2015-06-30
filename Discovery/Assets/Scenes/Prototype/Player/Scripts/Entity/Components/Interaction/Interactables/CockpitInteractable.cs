using UnityEngine;
using System.Collections;
using DefinitiveStudios.Discovery.Core.Entity.Components.Camera;
using DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactors;
using UnityEditor;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactables {

    public class CockpitInteractable : Interactable {


        public CockpitInteractable() : base("Cockpit") {}

        public override void Update() {
            if (currInteractor == null) return;

            if (!Input.GetAxis("Thrust").Equals(0)) {
                EventSystem.ApplyThrust(Input.GetAxis("Thrust") * 0.05f);
            }
            base.Update();
        }

        protected override void Enable(bool enable) {
            Cursor.visible = enable;
            GetComponentInChildren<Camera>().enabled = enable;
            GetComponentInChildren<Mouselook>().enabled = enable;
        }

    }
}
