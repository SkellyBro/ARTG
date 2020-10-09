using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
//this gets the likedetails for a location and is used to get data from Firebase
public class LikeDetails{

    public string localId;
    public string locationName;
    public string userFirstName;
    public string userLastName;

    public LikeDetails()
    {
        localId = Counter.likeLocalID;
        locationName = Counter.likeLocationName;
        userFirstName = Counter.likeUserFirstName;
        userLastName = Counter.likeUserLastName;
    }
	
}
