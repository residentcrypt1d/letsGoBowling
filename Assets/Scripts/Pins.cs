using UnityEngine;

public class Pins : MonoBehaviour
{
    //Attach this script to all the bowling pins
    public bool isStandingUp()
    {
        if (Vector3.Dot(this.transform.up, Vector3.up) < 0.8f)
        {
            return false;
        }
        return true;
    }
}

