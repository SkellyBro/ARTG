using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Wikitude;
//this handles the logout for application
public class Logout : MonoBehaviour {
    //this si the scene that the user will be redirected to
    public string StartScreen = "StartScreen";

    //this is the variable that controls the popup
    bool showPopUp = false;

    //this controls the scripts responsible for the trackers
    public ImageTracker imageTracker;

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
            //sets everything to null and loads the start screen
            //apparently this is how you handle logout with the rest api from what I researched, it works but I'm unsure about this
            LoginHandler.idToken = null;
            LoginHandler.localId = null;
            LoginHandler.firstName = null;
            LoginHandler.lastName = null;

            SceneManager.LoadScene(StartScreen);
        }

        if (GUI.Button(new Rect(550, 200, 400, 200), "No"))
        {

            showPopUp = false;
            imageTracker.enabled = true;
            StartCoroutine(GPSController.GetComponent<GPSController>().PopUpRefresher());
            
        }

    }

    //this is for the actual logout button press
    public void OnLogout()
    {
        imageTracker.enabled = false;
        showPopUp = true;
        GPSController.GetComponent<GPSController>().showGUI = false;
    }

    //this is if the user clicks on the back button
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            imageTracker.enabled = false;
            showPopUp = true;
            GPSController.GetComponent<GPSController>().showGUI = false;
        }
    }
}
