using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
//this is used to get location data from firebase
public class ObjectLocation {

    public int GPSLat;
    public int GPSLong;
    public int LikeCounter;
    public string LocationName;
    public string LocationTag;
    public string Description;

    public ObjectLocation()
    {
        Description = objectCounter.description;
        GPSLat = objectCounter.gPSLat;
        GPSLong = objectCounter.gPSLong;
        LikeCounter = objectCounter.likeCounter;
        LocationName = objectCounter.locationName;
        LocationTag = objectCounter.locationTag;
        
    }
}
