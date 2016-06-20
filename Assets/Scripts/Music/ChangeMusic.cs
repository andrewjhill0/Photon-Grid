using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour
{

    public AudioClip level2Music;

    private AudioSource source;

    // Use this for initialization
    void Awake() //called before start()
    {

        source = GetComponent<AudioSource>();
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 2)
        {
            source.clip = level2Music;
            source.Play();
        }
    }

}
