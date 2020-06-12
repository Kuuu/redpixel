using UnityEngine;
using System.Collections;

public class PlatformFall : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Fall() {
		GetComponent<Animator>().Play("Fall");
	}
}
