using UnityEngine;
using Constants;
using System.Collections;
using Behaviors;

namespace Controllers {
	public class PlayerController : MonoBehaviour {

        private float speed;
		public bool isAlive = true;
	    public float turningSpeed;
		private static InputController inputController;  // do we need this?
		private Rigidbody vehicle;
        public Transform walls;


		// Use this for initialization
		void Start () {
            speed = PlayerConstants.BASE_VEHICLE_SPEED;
            vehicle = GetComponent<Rigidbody>();
            vehicle.velocity = Vector3.forward * speed;

			inputController = new InputController (this);

	        
		}
		
		// Update is called once per frame
		void Update () {
            StartCoroutine(PlayerBehaviors.ejectWall(gameObject, walls));
		}
		
		void FixedUpdate()	{

	
			/*int input = inputController.update ();


			if (input == InputConstants.INPUT_BOOST)
				StartCoroutine (activateSpeedBoost());
			else if (input == InputConstants.INPUT_LEFT)
				turnPlayer (InputConstants.INPUT_LEFT);
			else if (input == InputConstants.INPUT_RIGHT)
				turnPlayer (InputConstants.INPUT_RIGHT);*/
		}

		

	    void OnCollisionEnter(Collision other)
	    {
	        if (other.collider.tag == "Wall")
	        {
	            Debug.Log("Collision");
	            gameObject.SetActive(false);
                isAlive = false;
	        }
	    }
	}
}

