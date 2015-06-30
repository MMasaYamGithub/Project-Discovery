using UnityEngine;
using System.Collections;
using DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactables;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.Interaction.Interactors {

    public class PlayerInteractor : Interactor {

        protected override void Enable(bool enable) {
            GetComponent<Camera>().enabled = enable;
            transform.parent.gameObject.SetActive(enable);
        }
    }
}
