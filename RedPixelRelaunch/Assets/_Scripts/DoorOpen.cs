using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {

	public Sprite opened;

	public bool allow_enter;

	// Use this for initialization
	void Start () {
		allow_enter = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OpenYourself() {
		GetComponent<SpriteRenderer>().sprite = opened;
		allow_enter = true;
		Sounds.Instance.PlayDoor();
	}
}
