using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
//this class is used to get the location information of the GPS location through firebase
public class GPSLocation {

    public int GPSLat;
    public int GPSLong;
    public int LikeCounter;
    public string LocationName;
    public string LocationTag;
    public string Description;

    public GPSLocation()
    {
        Description = gpsCounter.description;
        GPSLat = gpsCounter.gPSLat;
        GPSLong = gpsCounter.gPSLong;
        LikeCounter = gpsCounter.likeCounter;
        LocationName = gpsCounter.locationName;
        LocationTag = gpsCounter.locationTag;
        
    }
}
