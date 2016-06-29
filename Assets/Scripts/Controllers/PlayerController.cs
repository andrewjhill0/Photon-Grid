using UnityEngine;
using Constants;
using System.Collections;
using Behaviors;
using Global;
using Controller;
using UnityEngine.Networking;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Controllers {
	public class PlayerController : NetworkBehaviour {

        private bool isControlledPlayer; // Truncated.  We don't need this anymore due to the network framework.  IsLocalPlayer is sufficient.
        private bool isAI;
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
            if (EditorSceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
            {
                GameState.instance.updatePlayerList(gameObject);

            }
        }
		// Use this for initialization
		void Start () {
            if (EditorSceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
            {
                Material mat = (Material)Resources.Load("Vehicles/" + GlobalTags.PLAYER_COLORS[playerNum], typeof(Material));
                GetComponent<Renderer>().material = mat;

                speed = PlayerConstants.BASE_VEHICLE_SPEED;
                vehicle = GetComponent<Rigidbody>();
                vehicle.velocity = vehicle.transform.forward * speed;
            }
	        
		}
		
		// Update is called once per frame
		void Update () {
            if (EditorSceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
            {
                StartCoroutine(PlayerBehaviors.ejectWall(gameObject, walls));
            }
		}
		
		void FixedUpdate()	{
            if (EditorSceneManager.GetActiveScene().name == GlobalTags.GAME_SCREEN)
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

