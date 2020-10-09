using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
//this class is used to get the comments of a location from firebase
public class MarkerCommentClass{

    public string Location;
    public string Comment;
    public string CommentDate;
    public string CommenterFirstName;
    public string CommenterLastName;
    public string localId;

    public MarkerCommentClass(string Comment, string CommentDate, string CommenterFirstName, string CommenterLastName, string Location, string localId)
    {

        this.Comment = Comment;
        this.CommentDate = CommentDate;
        this.CommenterFirstName = CommenterFirstName;
        this.CommenterLastName = CommenterLastName;
        this.Location=Location;
        this.localId = localId;
    }
}
