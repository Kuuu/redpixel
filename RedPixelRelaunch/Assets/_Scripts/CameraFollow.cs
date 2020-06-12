using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	GameObject player;
    public float trackingSpeed = 8f;

    private Vector3 offset;
	private Vector3 newpos;

	void Start () 
	{
		player = GameObject.Find("player");
		offset = transform.position - player.transform.position;
		newpos = new Vector3 ();
	}

    private void FixedUpdate() {
        if (player != null) {
            var newPos = Vector2.Lerp(transform.position,
                player.transform.position,
                Time.deltaTime * trackingSpeed);
            var camPosition = new Vector3(newPos.x, newPos.y, -10f);
            var v3 = camPosition;
            //var clampX = Mathf.Clamp(v3.x, minX, maxX);
            //var clampY = Mathf.Clamp(v3.y, minY, maxY);
            //transform.position = new Vector3(clampX, clampY, -10f);
            transform.position = new Vector3(v3.x, v3.y, -10f);
        }
    }
}