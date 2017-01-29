using UnityEngine;
using System.Collections;
using Constants;
using Global;
using Controllers;
using Controller;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Behaviors {
    /// <summary>
    /// Abstract class that defines the shared behaviors of all vehicles in the game.
    /// </summary>
    public abstract class VehicleBehaviors {
        #region methods
        /// <summary>
        /// This static method allows the player gameobject to change it's velocity and rotation, thus "turning" the player gameobject.
        /// </summary>
        /// <param name="player">  The locally controlled player gameobject.  Should be the vehicle the player is controlling.</param>
        /// <param name="input">  Which direction is it turning?  See the Inputconstants class for definitions.</param>
        /// <returns> void </returns>
        public static void turnPlayer(GameObject player, int input) // 
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            Vector3 direction = (input == InputConstants.INPUT_LEFT ? -vehicle.transform.up : vehicle.transform.up);

            vehicle.velocity = Quaternion.AngleAxis((PlayerConstants.TURNING_SPEED * Time.deltaTime), direction) * vehicle.velocity;
            vehicle.rotation = Quaternion.LookRotation(vehicle.velocity);
        }

        /// <summary>
        /// This static Coroutine method will put the player gameobject into boost mode (higher velocity) for the BOOST_DURATION time.  
        /// Will access the singleton Cooldowns instance running in the game scene.
        /// </summary>
        /// <param name="player">  The player gameobject to be boosted.</param>
        /// <returns> IEnumerator </returns>
        public static IEnumerator activateSpeedBoost(GameObject player) {
            Cooldowns cooldowns = GetGlobalObjects.getCooldownsInstance();
            int playerNum = player.GetComponent<Controllers.PlayerController>().PlayerNum;

            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            if(cooldowns.IsBoostReady[playerNum] == true) {
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

        /// <summary>
        /// This static Coroutine method that will instantiate a Wall prefab at a set distance and rotation behind a given player gameobject.
        /// This will access the Cooldowns singleton instance.
        /// </summary>
        /// <param name="player">  The player gameobject to put the wall behind.</param>
        /// <param name="wallPrefab">  The wall prefab that will be instantiated.  Depricated.  Needs to be removed.</param>
        /// <returns> IEnumerator  </returns>
        public static IEnumerator ejectWall(GameObject player, GameObject wallPrefab) {
            Debug.Log("Phase 1: EjectWall called by: " + player.GetComponent<Controllers.PlayerController>().PlayerNum);

            Cooldowns cooldowns = GetGlobalObjects.getCooldownsInstance();
            int playerNum = player.GetComponent<Controllers.PlayerController>().PlayerNum;

            Debug.Log("Phase 2: EjectWall called by: " + player.GetComponent<Controllers.PlayerController>().PlayerNum +
                "  ready value: " + cooldowns.IsWallReady[playerNum]);
            if(cooldowns.IsWallReady[playerNum]) {
                Debug.Log("Phase 3: EjectWall called by: " + player.GetComponent<Controllers.PlayerController>().PlayerNum);
                cooldowns.IsWallReady[playerNum] = false;

                Rigidbody vehicle = player.GetComponent<Rigidbody>();

                Vector3 behindVehicle = vehicle.transform.position - vehicle.transform.forward * PlayerConstants.WALL_SPAWN_DISTANCE;
                behindVehicle.y = PlayerConstants.WALL_HEIGHT;
                Debug.Log("Phase 4: EjectWall called by: " + player.GetComponent<Controllers.PlayerController>().PlayerNum);

                GameObject spawnedWall;
                spawnedWall = (GameObject) MonoBehaviour.Instantiate(Resources.Load(GlobalTags.WALL_PREFAB), behindVehicle, Quaternion.LookRotation(vehicle.velocity));

                Debug.Log("Phase 5: EjectWall called by: " + player.GetComponent<Controllers.PlayerController>().PlayerNum);
                NetworkServer.Spawn(spawnedWall);
                GameState.Instance.RpcSetWallToPlayer(player, spawnedWall);
                Debug.Log("Phase 6: EjectWall called by: " + player.GetComponent<Controllers.PlayerController>().PlayerNum + "  " + PlayerConstants.WALL_SPAWN_RESPAWN_TIME);
                yield return new WaitForSeconds(PlayerConstants.WALL_SPAWN_RESPAWN_TIME);
                cooldowns.IsWallReady[playerNum] = true;
                Debug.Log("Phase 7*****: EjectWall called by: " + player.GetComponent<Controllers.PlayerController>().PlayerNum);
            }
        }
        #endregion
    }
}
