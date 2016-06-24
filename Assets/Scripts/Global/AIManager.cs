using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Behaviors;
using Constants;       //Allows us to use Lists. 

namespace Global
{

    public class AIManager : MonoBehaviour
    {

        public static AIManager instance = null;              //Static instance of AIManager which allows it to be accessed by any other script.
        //private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
        //private int level = 3;                                  //Current level number, expressed in game as "Day 1".
        private List<int>  aiPlayers = new List<int>();
        private List<GameObject> aiPlayerObjects = new List<GameObject>();

        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Get a component reference to the attached BoardManager script
            //boardScript = GetComponent<BoardManager>();

            //Call the InitGame function to initialize the first level 
            InitGame();
        }

        //Initializes the game for each level.
        void InitGame()
        {
            //Call the SetupScene function of the BoardManager script, pass it current level number.
            //boardScript.SetupScene(level);

        }

        //Update is called every frame.
        void Update()
        {
            /*List<GameObject> aiPlayerObjects;

            for(int i = 0; i < )
             * */
            foreach(GameObject player in AiPlayerObjects)
            {
                System.Random rnd = new System.Random();
                double turnDirection = rnd.NextDouble();

                if (turnDirection <= 0.5) // turn left
                {
                    PlayerBehaviors.turnPlayer(player, InputConstants.INPUT_LEFT);
                    if (turnDirection < AIConstants.BOOST_FREQ)
                    {
                        boostAIPlayer(player);
                    }
                }
                else // turn right
                {
                    PlayerBehaviors.turnPlayer(player, InputConstants.INPUT_RIGHT);
                    if (turnDirection > 1 - AIConstants.BOOST_FREQ)
                    {
                        boostAIPlayer(player);
                    }
                }
            }
        }

        public List<int> AiPlayers
        {
            get
            {
                return this.aiPlayers;
            }
            set
            {
                this.aiPlayers = value;
            }
        }
        public List<GameObject> AiPlayerObjects
        {
            get
            {
                return this.aiPlayerObjects;
            }
            set
            {
                this.aiPlayerObjects = value;
            }
        }

        public void boostAIPlayer(GameObject player)
        {
            StartCoroutine(PlayerBehaviors.activateSpeedBoost(player));
        }
    }
}
