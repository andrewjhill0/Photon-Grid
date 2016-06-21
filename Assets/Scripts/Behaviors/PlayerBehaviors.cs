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
        

        public static void turnPlayer(GameObject player, int input)
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            Vector3 direction = (input == InputConstants.INPUT_LEFT ? -vehicle.transform.up : vehicle.transform.up);
            
            vehicle.velocity = Quaternion.AngleAxis((100 * Time.deltaTime), direction) * vehicle.velocity;
            vehicle.rotation = Quaternion.LookRotation(vehicle.velocity);
        }

        public static IEnumerator activateSpeedBoost(GameObject player) 
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();
            isBoostReady = false;

            vehicle.velocity *= PlayerConstants.BOOST_AMOUNT;
            yield return new WaitForSeconds(PlayerConstants.BOOST_DURATION);
            Debug.Log("Boost On Cooldown");

            vehicle.velocity /= PlayerConstants.BOOST_AMOUNT;
            yield return new WaitForSeconds(PlayerConstants.BOOST_COOLDOWN);
            Debug.Log("Boost Off Cooldown");

            isBoostReady = true;
        }

        public static void ejectWall(GameObject player, Transform wall)
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();
            //Vector3 behindVehicle = vehicle.position - 

            //Instantiate(brick, new Vector3(x, y, 0), Quaternion.identity);

            return;
        }
    }
}