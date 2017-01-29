using UnityEngine;
using System.Collections;
//using UnityEngine.UI;

namespace Buttons {

    public class BackButton : MonoBehaviour {
        public void destroySceneObjects() {
            foreach(GameObject o in Object.FindObjectsOfType<GameObject>()) {
                Destroy(o);
            }
        }
    }
}
