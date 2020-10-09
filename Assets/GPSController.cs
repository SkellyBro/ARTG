using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Wikitude;

//this class is used to control the gps behavior of the app for the GPS AR stuff
public class GPSController : MonoBehaviour {

    //initial variables for storing startup message and GPS variables
    string message = "GPS Enabled!";

    //latitude and longitude variabels for the user's location
    float thisLat;
    float thisLong;

    //error variable
    int error;

    //GPS popup variable
    bool GPSPopup = false;
    public bool showGUI = true;

    //datetime variable for the last time the app updated the user's position
    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    //this is the object we'll be superimposing 
    public GameObject ARObject;
    public Trackable trackable;

    //this is the name of the scene that the user will be directed to
    string scene = "gpsDescription";

    int showGuiTimer = 1;

    //feedback information thats displayed on the user's device
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;

        if (error != 0)
        {
            GUI.contentColor = Color.red;
        }
        else
        {
            GUI.contentColor = Color.green;
        }

        GUI.Label(new Rect(230, 400, 1000, 500), message);

        //if this is true, display the popup
        if (GPSPopup)
        {
            GUI.skin.window.fontSize = 70;
            GUI.Window(0, new Rect(230, 900, 1000, 500), ShowGPSAR, "GPS AR Experience");
        }
    }

    void ShowGPSAR(int windowID)
    {
        // You may put a label to show a message to the player

        GUI.skin.label.fontSize = 60;

        GUI.Label(new Rect(90, 100, 1000, 450), "There is a GPS AR Experience nearby, would you like to view it?");

        // You may put a button to close the pop up too


        GUI.skin.button.fontSize = 50;

        /*
        This is for the && stuff in the if statement
        Code Accessed On: 13/09/2019
        Code Author: adhdchris
        Code Available at: https://forum.unity.com/threads/ispointerovereventsystemobject-always-returns-false-on-mobile.265372/
                 
        */
        //gui buttons that handle boolean decisions
        if (GUI.Button(new Rect(50, 300, 400, 200), "Yes") && trackable.enabled)
        {
            ARObject.SetActive(true);
            GPSPopup = false;
            showGUI = false;
            StartCoroutine(PopUpRefresher());

        }

        if (GUI.Button(new Rect(550, 300, 400, 200), "No") && trackable.enabled)
        {
            ARObject.SetActive(false);
            GPSPopup = false;
            showGUI = false;
            StartCoroutine(PopUpRefresher());
        }
    }
    
    //function responsible for starting the GPS service
    IEnumerator StartGPS()
    {
        message = "Starting";
        //check to see if user has location services enabled
        if (!Input.location.isEnabledByUser)
        {
            message = "Please turn on your GPS!";
            error++;
            yield break;
        }

        //start service before querying location
        Input.location.Start(5,0);//accuracry and displacement values in meters

        //time to wait for service to initialize
        int maxWait = 5;

        while(Input.location.status==LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        //service did not initialize in 5 seconds
        if(maxWait < 1)
        {
            message = "GPS Service Timed Out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            message = "Unable to determine device location";
            yield break;
        }
        else
        {
            Input.compass.enabled = true;

            message = "Lat: " + Input.location.lastData.latitude +
                "\nLong: " + Input.location.lastData.longitude +
                "\nAlt: " + Input.location.lastData.altitude +
                "\nHoriz Acc: " + Input.location.lastData.horizontalAccuracy +
                "\nVert Acc:" + Input.location.lastData.verticalAccuracy +
                "\n===========" +
                "\nHeading: " + Input.compass.trueHeading;
        }

    }
    
    //function for refreshing the value needed for the GPS popup to appear
   public IEnumerator PopUpRefresher()
    {
        showGuiTimer = 5;
        //if the GPS AR object isn't already active
        if (!ARObject.activeSelf)
        {
            while (showGuiTimer > 0)
            {
                yield return new WaitForSeconds(1);
                showGuiTimer--;//decay value 
            }
        }
    }

    // Use this for initialization
    void Start () {
        //start GPS on script start
        StartCoroutine(StartGPS());

    }
	
	// Update is called once per frame
	public void Update () {
        //this code is used to keep updating the information displayed on the device
        DateTime lastUpdate = epoch.AddSeconds(Input.location.lastData.timestamp);
        DateTime rightNow = DateTime.Now;

        thisLat = Input.location.lastData.latitude;
        thisLong = Input.location.lastData.longitude;
        message = "Device ready to find GPS AR Experiences." +
            "\nCurrent Lat:"+thisLat+"\nCurrent Long:"+thisLong;

        //if user is within this lat/longitude range display the gps popup
       if ((thisLat>=10.288 &&  thisLat<=10.289) && (thisLong <= -61.420 && thisLong >= -61.421) && showGUI == true)
        //if ((thisLat >= 10.28811 && thisLat <= 10.28815) && (thisLong <= -61.42051 && thisLong >= -61.42055) && showGUI == true)
        {
            //this would force the GPSPopup GUI thing to appear
            GPSPopup = true;
          
        }
        else if((thisLat >= 10.276 && thisLat <= 10.280) && (thisLong <= -61.444 && thisLong >= -61.450) && showGUI == true)
        {
            GPSPopup = true;
        }
        else
        {
            GPSPopup = false;
        }

       //set the gps popup value to true, which would push the GPS popup to the user's screen
        if (showGuiTimer == 0)
        {
            showGUI = true;
        }



    }//end of update
}
