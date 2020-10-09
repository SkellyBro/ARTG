using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//this is used to get information about a location from firebase
public class Location{

    public int GPSLat;
    public int GPSLong;
    public int LikeCounter;
    public string LocationName;
    public string LocationTag;
    public string Description;

    public Location()
    {
        Description = Counter.description;
        GPSLat = Counter.gPSLat;
        GPSLong = Counter.gPSLong;
        LikeCounter = Counter.likeCounter;
        LocationName = Counter.locationName;
        LocationTag = Counter.locationTag;
       
    }

}
