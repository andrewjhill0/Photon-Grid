using UnityEngine;
using System.Collections;
using Controllers;
using Global;
using Constants;
using UnityEngine.Networking;

namespace Controllers {
	public class CameraController : MonoBehaviour {

		private GameObject player;
		private PlayerController playerController;  
		private bool inBirdsEyeView = false;
		
		void Start () {
            player = GetGlobalObjects.getControllablePlayer();
            playerController = player.GetComponent<PlayerController>();
		}

        // we want to use LateUpdate() because it is always executed after all the other Update()'s called on this frame.  
        // Since this is a follow camera, we want to follow the updated movements
		void LateUpdate () {

                if (!GetGlobalObjects.getGameState().getGameOver())
                {
                    // Follow the player's transform
                    transform.position = (player.transform.position - player.transform.forward * CameraConstants.STANDARD_CAMERA_DISTANCE_MULT) + CameraConstants.STANDARD_CAMERA_HEIGHT;

                    // Follow the player's forward direction
                    transform.LookAt(player.transform.position);
                }
                StartCoroutine(CheckIfPlayerDead());
            
		}

		private IEnumerator CheckIfPlayerDead ()
		{
            GameState gameState = GetGlobalObjects.getGameState();

			if (gameState.getGameOver() && !inBirdsEyeView) 
			{
				// Adjust the camera to bird's-eye view
				goToBirdsEyeView();
				Debug.Log("BIRD EYE VIEW");
                yield return new WaitForSeconds(10);
                //Application.Quit();
			}
		}

		private void goToBirdsEyeView()  //We want to look down at the whole playing field after we die or the game ends.
		{
			inBirdsEyeView = true;
            transform.position = CameraConstants.BIRDS_EYE_POSITION;
            transform.rotation = CameraConstants.BIRDS_EYE_ROTATION;
		}
	}
}