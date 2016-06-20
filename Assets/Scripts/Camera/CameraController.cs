using UnityEngine;
using System.Collections;
using Controllers;

namespace Controllers {
	public class CameraController : MonoBehaviour {

		public GameObject player;
		public PlayerController playerController; 
		private bool inBirdsEyeView = false;
		
		void Start () {
		}

		void LateUpdate () {
			if (playerController.isAlive) {
				// Follow the player's transform
				Vector3 cameraY = new Vector3 (0, 15, 0);
				transform.position = (player.transform.position - player.transform.forward * 25)
					+ cameraY;

				// Follow the player's forward direction
				transform.LookAt (player.transform.position);



			}
			StartCoroutine (CheckIfPlayerDead ());
		}

		private IEnumerator CheckIfPlayerDead ()
		{
			if (!playerController.isAlive && !inBirdsEyeView) 
			{
				// Adjust the camera to bird's-eye view
				goToBirdsEyeView();
				Debug.Log("BIRD EYE VIEW");
				yield return null;
			}
		}

		private void goToBirdsEyeView()
		{
			inBirdsEyeView = true;
			
			Vector3 newPosition = new Vector3(0f, 1454f, 0);
			transform.position = newPosition;

			Vector3 relativePos = new Vector3(0f,0f,0f) - transform.position;
			Quaternion rotation = Quaternion.LookRotation(relativePos);
			transform.rotation = rotation;

		}

	}
}