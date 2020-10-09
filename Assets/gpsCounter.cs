using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using System;

//this is the same as the counter class, just with some minor adjustments for the GPS AR
//see the counter class for the full comments
public class gpsCounter : MonoBehaviour {

    //variables for the location
    public static int gPSLat;
    public static int gPSLong;
    public static int likeCounter;
    public static string locationName;
    public static string locationTag;
    public static string description;

    private string databaseURL = "https://fir-test-46ee3.firebaseio.com/LikeDetails";
    private string databaseURL2 = "https://fir-test-46ee3.firebaseio.com/Location";
    //this url is for storing user history
    private string databaseURL3 = "https://fir-test-46ee3.firebaseio.com/UserHistory";

    private string idToken = LoginHandler.idToken;

    string tag = GPSRayCast.gpsTagName;

    //variable to turn the button on or off
    bool buttonStatus = true;

    //variables for the database get
    public static string likeLocalID;
    public static string likeLocationName;
    public static string likeUserFirstName;
    public static string likeUserLastName;

    //button
    public Button likeButton;

    //date for history
    string date = DateTime.Now.ToString("MM/dd/yyyy HH:mm");


    // Use this for initialization
    void Start()
    {

        GetLocation(tag);
    }

    //this is to handle the increment for the likes
    public void onLike()
    {
        likeCounter++;

        UpdateCounter();
    }


    public void GetDetails(string lN)
    {
        Debug.Log(locationName);
        RestClient.Get<LikeDetails>(databaseURL + "/" + lN + "/" + LoginHandler.localId + ".json?auth=" + idToken).Then(response =>
        {
            likeLocalID = response.localId;
            likeLocationName = response.locationName;
            likeUserFirstName = response.userFirstName;
            likeUserLastName = response.userLastName;

            Debug.Log(likeLocalID);
            Debug.Log(likeLocationName);
            Debug.Log(likeUserFirstName);
            Debug.Log(likeUserLastName);

            likeButton.interactable = false;
            likeButton.GetComponentInChildren<Text>().text = "Liked";
        }).Catch(error => //this error will do nothing, this "Error" is only to see if the user liked the location or not
        {
           
            Debug.Log(error);//this will display the error

        });
    }

    public void GetLocation(string tag)
    {
        RestClient.Get<GPSLocation>(databaseURL2 + "/" + tag + ".json?auth=" + idToken).Then(response =>
        {
            gPSLat = response.GPSLat;
            gPSLong = response.GPSLong;
            likeCounter = response.LikeCounter;
            locationName = response.LocationName;
            locationTag = response.LocationTag;
            description = response.Description;

            //this is to store the user's visit history
            StoreHistory(locationName, description, date);

            //this is to get the "LikeDetails" from firebase
            GetDetails(locationName);
        });
    }

    public void UpdateCounter()
    {
        GPSLocation loc = new GPSLocation();

        RestClient.Put<GPSLocation>(databaseURL2 + "/" + tag + ".json?auth=" + idToken, loc);

        InsertLikeDetails();
    }

    public void InsertLikeDetails()
    {
        //create new LikeDetails object
        LikeDetails likeDetails = new LikeDetails();
        likeDetails.localId = LoginHandler.localId;
        likeDetails.locationName = locationName;
        likeDetails.userFirstName = LoginHandler.firstName;
        likeDetails.userLastName = LoginHandler.lastName;

        //put to database
        RestClient.Put<LikeDetails>(databaseURL + "/" + locationName + "/" + LoginHandler.localId + ".json?auth=" + idToken, likeDetails);

        likeButton.interactable = false;
        likeButton.GetComponentInChildren<Text>().text = "Liked";
    }

    public void StoreHistory(string lN, string des, string date)
    {

        //put request to Firebase
        //create new UserHistory object to insert into firebase
        UserHistory history = new UserHistory(des, date, lN);

        //create insert date to act as the keys for each insert
        string insertDate = DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss");

        RestClient.Put<UserHistory>(databaseURL3 + "/" + LoginHandler.localId + "/" + insertDate + ".json?auth=" + idToken, history);

    }
}
