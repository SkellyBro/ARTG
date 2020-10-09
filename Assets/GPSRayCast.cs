using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//this class handles the raycast to the GPS object
public class GPSRayCast : MonoBehaviour {

    string scene = "gpsDescription";
    public static string gpsTagName;

    // Update is called once per frame
    void Update () {
        //raycast stuff
        //this get the finger touch
        if (Input.GetMouseButtonDown(0))
        {
            //new raycast object
            RaycastHit hit;
            //ray created at mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if the raycast hits something
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                //check the tag of the object
                if (hit.collider.gameObject.tag == "GPSPref")

                {
                    //if the tag is the right one, set the tag name and load the description scene
                    gpsTagName = "GPSPref";
                    SceneManager.LoadScene(scene);
                }
            }

        }//end of if
    }
}
