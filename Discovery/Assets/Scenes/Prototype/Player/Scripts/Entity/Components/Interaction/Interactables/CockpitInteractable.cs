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

            applyAxis(Axis.X);
            applyAxis(Axis.Y);
            applyAxis(Axis.Z);
            applyAxis(Axis.Roll);
            applyAxis(Axis.Pitch);
            applyAxis(Axis.Yaw);
            base.Update();
        }

        protected override void Enable(bool enable) {
            Cursor.visible = enable;
            GetComponentInChildren<Camera>().enabled = enable;
            GetComponentInChildren<Mouselook>().enabled = enable;
        }

        private void applyAxis(Axis axis) {
            string axisName = "Thrust" + axis;
            if (!Input.GetAxis(axisName).Equals(0)) {
                EventSystem.ApplyThrust(axis, Input.GetAxis(axisName) * 0.05f);
            }
        }

    }
}
