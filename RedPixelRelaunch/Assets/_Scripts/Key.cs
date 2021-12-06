using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Sprite off;
    public bool isOn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOff()
    {
        GetComponent<SpriteRenderer>().sprite = off;
        isOn = false;
    }
}
