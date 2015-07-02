using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Linq;
using UnityEngine.UI;


namespace DefinitiveStudios.Discovery.Prototype.Player.Entity.Components.UI.Views {

    public class ThrustView : MonoBehaviour {

        void OnEnable() {
            EventSystem.OnThrustUpdated += UpdateThrustDisplay;
        }

        void OnDisable() {
            EventSystem.OnThrustUpdated -= UpdateThrustDisplay;
        }
        
        private void UpdateThrustDisplay(Axis axis, float value) {
            //TODO: Create Tag manager
            transform.Find(axis + "Value").GetComponent<Text>().text = Mathf.Round(value * 100).ToString(CultureInfo.InvariantCulture);
        }

    }

}
