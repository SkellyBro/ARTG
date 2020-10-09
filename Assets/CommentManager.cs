using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using FullSerializer;

//this class is for loading the correct comments from firebase and populating the comments in a scrollview
public class CommentManager : MonoBehaviour {

    //create new full serializer object for the data pulled from firebase, Unity's built in serializer isn't well equipped to deal with this
    //so this has to be used
    private static fsSerializer serializer = new fsSerializer();

    //database url for Firebase leading to the comments collection
    string databaseURL = "https://fir-test-46ee3.firebaseio.com/Comments";

    //get the idToken from the loginhandler so Firebase would allow interactions
    string idToken = LoginHandler.idToken;

    //this is to store the name of the location, so the correct sub-collection from the comments collection can be pulled
    string locationName;

    //user id
    public static string localID = LoginHandler.localId;
    //this is the gameobject that will be used to store the individual comments
    public GameObject commentPanel;
    //this determines how many columns the scroll will have
    private int columnCount = 1; 
    //this is a callback method
    public static GetUsersCallback callback;
    //this is to display the name of the location
    public Text header;

    //delegate method to ensure that data is returned as a dictionary
    public delegate void GetUsersCallback(Dictionary<string, MarkerCommentClass> users);
    
    // Use this for initialization
    void Start() {
        //get location name from counter and change variables
        locationName = Counter.locationName;
        header.text = Counter.locationName;
        /*
         Yo homie, none of this is mine. Like none, I had to watch a video then spend 3+ hours debugging to get this to work.
         Code adapted from: https://www.youtube.com/watch?v=TRLsmuYMs8Q
         Code author: Stuart Spence
         Code Accessed On: 22/09/2019
        
         
         */
        //this is to get the GUI rectangles to position the data
        RectTransform rowRectTransform = commentPanel.GetComponent<RectTransform>();
        RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();

        //this is to calculate the width and height of each item
        float width = containerRectTransform.rect.width/columnCount;
        float ratio = width / rowRectTransform.rect.width;
        float height = rowRectTransform.rect.height * ratio;
        
        //make connection to firebase and try to pull the stuff
        //these values are to orient the data on an x/y basis
        int j = 0;
        int i = 0;
        this.LoadComments(users => {

            int rowCount = users.Count / columnCount;
            if (users.Count % rowCount > 0)
                rowCount++;

            //adjust the height of the container so that it will just barely fit all its children
            float scrollHeight = height * rowCount;
            containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
            containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);

            foreach (var user in users)
            {
               
                if(i % columnCount == 0)
                {
                    j++;
                }

                //create new gameobject
                GameObject newItem = Instantiate(commentPanel) as GameObject;
                newItem.name = gameObject.name + " item at (" + i + "," + j + ")";
                newItem.transform.parent = gameObject.transform;


                /*
                 Code adapted from: https://answers.unity.com/questions/1464707/how-can-i-access-specific-children-of-a-prefabs-i.html
                 Code accessed on: 22/09/2019
                 Code Author: Legend_Bacon
                 
                 */
                 //loop through the commentPanel game object's children, and change content where the name of the child matches something
                for (int f=0; f< newItem.transform.childCount; f++)
                {
                    Transform currentItem = newItem.transform.GetChild(f);

                    //search by name
                    if (currentItem.name.Equals("CommenterName"))
                    {
                        //value change
                        currentItem.GetComponentInChildren<Text>().text = user.Value.CommenterFirstName+" "+user.Value.CommenterLastName;
                    }

                    if (currentItem.name.Equals("CommentDate"))
                    {
                        currentItem.GetComponentInChildren<Text>().text = user.Value.CommentDate;
                    }

                    if (currentItem.name.Equals("CommentBody"))
                    {
                        currentItem.GetComponentInChildren<Text>().text = user.Value.Comment;
                    }

                }

                //move and size the new item
                
                RectTransform rectTransform = newItem.GetComponent<RectTransform>();

                float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
                float y = containerRectTransform.rect.height / 2 - height * j;
                rectTransform.offsetMin = new Vector2(x, y);


                x = rectTransform.offsetMin.x + width;
                y = rectTransform.offsetMin.y + height;
                rectTransform.offsetMax = new Vector2(x, y);

                i++;
            }
        });

        

    }

    /*
     This code ain't mine homie. 
     Code Adapted from: https://medium.com/@rotolonico/firebase-database-in-unity-with-rest-api-42f2cf6a2bbf
     Code Author: Domenico Rotolo
     Code Accessed on: 21/09/2019
     */

    //this method is used to pull the comments from firebase
    //it takes the callback method as a parameter
    public void LoadComments(GetUsersCallback callback)
    {
        //database call
        RestClient.Get(databaseURL + "/" + locationName + ".json?auth=" + idToken).Then(response =>
        {
            //recieve the contents of the db call as a json
            var responseJson = response.Text;

            //parse the data
            var data = fsJsonParser.Parse(responseJson);

            //create new object to hold the deserialized data
            object deserialized = null;

            //try to deserialize the data following the dictionary definition + the data defintion of the MarkerCommentClass, referencing the data into the
            //deserialized object
            serializer.TryDeserialize(data, typeof(Dictionary<string, MarkerCommentClass>), ref deserialized);

            //store data as a dictionary
            var users = deserialized as Dictionary<string, MarkerCommentClass>;

            //this is used to actually create the dictionary
            callback(users);
        });

    }
}
