using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MenuChanges
{
    class DontDestroyOnLoad : MonoBehaviour
    {
        void Awake()
        { // called before Start()
            DontDestroyOnLoad(gameObject);

        }
    }
}
