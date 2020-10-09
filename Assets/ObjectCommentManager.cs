using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using FullSerializer;

//this class performs the same function as the CommentManager class
public class ObjectCommentManager : MonoBehaviour {

    private static fsSerializer serializer = new fsSerializer();
    string databaseURL = "https://fir-test-46ee3.firebaseio.com/Comments";
    string idToken = LoginHandler.idToken;
    string locationName;

    //get comment stuff for later too
    public static string localID = LoginHandler.localId;
    //get UI stuff for later
    public GameObject commentPanel;
    //stuff for the scrolly thing
    public int columnCount = 1; 
    //callback thingy
    GetUsersCallback callback;
    //this is for the header
    public Text header;

    //delegate method to ensure that data is returned as a dictionary?
    public delegate void GetUsersCallback(Dictionary<string, MarkerCommentClass> users);
    
    // Use this for initialization
    void Start() {

        locationName = objectCounter.locationName;
        header.text = objectCounter.locationName;

        /*
         Yo homie, none of this is mine. Like none, I had to watch a video then spend 3+ hours debugging to get shit to work. 
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

        //adjust height of container so it could fit the children

        /*
        containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
        containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);
        */

        //make connection to firebase and try to pull the stuff
        int j = 0;
        int i = 0;
        this.LoadComments(users => {

            Debug.Log(users.Count);

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
                for (int f=0; f< newItem.transform.childCount; f++)
                {
                    Transform currentItem = newItem.transform.GetChild(f);

                    //search by name
                    if (currentItem.name.Equals("CommenterName"))
                    {
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



                Debug.Log(user.Value.Comment);
                Debug.Log(user.Value.CommentDate);
                Debug.Log(user.Value.CommenterFirstName);
                Debug.Log(user.Value.CommenterLastName);
                Debug.Log(user.Value.Location);
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


    public void LoadComments(GetUsersCallback callback)
    {
        Debug.Log("Location Name:"+locationName);
        RestClient.Get(databaseURL + "/" + locationName + ".json?auth=" + idToken).Then(response =>
        {
            var responseJson = response.Text;
            Debug.Log(responseJson);

            var data = fsJsonParser.Parse(responseJson);

            Debug.Log(data);
            object deserialized = null;

            serializer.TryDeserialize(data, typeof(Dictionary<string, MarkerCommentClass>), ref deserialized);

            var users = deserialized as Dictionary<string, MarkerCommentClass>;
            Debug.Log(users);
            Debug.Log("Right before the callback!");

            callback(users);
        });

    }
}
