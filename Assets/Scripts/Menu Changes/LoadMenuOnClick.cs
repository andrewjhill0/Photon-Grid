using UnityEngine;
using System.Collections;
using Constants;
using UnityEngine.SceneManagement;

public class LoadMenuOnClick : MonoBehaviour {

    //public GameObject loadingImage;

    public void LoadScene(int level)
    {
       // loadingImage.SetActive(true);
        SceneManager.LoadScene(level);
    }
}
