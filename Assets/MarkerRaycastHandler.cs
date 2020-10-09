using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Proyecto26;
using UnityEngine.EventSystems;

//this class is responsible for handling the marker based AR raycast.
public class MarkerRaycastHandler : MonoBehaviour {

    //variable for storing the scene that has the information for the selected thingy
    string descriptionScene = "descriptionScene";
    public static string tagName;

    // Update is called once per frame
    void Update () {

         if (Input.GetMouseButtonDown(0))
            {
                //this is where the magic happens
                //create a new raycast object
                RaycastHit hit;

                //create a ray called ray and have it take the position of the mouse
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //might need to look up the documentation on this one chief
                if (Physics.Raycast(ray, out hit, 5.0f))
                {
                //this is the major thing here, so for our project you'll need to change the tag of the plane or quad you'll have the images on 
                //and probably do this and apply it to a trackable or something
                //remember we have a bunch of resources on google that could help with this

                //the EventSystem code isn't mine
                /*
                 Code Accessed On: 13/09/2019
                 Code Author: fafase
                 Code Available at: https://answers.unity.com/questions/1079066/how-can-i-prevent-my-raycast-from-passing-through.html
                 
                 */

                /*
                 The first thing didn't work but provided a basis for this other thing, that did work.
                 Code Accessed On: 13/09/2019
                 Code Author: adhdchris
                 Code Available at: https://forum.unity.com/threads/ispointerovereventsystemobject-always-returns-false-on-mobile.265372/
                 
                 */

                if (hit.collider.gameObject.tag.Equals("MarkerPref") && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
                {
                    tagName = "MarkerPref";
                    SceneManager.LoadScene(descriptionScene);
                }
            }
        }
       
    }//end of update

}
