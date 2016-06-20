using UnityEngine;
using System.Collections;
using Constants;

public class LoadMenuOnClick : MonoBehaviour {

    //public GameObject loadingImage;

    public void LoadScene(int level)
    {
       // loadingImage.SetActive(true);
        Application.LoadLevel(level);
    }
}
