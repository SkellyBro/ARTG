using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Wikitude;

//performs the same action as the logout class, but tailored for the object camera scene due to some minor differences between them
public class ObjectCameraLogout : MonoBehaviour {
    //this si the scene that the user will be redirected to
    public string StartScreen = "StartScreen";

    //this is the variable that controls the popup
    bool showPopUp = false;

    //this controls the scripts responsible for the trackers
    public ObjectTracker objectTracker;

    //GPSController Gameobject
    public GameObject GPSController;


    //feedback box

    /*
     This popup code isn't mine.
    Code Accessed On: 13/09/2019
    Code Author: amirghayes
    Code Available at: https://forum.unity.com/threads/how-to-create-a-pop-up-window.96290/     
         
         */
    private void OnGUI()
    {
        if (showPopUp)
        {
            GUI.skin.window.fontSize = 70;
            GUI.Window(0, new Rect(230, 1200, 1000, 500), ShowGUI, "Logout Confirmation");
        }
    }

    void ShowGUI(int windowID)
    {
        // You may put a label to show a message to the player

        GUI.skin.label.fontSize = 60;

        GUI.Label(new Rect(90, 100, 1000, 300), "Are you sure you want to log out?");

        // You may put a button to close the pop up too


        GUI.skin.button.fontSize = 50;

        /*
        This is for the && stuff in the if statement
        Code Accessed On: 13/09/2019
        Code Author: adhdchris
        Code Available at: https://forum.unity.com/threads/ispointerovereventsystemobject-always-returns-false-on-mobile.265372/
                 
        */
        if (GUI.Button(new Rect(50, 200, 400, 200), "Yes"))
        {
            LoginHandler.idToken = null;
            LoginHandler.localId = null;
            LoginHandler.firstName = null;
            LoginHandler.lastName = null;

            SceneManager.LoadScene(StartScreen);
        }

        if (GUI.Button(new Rect(550, 200, 400, 200), "No"))
        {

            showPopUp = false;
            objectTracker.enabled = true;
            StartCoroutine(GPSController.GetComponent<GPSController>().PopUpRefresher());
        }

    }

    public void OnLogout()
    {
        objectTracker.enabled = false;
        showPopUp = true;
        GPSController.GetComponent<GPSController>().showGUI = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            objectTracker.enabled = false;
            showPopUp = true;
            GPSController.GetComponent<GPSController>().showGUI = false;
        }
    }
}
