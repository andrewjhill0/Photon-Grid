using UnityEngine;
using System.Collections;
using Constants;
using Global;

namespace Behaviors
{
    public static class PlayerBehaviors
    {
        // TO-DO
        // We probably should rewrite this so that each player's isBoostReady is stored within the player's object.  
        // This class should be static, but we can only handle one player with these methods, since there's no way to handle 2 different, concurrent values of isBoostReady
        // Perhaps we can move isBoostReady and speed into the activateSpeedBoost method and this will solve the problem, as there can be many concurrent but separate calls to the same method.
        private static bool isBoostReady = true;
        private static bool wallReady = false;
        

        public static void turnPlayer(GameObject player, int input)
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            Vector3 direction = (input == InputConstants.INPUT_LEFT ? -vehicle.transform.up : vehicle.transform.up);
            
            vehicle.velocity = Quaternion.AngleAxis((100 * Time.deltaTime), direction) * vehicle.velocity;
            vehicle.rotation = Quaternion.LookRotation(vehicle.velocity);
        }

        public static IEnumerator activateSpeedBoost(GameObject player)
        {
            GameObject globalVariables = GameObject.FindGameObjectsWithTag(GlobalTags.GLOBAL_VARIABLES)[0];
            GlobalStartup global = globalVariables.GetComponent<GlobalStartup>();
            Cooldowns cooldowns = global.cooldowns;
            
            Rigidbody vehicle = player.GetComponent<Rigidbody>();
            if (cooldowns.isBoostReady == true)
            {
                cooldowns.isBoostReady = false;

                vehicle.velocity *= PlayerConstants.BOOST_AMOUNT;
                yield return new WaitForSeconds(PlayerConstants.BOOST_DURATION);
                Debug.Log("Boost On Cooldown");

                vehicle.velocity /= PlayerConstants.BOOST_AMOUNT;
                yield return new WaitForSeconds(PlayerConstants.BOOST_COOLDOWN);
                Debug.Log("Boost Off Cooldown");

                cooldowns.isBoostReady = true;
            }
        }

        public static void ejectWall(GameObject player, Transform wall)
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();
            //Vector3 behindVehicle = vehicle.position - 

            //Instantiate(brick, new Vector3(x, y, 0), Quaternion.identity);

            return;
        }
        public static IEnumerator spawnWall(GameObject player)  // IEnumerators are basically coroutine signatures (methods)
        {
            wallReady = true;
            Rigidbody vehicle = player.GetComponent<Rigidbody>();
            yield return new WaitForSeconds(PlayerConstants.WALL_SPAWN_RATE);

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.AddComponent<Rigidbody>();
            cube.tag = "Wall";
            cube.name = "Player Wall";
            cube.transform.position = vehicle.position - (new Vector3(0, 0, 5));

            yield return new WaitForSeconds(PlayerConstants.WALL_SPAWN_RATE);
            wallReady = false;

        }
    }
}