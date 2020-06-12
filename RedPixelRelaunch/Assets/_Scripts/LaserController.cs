using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	public bool on;
	AudioSource audiosource;
	bool playingsound;

	// Use this for initialization
	void Start () {
		on = false;
		audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (on) {
			if (!playingsound) {
				audiosource.Play();
				playingsound = true;
			}
		} else {
			audiosource.Stop();
			playingsound = false;
		}
	}
}
