using UnityEngine;
using System.Collections;


namespace DefinitiveStudios.Discovery.Core.Utils {

    static public class Utils {


        /// <summary>
        /// Reimplementation of GetComponentInChildren that includes inactive game objects. Based on http://answers.unity3d.com/questions/555101/possible-to-make-gameobjectgetcomponentinchildren.html.
        /// </summary>
        /// <typeparam name="T">Component type to find</typeparam>
        /// <param name="obj">Root GameObject</param>
        /// <returns>Component of type T or null if none found</returns>
        static public T GetComponentInChildren<T>(GameObject obj) where T : Component {
            var component = obj.GetComponent<T>();
            if (component == null) {
                Transform root = obj.transform;
                for (var i = 0; i < root.childCount; i++) {
                    component = GetComponentInChildren<T>(root.GetChild(i).gameObject);
                    if (component != null) break;
                }
            }

            return component;
        }
    }

}
