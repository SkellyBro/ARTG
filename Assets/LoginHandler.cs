using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

//this class handles the login for the applicaiton and downloading assets from the server 
public class LoginHandler : MonoBehaviour {

    //declare variables to take stuff from the login form fields
    public InputField email;
    public InputField password;

    //Feedback Variables
    string message;
    string gpsMessage;
    int ErrorCount;

    //this is the login button itself
    public Button loginButton;

    //variables to store the user's name
    public static string firstName;
    public static string lastName;

    //variables for the scene change
    public float delay = 2;
    public string CameraScene = "CameraScene";

    //variable for storing the scenes downloaded from the webserver
    public static string[] scenePath;

    //variables for storing the latitude and longitude of the location
    float thisLat;
    float thisLong;

    //firebase variables for storing stuff 
    //this contains the URL to connect to the database
    private string databaseURL = "https://fir-test-46ee3.firebaseio.com/users";
    //this contains the authorization key for firebase
    private string AuthKey = "AIzaSyBFTXgN8qJimcQBJB2NRG-HXOqBVX9YvN4";
    //this contains the idToken that will be used to ensure that users are authenticated
    public static string idToken;
    //this is the Id that will be used to uniquely identify the various users stored on firebase
    public static string localId;

    //this is run on initialization
    void Start()
    {
        //this method starts the GPS service
        StartCoroutine(StartGPS());
    }

    //this is called once every frame
    private void Update()
    {
        //this will get the current latitude and longitude of the user
        thisLat = Input.location.lastData.latitude;
        thisLong = Input.location.lastData.longitude;

        //check to see if user has location services enabled
        if (Input.location.isEnabledByUser)
        {
            gpsMessage = "";
            loginButton.enabled = true;
        }
        else
        {
            gpsMessage = "\nPlease turn on your GPS";
            loginButton.enabled = false;
        }
    }

    //this is to get the user's name from Firebase on login
    private void GetUserName()
    {
        //database call
        RestClient.Get<User>(databaseURL + "/" + localId + ".json?auth=" + idToken).Then(response =>
        {
            //when this db call resolves, the data from the response, which is a user object, will be stored inside of these variables
            firstName = response.userFirstName;
            lastName = response.userLastName;

            message += "\nGetting user location...";

            //if the user is within this lat/long range assets would be downloaded
            
            if ((thisLat >= 10.288 && thisLat <= 10.289) && (thisLong <= -61.420 && thisLong >= -61.421))
            {
                //coroutine to download the assets from the webserver
                /*
                 Assets being:
                 Description Canvas Scenes
                 Comment Scenes

                 All other assets had to remain in-app due to coding considerations discussed in the report.
                 */
                StartCoroutine(LoadAssets());
                //coroutine to load the new scene after a few seconds
                StartCoroutine(LoadLevelAfterDelay(delay));
            }
            else if ((thisLat >= 10.276 && thisLat <= 10.280) && (thisLong <= -61.444 && thisLong >= -61.450))
            {
                StartCoroutine(LoadAssets());
                //coroutine to load the new scene after a few seconds
                StartCoroutine(LoadLevelAfterDelay(delay));

            }
            else if (true)
            {
                StartCoroutine(LoadAssets());
                //coroutine to load the new scene after a few seconds
                StartCoroutine(LoadLevelAfterDelay(delay));

            }
            else
            {
                ErrorCount++;
                message = "";
                message += "\nLogin failed, you are not in a location supported by the app!";
            }           
        });//end of RestClient get
    }

