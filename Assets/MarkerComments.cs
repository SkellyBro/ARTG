using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this class handles the navigation for the description scene onto the comment scene
public class MarkerComments : MonoBehaviour {

    private string scene = "CommentScene";

    public void onClick()
    {
        SceneManager.LoadScene(scene);
    }
}
