using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
//this class gets the commentID of a comment and is used by the various create a comment classes
public class CommentID{

    public int comID;

    public CommentID(int counter)
    {
        this.comID = counter;
    }

}
