using System;
using UnityEngine;

public class BadBrick : Brick
{
    public BadBrick(string resourceIdIn, GameObject startParent = null) : base(resourceIdIn, startParent)
    {
    }

    public BadBrick(string resourceIdIn, string brickPackIn, GameObject startParent = null) : base(resourceIdIn, brickPackIn, startParent)
    {
    }

    public void Init()
    {
        // Call the base initialization
        base._Init();

        // Additional initialization for BadBrick
        Debug.Log("BadBrick initialized with resource ID: " + resourceId);
    }
}