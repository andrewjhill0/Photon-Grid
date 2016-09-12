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
        private bool checkWallTimer;


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
            checkWallTimer = true;
            Debug.Log("PC Started");
            if (SceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
            {

                if (isLocalPlayer)
                {
                    Debug.Log("PC Local Player Start.");
                    
                    enableCameraController();
                    speed = PlayerConstants.BASE_VEHICLE_SPEED;
                    vehicle = GetComponent<Rigidbody>();
                    Debug.Log("Before Setting Velocity, IsLocalPlayer: " + isLocalPlayer + " #" + PlayerNum + " 's speed is: " + vehicle.velocity);
                    vehicle.velocity = vehicle.transform.forward * speed;
                    Debug.Log("After Setting Velocity, IsLocalPlayer: " + isLocalPlayer + " #" + PlayerNum + " 's speed is: " + vehicle.velocity);
                    //CmdStartVehicle(); // vehicles are locally controlled. we can't do a cmd
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
        
		
		// Update is called once per frame
        void Update()
        {
            //Debug.Log("UPdating Player Controller for player " + playerNum);
            if (SceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
            {
               /* if(isLocalPlayer)
                {
                    if (checkWallTimer)
                    {
                        checkWallTimer = false;
                        StartCoroutine(wallTimerReset());
                        //Debug.Log("Is Wall ready for client player? " + Cooldowns.Instance.IsWallReady[playerNum]);
                        Debug.Log("Phase -1: EjectWall called by: " + playerNum);
                        CmdEjectWall(gameObject, walls); // walls are server-controlled and spawned, so we must use a Cmd
                    }

                }*/

                if (isServer)
                {
                    if (isAI)
                    {
                        StartCoroutine(PlayerBehaviors.ejectWall(gameObject, walls));
                    }
                }
            }

            
		}

        private IEnumerator wallTimerReset()
        {
            yield return new WaitForSeconds(0.3f);
            checkWallTimer = true;
        }

        [Command]
        void CmdEjectWall(GameObject gameObject, GameObject walls)
        {
                Debug.Log("Phase 0  : EjectWall called by: " + playerNum);
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
	        if (other.collider.tag == GlobalTags.WALL_TAG)
	        {
                Debug.Log(other.gameObject.GetComponent<WallController>().PlayerID + "'s wall Collision for Player" + 
                    GetComponent<PlayerController>().PlayerNum + "at: " + transform.position.ToString());
	            gameObject.SetActive(false);
                isAlive = false;
	        }
            if (other.collider.tag == GlobalTags.BOUNDARY)
            {
                Debug.Log("Boundary Collision for Player" + GetComponent<PlayerController>().PlayerNum + "at: " + transform.position.ToString());
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
                        Debug.Log(other.collider.tag + " and Player" + GetComponent<PlayerController>().PlayerNum + " ran into each other. BOOM!"
                            + "at: " + transform.position.ToString());
                        gameObject.SetActive(false);
                        isAlive = false;
                    }
                }
            }

	    }
         

    }
}

