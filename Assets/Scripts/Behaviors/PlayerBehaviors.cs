using UnityEngine;
using System.Collections;
using Constants;
using Global;
using Controllers;
using Controller;

namespace Behaviors 
{
    public static class PlayerBehaviors 
    {
        

        public static void turnPlayer(GameObject player, int input) // 
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            Vector3 direction = (input == InputConstants.INPUT_LEFT ? -vehicle.transform.up : vehicle.transform.up);
            
            vehicle.velocity = Quaternion.AngleAxis((PlayerConstants.TURNING_SPEED * Time.deltaTime), direction) * vehicle.velocity;
            vehicle.rotation = Quaternion.LookRotation(vehicle.velocity);
        }

        public static IEnumerator activateSpeedBoost(GameObject player)
        {
            Cooldowns cooldowns = GetGlobalObjects.getCooldownsInstance();
            int playerNum = player.GetComponent<PlayerController>().PlayerNum;

            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            if (cooldowns.IsBoostReady[playerNum] == true)
            {
                cooldowns.IsBoostReady[playerNum] = false;

                vehicle.velocity *= PlayerConstants.BOOST_AMOUNT;
                yield return new WaitForSeconds(PlayerConstants.BOOST_DURATION);
                Debug.Log("Boost On Cooldown");

                vehicle.velocity /= PlayerConstants.BOOST_AMOUNT;
                yield return new WaitForSeconds(PlayerConstants.BOOST_COOLDOWN);
                Debug.Log("Boost Off Cooldown");

                cooldowns.IsBoostReady[playerNum] = true;
            }
        }

        public static IEnumerator ejectWall(GameObject player, GameObject wallPrefab)
        {
            Cooldowns cooldowns = GetGlobalObjects.getCooldownsInstance();
            int playerNum = player.GetComponent<PlayerController>().PlayerNum;

            if (cooldowns.IsWallReady[playerNum])
            {
                cooldowns.IsWallReady[playerNum] = false;
                Rigidbody vehicle = player.GetComponent<Rigidbody>();
                float alignToFloor = PlayerConstants.WALL_HEIGHT;
                Vector3 behindVehicle = vehicle.transform.position - vehicle.transform.forward * PlayerConstants.WALL_SPAWN_DISTANCE;
                behindVehicle.y = alignToFloor;

                GameObject spawnedWall;
                spawnedWall = (GameObject)MonoBehaviour.Instantiate(wallPrefab, behindVehicle, Quaternion.LookRotation(vehicle.velocity));
                setWallToPlayer(player, spawnedWall);

                yield return new WaitForSeconds(PlayerConstants.WALL_SPAWN_RESPAWN_TIME / Mathf.Sqrt(vehicle.velocity.sqrMagnitude));
                cooldowns.IsWallReady[playerNum] = true;
            }
        }


        private static void setWallToPlayer(GameObject player, GameObject wall)
        {
            int playerNum = player.GetComponent<PlayerController>().PlayerNum;
            wall.GetComponent<WallController>().PlayerID = playerNum;
            Material mat = (Material)Resources.Load("Walls/" + GlobalTags.PLAYER_COLORS[playerNum], typeof(Material));

            wall.GetComponent<Renderer>().material = mat;
        }



        
    }
}