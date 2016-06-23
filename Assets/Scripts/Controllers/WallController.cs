using UnityEngine;
using System.Collections;

namespace Controller
{
    public class WallController : MonoBehaviour
    {
        private int playerID; //who does this wall belong to

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int PlayerID
        {
            get
            {
                return this.playerID;
            }
            set
            {
                this.playerID = value;
            }
        }
    }
}