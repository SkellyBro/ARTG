using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using FullSerializer;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//this class handles pulling user visit history from firebase and populating a scrollview with the information
public class VisitHistory : MonoBehaviour {

    //url to access the user's history in Firebase
    private string databaseURL = "https://fir-test-46ee3.firebaseio.com/UserHistory";

    //gameobject to populate for the history information
    public GameObject historyPanel;

    //idtoken for the database pull
    string idToken = LoginHandler.idToken;

    //stuff for the scrolly thing
    private int columnCount = 1;

    //callback thingy
    public static GetHistoryCallback callback;

    //create new serializer using full serializer
    private static fsSerializer serializer = new fsSerializer();

    //delegate method to ensure that data is returned as a dictionary?
    public delegate void GetHistoryCallback(Dictionary<string, UserHistory> history);

    //this is for the natigation using the back button
    private string previousScene = "CameraScene";

    //this is for the gpsImage
    public Texture gpsTexture;

    //this is for the markerImage
    public Texture markerTexture;

    //this is for the objectImage
    public Texture objectTexture;

    // Use this for initialization
    void Start () {

        //this is to get the GUI rectangles to position the data
        RectTransform rowRectTransform = historyPanel.GetComponent<RectTransform>();
        RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();

        //this is to calculate the width and height of each item
        float width = containerRectTransform.rect.width / columnCount;
        float ratio = width / rowRectTransform.rect.width;
        float height = rowRectTransform.rect.height * ratio;

        int j = 0;
        int i = 0;
        this.LoadComments(history => {

            int rowCount = history.Count / columnCount;
            if (history.Count % rowCount > 0)
                rowCount++;

            //adjust the height of the container so that it will just barely fit all its children
            float scrollHeight = height * rowCount;
            containerRectTransform.offsetMin = new Vector2(containerRectTransform.offsetMin.x, -scrollHeight / 2);
            containerRectTransform.offsetMax = new Vector2(containerRectTransform.offsetMax.x, scrollHeight / 2);

            foreach (var record in history)
            {

                if (i % columnCount == 0)
                {
                    j++;
                }

                //create new gameobject
                GameObject newItem = Instantiate(historyPanel) as GameObject;
                newItem.name = gameObject.name + " item at (" + i + "," + j + ")";
                newItem.transform.parent = gameObject.transform;


                /*
                 Code adapted from: https://answers.unity.com/questions/1464707/how-can-i-access-specific-children-of-a-prefabs-i.html
                 Code accessed on: 22/09/2019
                 Code Author: Legend_Bacon
                 
                 */
                 //this loops through the gameobject to find its children and change the children where necessary
                for (int f = 0; f < newItem.transform.childCount; f++)
                {
                    Transform currentItem = newItem.transform.GetChild(f);

                    //search by name
                    if (currentItem.name.Equals("LocationName"))
                    {
                        //change location name
                        currentItem.GetComponentInChildren<Text>().text = record.Value.LocationName;
                    }

                    if (currentItem.name.Equals("VisitedDate"))
                    {
                        currentItem.GetComponentInChildren<Text>().text = record.Value.VisitDate;
                    }

                    if (currentItem.name.Equals("LocationDescription"))
                    {
                        currentItem.GetComponentInChildren<Text>().text = record.Value.Description;
                    }

                  //this code determines the location and changes the rawimage texture based on the location name
                    if (record.Value.LocationName.Equals("GPSPlace"))
                    {
                        if (currentItem.name.Equals("Image"))
                        {
                             currentItem.GetComponentInChildren<RawImage>().texture = gpsTexture;
                        }

                    }else if (record.Value.LocationName.Equals("Place"))
                    {
                        if (currentItem.name.Equals("Image"))
                        {
                            currentItem.GetComponentInChildren<RawImage>().texture = markerTexture;
                        }
                    }
                    else if(record.Value.LocationName.Equals("ObjectPlace"))
                    {
                        if (currentItem.name.Equals("Image"))
                        {
                            currentItem.GetComponentInChildren<RawImage>().texture = objectTexture;
                        }
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
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(previousScene);
        }
    }

    //this method gets all the locations that the user has visited
    //this method is used to pull the comments from firebase
    //it takes the callback method as a parameter
    public void LoadComments(GetHistoryCallback callback)
    {
        //database call
        RestClient.Get(databaseURL + "/" + LoginHandler.localId + ".json?auth=" + idToken).Then(response =>
        {
            //recieve the contents of the db call as a json
            var responseJson = response.Text;

            //parse the data
            var data = fsJsonParser.Parse(responseJson);

            //create new object to hold the deserialized data
            object deserialized = null;
            //try to deserialize the data following the dictionary definition + the data defintion of the MarkerCommentClass, referencing the data into the
            //deserialized object
            serializer.TryDeserialize(data, typeof(Dictionary<string, UserHistory>), ref deserialized);

            //store data as a dictionary
            var history = deserialized as Dictionary<string, UserHistory>;
            Debug.Log(history);
            Debug.Log("Right before the callback!");
            //this is used to actually create the dictionary
            callback(history);
        });

    }
}
