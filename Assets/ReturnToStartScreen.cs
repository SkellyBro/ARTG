using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this class handles navigation back onto the start screen
public class ReturnToStartScreen : MonoBehaviour {

    public string StartScreen;

    public void OnReturn()
    {
        SceneManager.LoadScene(StartScreen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(StartScreen);
        }
           
    }
}
