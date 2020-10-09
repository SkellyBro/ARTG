using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this class handles navigation back onto the camera scenes
public class ReturnToCamera : MonoBehaviour {

    string loadLevel="CameraScene";

    public void Return()
    {
        SceneManager.LoadScene(loadLevel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(loadLevel);
        }
    }
}
