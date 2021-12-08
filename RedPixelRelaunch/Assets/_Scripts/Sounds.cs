using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	private static Sounds instance = null;

	AudioSource[] sources;

	AudioSource music, coin, jelly, door, jumper;

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
		music = sources[0];
		coin = sources[1];
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

	public void TurnSounds(bool isOn)
    {
		if (isOn)
        {
			coin.mute = false;
			jelly.mute = false;
			door.mute = false;
			jumper.mute = false;
        } else
        {
			coin.mute = true;
			jelly.mute = true;
			door.mute = true;
			jumper.mute = true;
		}
    }

	public void TurnMusic(bool isOn)
    {
		if (isOn)
        {
			music.mute = false;
        } else
        {
			music.mute = true;
        }
    }

}
