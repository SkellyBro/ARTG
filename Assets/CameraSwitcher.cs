using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this class handles the navigation on the camera screens
public class CameraSwitcher : MonoBehaviour {

    string imageScene = "CameraScene";
    string objectScene = "ObjectCamera";
    string historyScene = "HistoryScene";

	public void OnObjectSwitch()
    {
        SceneManager.LoadScene(imageScene);
    }

    public void OnImageSwitch()
    {
        SceneManager.LoadScene(objectScene);
    }

    public void OnHistory()
    {
        SceneManager.LoadScene(historyScene);
    }

}
