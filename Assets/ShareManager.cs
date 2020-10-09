using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class handles the content sharing for the device using the NativeShare plugin
public class ShareManager : MonoBehaviour {

    //variables for the content on the page
    string subject;
    string title;
    string description;

    //this is to get the canvas and pull the title of the description
    public GameObject canvas;
    
	public void OnShare()
    {
        //get stuff from the canvas. 
        for (int f = 0; f < canvas.transform.childCount; f++)
        {
            Transform currentItem = canvas.transform.GetChild(f);

            //search by name
            if (currentItem.name.Equals("Title"))
            {
                title = currentItem.GetComponentInChildren<Text>().text;
            }

            //search by name
            if (currentItem.name.Equals("Description Scroller"))
            {
                description = currentItem.GetComponentInChildren<Text>().text;
            }

        }

        //set subject
        subject = "Historical Information on "+title;

        //create nativeshare object
        NativeShare native = new NativeShare();

        //set variables for nativeshare object
        native.SetSubject(subject);
        native.SetText(description);
        native.SetTitle(title);
        native.AddFile("Assets/Images/startscreen2.jpg", ".jpg");

        //try some freaky shit
        native.Share();


    }
}
