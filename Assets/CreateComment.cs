using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using System;
using UnityEngine.SceneManagement;
using FullSerializer;

//this class is used to handle the comment creation
public class CreateComment : MonoBehaviour {

    //variables to insert into firebase
    //this is the actual comment 
    public InputField comment;
    //variables for the insert into the database
    string userFirstName = LoginHandler.firstName;
    string userLastName = LoginHandler.lastName;
    string date = DateTime.Now.ToString("MM/dd/yyyy");
    string location = Counter.locationName;
    string idToken = LoginHandler.idToken;
    string localId = LoginHandler.localId;
    //variable to store the ID for the comment
    int ID;

    //create new serializer object
    private static fsSerializer serializer = new fsSerializer();
    //firebase database URL for the comments
    string databaseURL = "https://fir-test-46ee3.firebaseio.com/Comments";
    //firebase database URL for the comment ID counter that would control the IDCounter collection responsible for controling the ID's for the comment
    string databaseURL2 = "https://fir-test-46ee3.firebaseio.com/Comments/IDCounter";

    //error check stuff
    int errorCount;
    //feedback message stuff
    string message;
    bool feedbackPopup=false;
    bool ConfirmationPopup = false;
    //feedback GUI thing
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

        //error popup
        if (feedbackPopup)
        {
            GUI.skin.window.fontSize = 70;
            GUI.Window(0, new Rect(230, 900, 1000, 500), ShowError, "Errors Found!");
        }

        //feedback for the successful comment creation
        if (ConfirmationPopup)
        {
            GUI.skin.window.fontSize = 70;
            GUI.Window(0, new Rect(230, 900, 1000, 500), CommentConfirmation, "Comment Made!");
        }
    }
    //comment created popup
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
    //this handles the comment ID incrementing
    public void GetID(string comment)
    {
        //getID from db
        RestClient.Get<CommentID>(databaseURL2 + "/" + location + ".json?auth=" + idToken).Then(response =>
        {
            ID = response.comID;
            Debug.Log("The Id for the comment is:" + ID);

            //this pushes the comment and the ID to firebase
            PushToDatabase(comment, ID);
        }).Catch(error => //this is an error to check if the registration was successful or not
        {

            Debug.Log(error);//this will display the error

        });
    }

    //error popup
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
            feedbackPopup = false;
        }
    }

    //function for handling the button press
    public void OnSubmit()
    {
        Validate(comment.text.Trim());
    }

    //handles the actual push to the db
    public void PushToDatabase(string com, int ID)
    {
        //create new class to push into db
        MarkerCommentClass commentClass = new MarkerCommentClass(com, date, userFirstName, userLastName, location, localId);

        //the url for this has been modified, to take the ID as the key
        RestClient.Put(databaseURL+"/"+location+ "/" + ID + ".json?auth=" + idToken, commentClass);

        //increment ID and push it back into DB
        ID++;
        CommentID commentID = new CommentID(ID);
        RestClient.Put(databaseURL + "/" + "IDCounter" + "/" + location + ".json?auth=" + idToken, commentID);

        message += "\n Comment made successfully!";
        ConfirmationPopup = true;
    }

    //validation
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
            feedbackPopup = true;
        }

    }

}
