using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using System;
using UnityEngine.SceneManagement;

//this class is similar to the CreateComment class, but has minor changes to accomodate for the GPS AR
//see the CreateComment class for a full documentation of the class
public class GPSCreateComment : MonoBehaviour {

    //variables to insert into firebase
    public InputField comment;
    string userFirstName = LoginHandler.firstName;
    string userLastName = LoginHandler.lastName;
    string date = DateTime.Now.ToString("MM/dd/yyyy");
    string location = gpsCounter.locationName;
    string idToken = LoginHandler.idToken;
    string localId = LoginHandler.localId;

    //firebase database URL
    string databaseURL = "https://fir-test-46ee3.firebaseio.com/Comments";
    string databaseURL2 = "https://fir-test-46ee3.firebaseio.com/Comments/IDCounter";

    int ID;

    //error check stuff
    int errorCount;
    string message;
    bool GPSPopup=false;
    bool ConfirmationPopup = false;

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;

        if (errorCount != 0)
        {
            GUI.contentColor = Color.red;
        }
        else
        {
            GUI.contentColor = Color.green;
        }

        if (GPSPopup)
        {
            GUI.skin.window.fontSize = 70;
            GUI.Window(0, new Rect(230, 900, 1000, 500), ShowError, "Errors Found!");
        }

        if (ConfirmationPopup)
        {
            GUI.skin.window.fontSize = 70;
            GUI.Window(0, new Rect(230, 900, 1000, 500), CommentConfirmation, "Comment Made!");
        }
    }

    public void GetID(string comment)
    {
        //getID from db
        RestClient.Get<CommentID>(databaseURL2 + "/" + location + ".json?auth=" + idToken).Then(response =>
        {
            ID = response.comID;
            Debug.Log("The Id for the comment is:" + ID);

            PushToDatabase(comment, ID);
        }).Catch(error => //this is an error to check if the registration was successful or not
        {

            Debug.Log(error);//this will display the error

        });
    }

    void CommentConfirmation(int windowID)
    {
        GUI.skin.label.fontSize = 60;

        GUI.Label(new Rect(90, 100, 1000, 600), message);

        // You may put a button to close the pop up too

        GUI.skin.button.fontSize = 50;

        /*
        This is for the && stuff in the if statement
        Code Accessed On: 13/09/2019
        Code Author: adhdchris
        Code Available at: https://forum.unity.com/threads/ispointerovereventsystemobject-always-returns-false-on-mobile.265372/
                 
        */
        if (GUI.Button(new Rect(300, 300, 400, 200), "Close"))
        {
            ConfirmationPopup = false;
            /*
             I need you to understand, tht I ahve no idea what I'm doing and Google is my teacher homie. 
             Code adapted from: https://answers.unity.com/questions/1109479/how-to-reload-a-scene-using-scenemanager.html
             Code accessed on: 23/09/2019
             Code Author: orvedal
             
             */
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }

    void ShowError(int windowID)
    {
        GUI.skin.label.fontSize = 60;

        GUI.Label(new Rect(90, 100, 1000, 600), message);

        // You may put a button to close the pop up too

        GUI.skin.button.fontSize = 50;

        /*
        This is for the && stuff in the if statement
        Code Accessed On: 13/09/2019
        Code Author: adhdchris
        Code Available at: https://forum.unity.com/threads/ispointerovereventsystemobject-always-returns-false-on-mobile.265372/
                 
        */
        if (GUI.Button(new Rect(300, 300, 400, 200), "Close"))
        {
            GPSPopup = false;
        }
    }

    public void OnSubmit()
    {
        Validate(comment.text.Trim());
    }

    public void PushToDatabase(string com, int ID)
    {
        //create new class to push into db
        MarkerCommentClass commentClass = new MarkerCommentClass(com, date, userFirstName, userLastName, location, localId);

        //the url for this has been modified, to take the ID as the key
        RestClient.Put(databaseURL + "/" + location + "/" + ID + ".json?auth=" + idToken, commentClass);

        //increment ID and push it back into DB
        ID++;
        CommentID commentID = new CommentID(ID);
        RestClient.Put(databaseURL + "/" + "IDCounter" + "/" + location + ".json?auth=" + idToken, commentID);

        message += "\n Comment made successfully!";
        ConfirmationPopup = true;
    }

    public void Validate(string comment)
    {
        errorCount = 0;

        if (comment.Length == 0)
        {
            errorCount++;
            message = "\nYour comment is empty!";
        }

        if (comment.Length > 200)
        {
            errorCount++;
            message = "\nYour comment cannot be more than 200 characters!";
        }

        if (errorCount == 0)
        {
            GetID(comment);
        }
        else
        {
            GPSPopup = true;
        }

    }

}
