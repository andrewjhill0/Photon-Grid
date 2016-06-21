using UnityEngine;
using System.Collections;
using Constants;
using Global;

namespace Behaviors 
{
    public static class PlayerBehaviors 
    {
        

        public static void turnPlayer(GameObject player, int input)
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            Vector3 direction = (input == InputConstants.INPUT_LEFT ? -vehicle.transform.up : vehicle.transform.up);
            
            vehicle.velocity = Quaternion.AngleAxis((100 * Time.deltaTime), direction) * vehicle.velocity;
            vehicle.rotation = Quaternion.LookRotation(vehicle.velocity);
        }

        public static IEnumerator activateSpeedBoost(GameObject player)
        {
            Cooldowns cooldowns = GetGlobalObjects.getCooldownsInstance();

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

        public static IEnumerator ejectWall(GameObject player, Transform wall)
        {
            Cooldowns cooldowns = GetGlobalObjects.getCooldownsInstance();

            if(cooldowns.isWallReady)
            {
                cooldowns.isWallReady = false;
                Rigidbody vehicle = player.GetComponent<Rigidbody>();
                float alignToFloor = vehicle.transform.position.y - (PlayerConstants.WALL_HEIGHT / 2); 
                Vector3 behindVehicle = vehicle.transform.position - vehicle.transform.forward * 10 - new Vector3(0.0f, alignToFloor, 0.0f);  

                MonoBehaviour.Instantiate(wall, behindVehicle, Quaternion.LookRotation(vehicle.velocity));

                yield return new WaitForSeconds(PlayerConstants.WALL_SPAWN_RATE / Mathf.Sqrt(vehicle.velocity.sqrMagnitude));
                cooldowns.isWallReady = true;
            }
        }

        
    }
}