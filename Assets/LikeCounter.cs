using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This whole class doesn't do anything, but my anxiety is preventing me from deleting it in case something breaks. 
 Nothing would happen, but I just don't have it in me to delete it.*/
public class LikeCounter : MonoBehaviour {
    public static int gPSLat;
    public static int gPSLong;
    public static int likeCounter;
    public static string locationName;
    public static string locationTag;
    //database variables to connect to firebase
    //this contains the URL to connect to the database
    private string databaseURL = "https://fir-test-46ee3.firebaseio.com/LikeDetails";

    private string idToken = LoginHandler.idToken;

    string location;

    //variable to turn the button on or off
    bool buttonStatus = true;

    //variables for the database get
    public static string likeLocalID; 
    public static string likeLocationName; 
    public static string likeUserFirstName; 
    public static string likeUserLastName;

    // Use this for initialization
    void Start () {
        // Debug.Log("Location Name is:"+locationName);

        //getLocation();
        getDetails();
       
    }

    public void onLike()
    {
        if (buttonStatus)
        {
            Debug.Log("True!");
        }
        else
        {
            Debug.Log("False!");
        }
    }

    public void getDetails()
    {

        RestClient.Get<LikeDetails>(databaseURL + "/"+"Place"+"/" +LoginHandler.localId+".json?auth=" + idToken).Then(response =>
        {
            likeLocalID = response.localId;
            likeLocationName = response.locationName;
            likeUserFirstName = response.userFirstName;
            likeUserLastName = response.userLastName;

            Debug.Log(likeLocalID);
            Debug.Log(likeLocationName);
            Debug.Log(likeUserFirstName);
            Debug.Log(likeUserLastName);

            buttonStatus = false;
        }).Catch(error => //this is an error to check if the registration was successful or not
        {
            buttonStatus = true;
            Debug.Log(error);//this will display the error

        });
    }
}

