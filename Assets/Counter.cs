using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*This class controls the Liking functionality for Image Tracking AR and provides static variables to be used by other classes*/

public class Counter : MonoBehaviour {

    //variables for the location that will be collected from Firebase
    public static int gPSLat;
    public static int gPSLong;
    public static int likeCounter;
    public static string locationName;
    public static string locationTag;
    public static string description;

    //firebase urls and variables
    //this is a url that points to the like details collection
    private string databaseURL = "https://fir-test-46ee3.firebaseio.com/LikeDetails";

    //this url is for the location information
    private string databaseURL2 = "https://fir-test-46ee3.firebaseio.com/Location";

    //this url is for storing user history
    private string databaseURL3 = "https://fir-test-46ee3.firebaseio.com/UserHistory";

    //user login token for the database operations
    private string idToken = LoginHandler.idToken;

    string tag= MarkerRaycastHandler.tagName;

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
    void Start () {
        //on start get the location name of the location based on the tag
        GetLocation(tag);
	}
    
    //this method handles the liking functionality
    public void onLike()
    {
        //increments the likecounter value pulled from the db
        likeCounter++;

        //updates the database
        UpdateCounter();
    }


    public void GetDetails(string lN)
    {
        Debug.Log(locationName);
        RestClient.Get<LikeDetails>(databaseURL + "/" + lN + "/" + LoginHandler.localId + ".json?auth=" + idToken).Then(response =>
        {
            //this is just a check to see if the user had liked this place previously, if the user did like the place then a record will be found
            //and the like button's interactibility would be set to false, but if no record was found then the user is free to like the location.
            likeLocalID = response.localId;
            likeLocationName = response.locationName;
            likeUserFirstName = response.userFirstName;
            likeUserLastName = response.userLastName;

            likeButton.interactable = false;
            likeButton.GetComponentInChildren<Text>().text = "Liked";
        }).Catch(error => //this error does nothing, important. Its only here out of protocol
        {
            Debug.Log(error);//this will display the error

        });
    }

    //this function gets the location information of the user's current location based on the AR object scanned.
    public void GetLocation(string tag)
    {   //database get
        RestClient.Get<Location>(databaseURL2 + "/" + tag + ".json?auth=" + idToken).Then(response =>
        {
            gPSLat = response.GPSLat;
            gPSLong = response.GPSLong;
            likeCounter = response.LikeCounter;
            locationName = response.LocationName;
            locationTag = response.LocationTag;
            description = response.Description;

            //this will store this location in the user's visted history inside of the db
            StoreHistory(locationName, description, date);
            //this gets the like details of the location
            GetDetails(locationName);
        });
    }

    //this just updates the location with the new like value
    public void UpdateCounter()
    {
        Location loc = new Location();
        RestClient.Put<Location>(databaseURL2 + "/" + tag + ".json?auth=" + idToken, loc);

        //this is used to insert who liked the location into a separate collection
        InsertLikeDetails();
    }

    //this function inserts who liked the location into a collection, so a record of this information is kept
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

        //once the user liked the location the button is set to uninteractible and its text changed
        likeButton.interactable = false;
        likeButton.GetComponentInChildren<Text>().text = "Liked";
    }

    //this function stores the location in the user's visit history
    public void StoreHistory(string lN, string des, string date)
    {
        
        //put request to Firebase
        //create new UserHistory object to insert into firebase
        UserHistory history = new UserHistory(des, date, lN);

        //create insert date to act as the keys for each insert
        string insertDate = DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss");

        //insert into firebase for the history
        RestClient.Put<UserHistory>(databaseURL3+"/"+LoginHandler.localId+"/"+insertDate+".json?auth="+idToken, history);

    }

}
