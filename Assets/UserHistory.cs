using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
//this class is used to get/push a user's history information to/from firebase
public class UserHistory {

    public string Description;
    public string VisitDate;
    public string LocationName;

    public UserHistory(string description, string visitDate, string locationName)
    {
        this.Description = description;
        this.VisitDate = visitDate;
        this.LocationName = locationName;
    }
}
