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

        /// <summary>
        /// Returns the normal of a mesh's first triangle. Useful for getting the normal of a plane.
        /// </summary>
        /// <param name="mesh">Mesh to use</param>
        /// <returns>Normal of first triangle</returns>
        static public Vector3 GetNormal(Mesh mesh) {
            Vector3 side1 = mesh.vertices[mesh.triangles[1]] - mesh.vertices[mesh.triangles[0]];
            Vector3 side2 = mesh.vertices[mesh.triangles[2]] - mesh.vertices[mesh.triangles[0]];
            Vector3 normal = Vector3.Cross(side1, side2);
            //normal /= normal.magnitude;
            return normal;
        }
    }

}
