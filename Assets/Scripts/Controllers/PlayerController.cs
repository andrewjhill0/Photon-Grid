using UnityEngine;
using Constants;
using System.Collections;
using Behaviors;
using Global;
using Controller;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Controllers {
	public class PlayerController : NetworkBehaviour {

        private bool isControlledPlayer; // Truncated.  We don't need this anymore due to the network framework.  IsLocalPlayer is sufficient.
        public bool isAI;
        private int playerNum;
        [SyncVar]
        private float speed;
        [SyncVar]
		public bool isAlive = false;
	    public float turningSpeed;
		private Rigidbody vehicle;
        public GameObject walls;


        void Awake()
        {
            Debug.Log("PC Awake");
            StartCoroutine(pcAwake());
            
        }

        private IEnumerator pcAwake()
        {
            //gameObject.SetActive(false);
            while(GameState.Instance == null)
            {
                Debug.Log("PC waiting for GameState to Awake()");
                yield return new WaitForSeconds(1);
            }
            Debug.Log("PC Waiting done.");
            GameState.Instance.updatePlayerList(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            Debug.Log("PC Started");
            if (SceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
            {

                if (isLocalPlayer)
                {
                    Debug.Log("PC Local Player Start.");
                    
                    enableCameraController();speed = PlayerConstants.BASE_VEHICLE_SPEED;
                    vehicle = GetComponent<Rigidbody>();
                    Debug.Log("IsLocalPlayer: " + isLocalPlayer + " #" + PlayerNum + " 's speed is: " + vehicle.velocity);
                    vehicle.velocity = vehicle.transform.forward * speed;
                    Debug.Log("IsLocalPlayer: " + isLocalPlayer + " #" + PlayerNum + " 's speed is: " + vehicle.velocity);
                    //CmdStartVehicle();
                }

                if(isServer)
                {
                    if(isAI)
                    {
                        speed = PlayerConstants.BASE_VEHICLE_SPEED;
                        vehicle = GetComponent<Rigidbody>();
                        vehicle.velocity = vehicle.transform.forward * speed;
                    }
                }

            }
	        
		}
        [Command]
        void CmdStartVehicle()
        {
        }  
        
		
		// Update is called once per frame
        void Update()
        {
            
            if (SceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
            {
                if(isLocalPlayer)
                {
                    CmdEjectWall(gameObject, walls);
                }

                if (isServer)
                {
                    if (isAI)
                    {
                        StartCoroutine(PlayerBehaviors.ejectWall(gameObject, walls));
                    }
                }
            }

            
		}

        [Command]
        void CmdEjectWall(GameObject gameObject, GameObject walls)
        {
                StartCoroutine(PlayerBehaviors.ejectWall(gameObject, walls));
        }

        private void enableCameraController()
        {
            Debug.Log("Does GameState Exist? ");
            if (GameObject.FindGameObjectWithTag(GlobalTags.CAMERA) != null)
            {
                Debug.Log("Yes");
            }
            else
            {
                Debug.Log("No");
            }
            GameObject.FindGameObjectWithTag(GlobalTags.CAMERA).GetComponent<CameraController>().enabled = true;
        }

		void FixedUpdate()	{
            if (SceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
            {
                if (isLocalPlayer)
                {
                    int input = InputController.Instance.update();

                    if (input == InputConstants.INPUT_BOOST)
                        StartCoroutine(PlayerBehaviors.activateSpeedBoost(gameObject));
                    else if (input == InputConstants.INPUT_LEFT)
                        PlayerBehaviors.turnPlayer(gameObject, InputConstants.INPUT_LEFT);
                    else if (input == InputConstants.INPUT_RIGHT)
                        PlayerBehaviors.turnPlayer(gameObject, InputConstants.INPUT_RIGHT);
                }
            }

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
        public bool IsAI 
        {
            get
            {
                return this.isAI;
            }
            set
            {
                this.isAI = value;
            }
        }

        
	    void OnCollisionEnter(Collision other)
	    {
	        if (other.collider.tag == GlobalTags.WALL)
	        {
                Debug.Log(other.gameObject.GetComponent<WallController>().PlayerID + "'s wall Collision for Player" + GetComponent<PlayerController>().PlayerNum);
	            gameObject.SetActive(false);
                isAlive = false;
	        }
            if (other.collider.tag == GlobalTags.BOUNDARY)
            {
                Debug.Log("Boundary Collision for Player" + GetComponent<PlayerController>().PlayerNum);
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

