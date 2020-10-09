using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this class handles the navigation from the description scene onto the comment scene
public class ObjectComments : MonoBehaviour {

    private string scene = "ObjectCommentScene";

    public void onClick()
    {
        SceneManager.LoadScene(scene);
    }
}
