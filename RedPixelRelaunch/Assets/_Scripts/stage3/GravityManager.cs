using UnityEngine;
using System.Collections;

public class GravityManager : MonoBehaviour {

	public bool up;

	public void ChangeDirection() {
		if (up) {
			Physics2D.gravity = new Vector2(0f, 9.81f);
		} else {
			Physics2D.gravity = new Vector2(0f, -9.81f);
		}
	}
		
}
