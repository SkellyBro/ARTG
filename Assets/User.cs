using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//this class is used to get and push user data to/from firebase.
public class User
{

    public string userFirstName;
    public string userLastName;
    public string userEmail;
    public string localId;

    public User()
    {
        userFirstName = RegistrationHandler.fName;
        userLastName = RegistrationHandler.lName;
        userEmail = RegistrationHandler.uEmail;
        localId = RegistrationHandler.localId;

    }
}