    //this function loads the assets from the server onto the device
    IEnumerator LoadAssets()
    {
        //create unnity web request to download content from server
        var uwr = UnityWebRequestAssetBundle.GetAssetBundle("http://urbansittertt.com/bundles/testarea");
        //this sends the web request 
        yield return uwr.SendWebRequest();
        //this gets the content downloaded from the server
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
        //this gets all the scenes from the bundle
        scenePath = bundle.GetAllScenePaths();

        if(scenePath.Equals(" "))
        {
            ErrorCount++;
            message += "\nAssets not downloaded, error encountered. Please contact administration for assistance";
        }
        else
        {
            message = "\nLocation assets downloaded!";
        }
        
        /*
         There is a weird behaviour with this. The assets downloaded are accessible via this script and the assets themselves do not exist in app.
         But, ideally you should have to access the content using the scenePath variable, instead unity loads the right scene based on code references.

         I don't completely understand it myself, but the scenes don't exist in the project and exist after this stuff downloads, Unity just handles it by itself.
         */
    }

    //coroutine to load level after successful registration
    /*
     
    Code Adapted from:https://answers.unity.com/questions/677406/loading-new-scene-after-time.html
    Code Accessed: 10/09/2019
    Code Author: Sir-Gatlin
        
    */
    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(CameraScene);
    }

    //feedback box
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 60;

        if (ErrorCount != 0)
        {
            GUI.contentColor = Color.red;
        }
        else
        {
            GUI.contentColor = Color.green;
        }

        //rect in this circumstance is shorthand for rectangle, the values sent in as parameters are the starting points and dimensions of the rectangle
        //the 30,30 is the position where its supposed to be and the 1000, 1000 is the dimensions 
        //this line of code seems to create a rectangle and place the earlier string message into it, to display it on screen. 
        GUI.Label(new Rect(230, 400, 1000, 500), message);
        GUI.Label(new Rect(230, 400, 1000, 500), gpsMessage);
    }

    //this handles the onlogin event
    public void OnLogin()
    {
        validation(email.text.Trim(), password.text.Trim());
        
    }

    IEnumerator StartGPS()
    {
        //check to see if user has location services enabled
        if (!Input.location.isEnabledByUser)
        {
            gpsMessage = "\nPlease turn on your GPS!";
            loginButton.enabled = false;
            yield break;
        }

        //start service before querying location
        Input.location.Start(5, 0);//accuracry and displacement values in meters

        //time to wait for service to initialize
        int maxWait = 5;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        //service did not initialize in 5 seconds
        if (maxWait < 1)
        {
            message = "\nGPS Service Timed Out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            message = "\nUnable to determine device location";
            yield break;
        }
        

    }

    //this method handles the login for the user
    public void SignInUser(string em, string pass)
    {

        //alot of this code isn't mine. 
        /*
         Code Adapted from: https://www.youtube.com/watch?v=dq8PAWLEo3E
         Code Author: uNicoDev
         Code Accessed On: 09/09/2019
         */

        //this string is effectively a JSON object with the user information captured from the application
        string userData = "{\"email\":\"" + em + "\", \"password\":\"" + pass + "\", \"returnSecureToken\":true}";

        //the userData string will then be passed to this url, for firebase to do its magical authentication stuff that still baffles me.
        RestClient.Post<AuthResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + AuthKey, userData).Then(response =>
        {

            idToken = response.idToken;
            localId = response.localId;
            message = "\nLogin Successful! You will be redirected to the main camera soon!";
            GetUserName();
        }).Catch(error => //this is an error to check if the registration was successful or not
        {
            ErrorCount ++;
            message = "\nLogin Failed! Please contact an administrator for assistance!";
            Debug.Log(error);//this will display the error

        });
    }

    //simple validation method to ensure that empty strings are not checked against the database
    public void validation(string em, string pass)
    {
        //set error count to 0
        ErrorCount = 0;

        //email validation
        if (em.Length == 0)
        {
            ErrorCount++;
            message += "\nYou must enter an email!";
        } 
        
        //email validation
        if (pass.Length == 0)
        {
            ErrorCount++;
            message += "\nYou must enter a password!";
        }

        //check to ensure there are no validation errors or GPS errors
        if (ErrorCount == 0)
        {
            SignInUser(em, pass);
        }
    }
}
