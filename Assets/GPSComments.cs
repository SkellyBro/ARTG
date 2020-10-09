using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this controls the navigation to the GPSComment screen
public class GPSComments : MonoBehaviour {

    private string scene = "GPSCommentScene";

    public void onClick()
    {
        SceneManager.LoadScene(scene);
    }
}
