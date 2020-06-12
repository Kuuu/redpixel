using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	private static Sounds instance = null;

	AudioSource[] sources;

	AudioSource coin, jelly, door, jumper;

	public static Sounds Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);

		sources = GetComponents<AudioSource>();
		coin = sources[1]; // [0] is music
		jelly = sources[2];
		door = sources[3];
		jumper = sources[4];
	}

	public void PlayCoin() {
		coin.Play();
	}
	public void PlayJelly() {
		jelly.Play();
	}
	public void PlayDoor() {
		door.Play();
	}
	public void PlayJumper() {
		jumper.Play();
	}

}
