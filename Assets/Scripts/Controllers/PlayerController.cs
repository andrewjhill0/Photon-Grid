using UnityEngine;
using Constants;
using System.Collections;
using Behaviors;
using Global;

namespace Controllers {
	public class PlayerController : MonoBehaviour {

        private bool isControlledPlayer;
        private int playerNum;
        private float speed;
		public bool isAlive = true;
	    public float turningSpeed;
		//private static InputController inputController;  // do we need this?
		private Rigidbody vehicle;
        public Transform walls;


		// Use this for initialization
		void Start () {
            speed = PlayerConstants.BASE_VEHICLE_SPEED;
            vehicle = GetComponent<Rigidbody>();
            vehicle.velocity = vehicle.transform.forward * speed;

			//inputController = new InputController (this);

	        
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

        public bool IsControlledPlayer
        {
            get
            {
                return this.isControlledPlayer;
            }
            set
            {
                this.isControlledPlayer = value;
            }
        }
        public int PlayerNum
        {
            get
            {
                return this.playerNum;
            }
            set
            {
                this.playerNum = value;
            }
        }

	    void OnCollisionEnter(Collision other)
	    {
	        if (other.collider.tag == GlobalTags.WALL)
	        {
                Debug.Log("Wall Collision for Player" + GetComponent<PlayerController>().PlayerNum);
	            gameObject.SetActive(false);
                isAlive = false;
	        }
            else 
            {
                int numPlayers = GetGlobalObjects.getNumberOfPlayers();
                for(int i = 0; i < numPlayers; i++)
                {
                    if(other.collider.tag == GlobalTags.PLAYERS[i])
                    {
                        Debug.Log(other.collider.tag + " and Player" + GetComponent<PlayerController>().PlayerNum + " ran into each other. BOOM!");
                        gameObject.SetActive(false);
                        isAlive = false;
                    }
                }
            }
	    }
	}
}

