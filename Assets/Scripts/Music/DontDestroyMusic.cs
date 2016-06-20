using UnityEngine;
using System.Collections;

public class DontDestroyMusic : MonoBehaviour {

    // Use this for initialization
    void Awake()
    { // called before Start()
        DontDestroyOnLoad(gameObject);

    }
}
