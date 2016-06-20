using UnityEngine;
using System.Collections;
using Constants;

namespace Controllers
{
    public static class PlayerBehaviors
    {

        public static readonly int TURNING_SPEED = 2;
        public static bool IS_BOOST_READY = true;
        public static readonly int boostDuration = 2; // why is this static?
        public static readonly int boostCooldown = 2;  // cooldown starts after the boost duration ends.
        public static readonly float speedBoost = 5.0f;
        public static float speed = 15;

        public static void turnPlayer(GameObject player, int input)
        {
            Rigidbody vehicle = player.GetComponent<Rigidbody>();

            Vector3 direction = (input == InputConstants.INPUT_LEFT ? -vehicle.transform.up : vehicle.transform.up);

            vehicle.transform.Rotate(direction * TURNING_SPEED);
        }

        public static IEnumerator activateSpeedBoost(GameObject player) 
        { 
            Rigidbody vehicle = player.GetComponent<Rigidbody>();
            IS_BOOST_READY = false;

//          Vector3 originalSpeed = vehicle.velocity;
            Vector3 originalSpeed = vehicle.transform.forward;

//          vehicle.velocity = originalSpeed * speedBoost;
//          vehicle.transform.Translate (originalSpeed * speedBoost);
            speed *= speedBoost;
            Debug.Log("Before Waiting 2 seconds");
            yield return new WaitForSeconds (boostDuration);
//          vehicle.velocity = originalSpeed;
//          vehicle.transform.Translate (originalSpeed);
            speed /= speedBoost;
            Debug.Log("After Waiting 2 Seconds");
            yield return new WaitForSeconds(boostCooldown);
            IS_BOOST_READY = true;
        }
    }
}