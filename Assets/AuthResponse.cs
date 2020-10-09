using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AuthResponse
{

    //the purpose of this class is to store and make use of the 2 firebase variables that will be used for the authentication and the identification of users
    //localId: that acts as a primary key of sorts, mostly and only for identification purposes
    public string localId;

    //idToken: a token used by firebase to determine if someone is logged in or not
    public string idToken;

}
