using UnityEngine;
using Constants;
using System.Collections;

namespace Controllers {
	public class InputController {

		private PlayerController player;

		public InputController (PlayerController player)
		{
			this.player = player;
		}

		public int update() {
			if (Input.GetKeyDown("space") && player.isBoostReady)
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