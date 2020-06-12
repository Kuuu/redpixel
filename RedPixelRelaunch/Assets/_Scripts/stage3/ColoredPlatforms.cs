using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoredPlatforms : MonoBehaviour {

	List<GameObject> platforms;

	List<GameObject> red;
	List<GameObject> blue;
	List<GameObject> yellow;
	List<GameObject> green;

	void Start() {
		
	}

	public void Init() {
		platforms = new List<GameObject>();
		red = new List<GameObject>();
		blue = new List<GameObject>();
		yellow = new List<GameObject>();
		green = new List<GameObject>();

		platforms.AddRange(GameObject.FindGameObjectsWithTag("ColoredPlatform"));
		platforms.AddRange(GameObject.FindGameObjectsWithTag("ColoredFallingPlatform"));

		foreach (GameObject platform in platforms) {
			switch (platform.GetComponent<ColorManager>().color) {
			case "red":
				red.Add(platform);
				break;
			case "blue":
				blue.Add(platform);
				break;
			case "yellow":
				yellow.Add(platform);
				break;
			case "green":
				green.Add(platform);
				break;
			}
		}

		platforms = null;
	}

	public void Adjust(string color) {
		switch (color) {
		case "red":
			foreach (GameObject platform in red) {
				platform.GetComponent<BoxCollider2D>().isTrigger = false;
			}

			foreach (GameObject platform in blue) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			foreach (GameObject platform in yellow) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			foreach (GameObject platform in green) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}
			break;

		case "green":
			foreach (GameObject platform in red) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			foreach (GameObject platform in blue) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			foreach (GameObject platform in yellow) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			foreach (GameObject platform in green) {
				platform.GetComponent<BoxCollider2D>().isTrigger = false;
			}
			break;

		case "blue":
			foreach (GameObject platform in red) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			foreach (GameObject platform in blue) {
				platform.GetComponent<BoxCollider2D>().isTrigger = false;
			}

			foreach (GameObject platform in yellow) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			foreach (GameObject platform in green) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}
			break;

		case "yellow":
			foreach (GameObject platform in red) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			foreach (GameObject platform in blue) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}

			foreach (GameObject platform in yellow) {
				platform.GetComponent<BoxCollider2D>().isTrigger = false;
			}

			foreach (GameObject platform in green) {
				platform.GetComponent<BoxCollider2D>().isTrigger = true;
			}
			break;
		}
	}
}
