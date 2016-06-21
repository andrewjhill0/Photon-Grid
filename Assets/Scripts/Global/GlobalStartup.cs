using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behaviors;
using UnityEngine;

namespace Global
{
    class GlobalStartup : MonoBehaviour
    {

        public Cooldowns cooldowns;
        // Use this for initialization
        void Start()
        {
            cooldowns = Cooldowns.Instance;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
