using UnityEngine;
using System.Collections;
using Constants;

namespace Controllers
{
    public static class PlayerBehaviors
    {
        // TO-DO
        // We probably should rewrite this so that each player's isBoostReady is stored within the player's object.  
        // This class should be static, but we can only handle one player with these methods, since there's no way to handle 2 different, concurrent values of isBoostReady
        // Perhaps we can move isBoostReady and speed into the activateSpeedBoost method and this will solve the problem, as there can be many concurrent but separate calls to the same method.
        public static bool isBoostReady = true;
        public static float speed = 15;

        public static void turnPlayer(GameObject player, int input)
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            Vector3 direction = (input == InputConstants.INPUT_LEFT ? -vehicle.transform.up : vehicle.transform.up);

            vehicle.transform.Rotate(direction * PlayerConstants.TURNING_SPEED * Time.deltaTime);  //deltaTime is needed to decouple turning from the frame rate of the game.
        }

        public static IEnumerator activateSpeedBoost(GameObject player) 
        { 
            Rigidbody vehicle = player.GetComponent<Rigidbody>();
            isBoostReady = false;

//          Vector3 originalSpeed = vehicle.velocity;
            Vector3 originalSpeed = vehicle.transform.forward;

//          vehicle.velocity = originalSpeed * speedBoost;
//          vehicle.transform.Translate (originalSpeed * speedBoost);
            speed *= PlayerConstants.BOOST_AMOUNT;
            Debug.Log("Before Waiting 2 seconds");
            yield return new WaitForSeconds (PlayerConstants.BOOST_DURATION);
//          vehicle.velocity = originalSpeed;
//          vehicle.transform.Translate (originalSpeed);
            speed /= PlayerConstants.BOOST_AMOUNT;
            Debug.Log("After Waiting 2 Seconds");
            yield return new WaitForSeconds(PlayerConstants.BOOST_COOLDOWN);
            isBoostReady = true;
        }
    }
}