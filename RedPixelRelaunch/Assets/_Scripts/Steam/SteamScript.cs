using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Steam Initialized: " + SteamManager.Initialized);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
