using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using FullSerializer;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System;
using UnityEngine.SceneManagement;

//this handles the registration for the app
public class RegistrationHandler : MonoBehaviour {

    //these are the variables taken from the registration form
    public InputField firstname;
    public InputField lastname;
    public InputField email;
    public InputField password;
    public InputField confirmPassword;

    //error check variable
    int ErrorCount;

    //error message variable
    string message;
    string check = "true";

    public Button RegButton;

    //database variables to connect to firebase
    //this contains the URL to connect to the database
    private string databaseURL = "https://fir-test-46ee3.firebaseio.com/users";
    //this contains the authorization key for firebase
    private string AuthKey = "AIzaSyBFTXgN8qJimcQBJB2NRG-HXOqBVX9YvN4";
    //this contains the idToken that will be used to ensure that users are authenticated
    private string idToken;
    //this is the Id that will be used to uniquely identify the various users stored on firebase
    public static string localId;

    //these variables are responsible for handling the post to the database
    public static string fName;
    public static string lName;
    public static string uEmail;

    //serializer object to check firebase to check for duplicate emails in the database
    public static fsSerializer serializer = new fsSerializer();

    //variables for handling the delayed scene load to the login page 
    public float delay = 6;
    public string Login = "Login";

    //this code checks to see if the email is already in the database or not
    public void CheckEmail(string email)
    {

        RestClient.Get(databaseURL + ".json").Then(response =>
          {

              fsData userData = fsJsonParser.Parse(response.Text);

              Dictionary<string, User> users = null;
              serializer.TryDeserialize(userData, ref users);

              foreach (var user in users.Values)
              {
                  if (user.userEmail == email)
                  {
                      message += "\nEmail already exists in database!";
                      break;
                  }
              }
          });
    }

    //feedback box
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;

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
        GUI.Label(new Rect(230, 400, 1000, 600), message);
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
        SceneManager.LoadScene(Login);
    }

    //registration button submission handler
    public void OnSubmit()
    {
        //function to validate user data, to ensure firebase doesn't get empties 
        validation(firstname.text.Trim(), lastname.text.Trim(), email.text.Trim(), password.text.Trim(), confirmPassword.text.Trim());
    }

    private void PostToDatabase()
    {
        //this creates a new user object using the user class
        //the user class takes the userName and userScore from this class in a constructor and uses it below
        User user = new User();

        //the url for this has been modified, we added a /users/ to it, so the registered user would be stored in a subfolder called "users"
        RestClient.Put(databaseURL + "/" + localId + ".json?auth=" + idToken, user);

        message += "\nRegistration Successful! You will be directed to the login screen soon.";

        //coroutine to transfer the user to the login screen after successful registration
        StartCoroutine(LoadLevelAfterDelay(delay));
    }

    public void RegisterUser(string fn, string ln, string em, string pass)
    {
        //alot of this code isn't mine. 
        /*
         Code Adapted from: https://www.youtube.com/watch?v=dq8PAWLEo3E
         Code Author: uNicoDev
         Code Accessed On: 09/09/2019
         */

        //this is a JSON string that will be sent to firebase
        string userData = "{\"email\":\"" + em + "\", \"password\":\"" + pass + "\", \"returnSecureToken\":true}";
        //using the REST API the firebase url will act as an endpoint for the registration
        if (ErrorCount == 0) {
            Debug.Log("ErrorCount="+ErrorCount);
            RestClient.Post<AuthResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + AuthKey, userData).Then(response =>
            {

                idToken = response.idToken;
                localId = response.localId;
                fName = fn;
                lName = ln;
                uEmail = em;

                if (ErrorCount == 0)
                {
                    PostToDatabase();
                }

            }).Catch(error => //this is an error to check if the registration was successful or not
            {
                ErrorCount++;
                message += "\nRegistration Unsuccessful! Please contact an admininistrator for assistance!";
                RegButton.interactable = true;
                Debug.Log(error);//this will display the error


            });
        }
    }

    //method for handling the validation, the validation for this is pretty basic since the focus of the app is the AR stuff, not validation.
    public void validation(string fn, string ln, string em, string pass, string confPass)
    {
        //setting message to blank so messages don't append off the screen
        message = "";
        ErrorCount = 0;

        //firstname validation
        if (fn.Length == 0)
        {
            ErrorCount++;
            message += "\nYou must enter a firstname!";
        }

        if (fn.Length > 25)
        {
            ErrorCount++;
            message += "\nYour firstname cannot be more than 25 characters!";
        }
        else
        {
            /*
             Code Adapted From:https://stackoverflow.com/questions/5342375/regex-email-validation
             Code Accessed On: 09/09/2019
             Code Author: Avinash
             Code Editor: Brad Larson
             */

            /*
             Regex Adapted from: https://stackoverflow.com/questions/1181419/verifying-that-a-string-contains-only-letters-in-c-sharp
             Code Accessed On: 10/09/2019
             Code Author: Philippe Leybaert
             */
            Regex regex = new Regex(@"^[a-zA-Z]+$");
            Match match = regex.Match(fn);
            if (!match.Success)
            {
                ErrorCount++;
                message += "\nYour firstname can only contain letters!";
            }

        }

        //lastname validation
        if (ln.Length == 0)
        {
            ErrorCount++;
            message += "\nYou must enter a lastname!";
        }

        if (ln.Length > 25 )
        {
            ErrorCount++;
            message += "\nYou lastname cannot be more than 25 characters!";
        }
        else
        {
            /*
             Code Adapted From:https://stackoverflow.com/questions/5342375/regex-email-validation
             Code Accessed On: 09/09/2019
             Code Author: Avinash
             Code Editor: Brad Larson
             */

            /*
             Regex Adapted from: https://stackoverflow.com/questions/1181419/verifying-that-a-string-contains-only-letters-in-c-sharp
             Code Accessed On: 10/09/2019
             Code Author: Philippe Leybaert
             */
            Regex regex = new Regex(@"^[a-zA-Z]+$");
            Match match = regex.Match(ln);
            if (!match.Success)
            {
                ErrorCount++;
                message += "\nYour lastname can only contain letters!";
            }

        }

        //email validation
        if (em.Length == 0)
        {
            ErrorCount++;
            message += "\nYou must enter an email!";
        }
        else
        {
            /*
             Code Adapted From:https://stackoverflow.com/questions/5342375/regex-email-validation
             Code Accessed On: 09/09/2019
             Code Author: Avinash
             Code Editor: Brad Larson
             */
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(em);
            if (!match.Success)
            {
                ErrorCount++;
                message += "\nEmail is in wrong format!";
            }

        }

        //function to check if email exists in the database
        /*
        if (CheckEmail(em).Equals("false"))
        {
            ErrorCount++;
            message += "Email exists in database!";
        }
        else
        {
            Debug.Log("we fine");
        }
        */  
        

        //password validation
        if (pass.Length == 0)
        {
            ErrorCount++;
            message += "\nYou must enter a password!";
        }

        if (pass.Length < 7)
        {
            ErrorCount++;
            message += "\nYou must enter a password with more than 7 characters!";
        }

        if (!confPass.Equals(pass))
        {
            ErrorCount++;
            message += "\nPasswords do not match!";
        }


        if (ErrorCount == 0)
        {
            CheckEmail(em);
            RegButton.interactable = false;
            RegisterUser(fn, ln, em, pass);
        }


    }

}
