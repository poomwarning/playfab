using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class checkinternet : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }
        else
        {
             Debug.Log("internet connection!");
        }
    }
}
