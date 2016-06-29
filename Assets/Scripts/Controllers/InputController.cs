using UnityEngine;
using Constants;
using System.Collections;

namespace Controllers {
	public class InputController : MonoBehaviour { 

        public static InputController Instance;

        void Start()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) return; Destroy(gameObject);

        }

		public int update() {
			if (Input.GetKeyDown("space"))
				return InputConstants.INPUT_BOOST;
			if (Input.GetKey ("left"))
				return InputConstants.INPUT_LEFT;
			if (Input.GetKey ("right"))
				return InputConstants.INPUT_RIGHT;
			if (Input.GetKey (KeyCode.Escape))
				return InputConstants.INPUT_ESCAPE;

			return 0;
		}

	}
}