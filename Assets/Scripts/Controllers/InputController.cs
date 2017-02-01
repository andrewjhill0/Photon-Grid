using UnityEngine;
using Constants;
using System.Collections;

namespace Controllers {
    /// <summary>
    /// Singleton class that gives information about the current user input.
    /// </summary>
    public class InputController : MonoBehaviour {
        public static InputController Instance;

        /// <summary>
        /// Called when the associated GameObject is initialized.
        /// </summary>
        void Start() {
            if(Instance == null)
                Instance = this;
            else if(Instance != this)
                return;
            Destroy(gameObject);
        }

        /// <summary>
        /// Returns the current user input.
        /// </summary>
        /// <returns>Value defined in InputConstants or 0 if no detected input.</returns>
        public int update() {
            if(Input.GetKeyDown("space"))
                return InputConstants.INPUT_BOOST;
            if(Input.GetKey("left"))
                return InputConstants.INPUT_LEFT;
            if(Input.GetKey("right"))
                return InputConstants.INPUT_RIGHT;
            if(Input.GetKey(KeyCode.Escape))
                return InputConstants.INPUT_ESCAPE;

            return 0;
        }
    }
}