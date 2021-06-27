using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove : MonoBehaviour {

	Rigidbody2D rb;

	public float moveSpeed = 10;
	public float jumpPower = 3.2f;
	public int jumperPower = 10;
	public int conveyorPower = 4;
	int jumpMultiplier = 5000;
	int moveMultiplier = 100;

	bool standing; // If we're standing we can jump
	bool isJumperAllowed = true;

	bool jump, jumper, left, right;
	int saved_jump = 0;
	const int MAX_JUMP_SAVE = 3;

	public int dieY = -10; // Player will die if goes below this
	public static int ui_on; //1 = on, 2 = off
	public static DeviceType device_type;


	// Use this for initialization
	void Start () {
		CrossPlatformInputManager.SetButtonUp("Left");
		CrossPlatformInputManager.SetButtonUp("Right");
		CrossPlatformInputManager.SetButtonUp("Jump");
		Application.targetFrameRate = 60;
		Physics2D.gravity = new Vector2(0f, -9.81f);

		GameObject.Find("Toogle").GetComponent<CanvasRenderer>().SetAlpha(0.0F);

		rb = GetComponent<Rigidbody2D>();

		device_type = SystemInfo.deviceType;

		if (ui_on == 0) // If it wasn't set yet, set it to on
			ui_on = 1;

		DrawUI();
	}

	void Update() {

        // device_type = DeviceType.Handheld;

        //PHONE INPUT
        if (device_type == DeviceType.Handheld) {
			/*
			if (CrossPlatformInputManager.GetButtonDown ("Jump") && standing) {
				jump = true;
			}
			*/

			if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
				saved_jump = MAX_JUMP_SAVE;
			}

			if (CrossPlatformInputManager.GetButton ("Right")) {
				right = true;
			}
			if (CrossPlatformInputManager.GetButtonUp ("Right")) {
				right = false;
			}

			if (CrossPlatformInputManager.GetButton ("Left")) {
				left = true;
			}
			if (CrossPlatformInputManager.GetButtonUp ("Left")) {
				left = false;
			}
		}
			
		//END OF PHONE INPUT

		//COMPUTER INPUT
		if (device_type == DeviceType.Desktop) {
			/*
			if (Input.GetButtonDown ("Vertical") && standing) {
				jump = true;
			}
			*/

			if (Input.GetButtonDown ("Vertical")) {
				saved_jump = MAX_JUMP_SAVE;
			}

			if (Input.GetKey (KeyCode.RightArrow)) {
				right = true;
			} else {
				right = false;
			}

			if (Input.GetKey (KeyCode.LeftArrow)) {
				left = true;
			} else {
				left = false;
			}
		}

		// END OF COMPUTER INPUT


		if (CrossPlatformInputManager.GetButtonDown("ChangeLevel")) {
			SceneManager.LoadScene(0);
		}


		if (CrossPlatformInputManager.GetButtonDown("Toogle")) {
			if (ui_on == 1) {
				ui_on = 2;
			} else {
				ui_on = 1;
			}

			DrawUI();
		}



		if (transform.position.y <= dieY) {
			Die();
		}
	}

	void FixedUpdate() {

		if (saved_jump > 0) {
			if (standing) {
				jump = true;
				saved_jump = 0;
			}
			saved_jump--;
		} else {
			jump = false;
			saved_jump = 0;
		}


		if(jump) {
			rb.AddForce(new Vector2(0, jumpPower*jumpMultiplier * Time.deltaTime));
			jump = false;
			saved_jump = 0;
		}
		if(jumper) {
			rb.AddForce(new Vector2(0, jumperPower*jumpMultiplier * Time.deltaTime));
			jumper = false;
			isJumperAllowed = false;
		}
		if(left)
			rb.AddForce(new Vector2(-moveSpeed*moveMultiplier * Time.deltaTime, 0));
		if(right)
			rb.AddForce(new Vector2(moveSpeed*moveMultiplier * Time.deltaTime, 0));
	}






	public void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Spike" || col.tag == "Fireball") {
			Die();
		}

		if (col.tag == "Jumper" && !jumper && isJumperAllowed) {
			Sounds.Instance.PlayJumper();
			col.gameObject.GetComponent<Animator>().Play("Jump");
			jumper = true;
			isJumperAllowed = false;
		}

		if (col.tag == "Jelly") {
			Die();
			Destroy(col.gameObject.transform.GetChild(0).gameObject);


		}

		if (col.tag == "Laser") {
			if (col.GetComponent<LaserController>().on) {
				Die();
			}
		}

		if (col.tag == "JellyHead") {
			Sounds.Instance.PlayJelly();
			Destroy(col.gameObject.transform.parent.gameObject);
			Destroy(col.gameObject);
			rb.AddForce(new Vector2(0, jumpPower*jumpMultiplier*2 * Time.deltaTime));
		}

		if ((col.tag == "Key") && (col.gameObject.GetComponent<SpriteRenderer>().enabled == true)) {
			col.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			for (int i = 0; i < col.transform.childCount; i++) {
				col.transform.GetChild(i).GetComponent<Animator>().Play("MovingPlatform");
			}
		}

		if ((col.tag == "Lights") && (col.gameObject.GetComponent<SpriteRenderer>().enabled == true)) {
			col.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("Darkness").GetComponent<CanvasRenderer>().SetAlpha(0.0F);
		}
	}








	public void OnTriggerStay2D(Collider2D col) {
		if (col.tag == "Laser") {
			if (col.GetComponent<LaserController>().on) {
				Die();
			}
		}
	}







	public void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Jumper") {
			isJumperAllowed = true;
			jumper = false;
		}
	}







	public void OnCollisionEnter2D(Collision2D col) {
		if (col.collider.tag == "FallingPlatform") {
			standing = true;
			col.gameObject.GetComponent<PlatformFall>().Fall();
		}

		if (col.collider.tag == "Platform") {
			standing = true;
		}

		if (col.collider.tag == "MovingPlatform") {
			standing = true;
			transform.parent = col.gameObject.transform;
		}

		if (col.collider.tag == "RotatingPlatform") {
			standing = true;
		}

		if (col.collider.tag == "ConveyorLeft") {
			standing = true;
		}

		if (col.collider.tag == "ConveyorRight") {
			standing = true;
		}
	}







	public void OnCollisionStay2D(Collision2D col) {

		if (col.collider.tag == "Platform") {
			standing = true;
		}

		if (col.collider.tag == "MovingPlatform") {
			standing = true;
			transform.parent = col.gameObject.transform;
		}

		if (col.collider.tag == "FallingPlatform") {
			standing = true;
		}

		if (col.collider.tag == "ConveyorLeft") {
			standing = true;
			rb.AddForce(new Vector2(-conveyorPower * moveMultiplier * Time.deltaTime, 0));
		}

		if (col.collider.tag == "ConveyorRight") {
			standing = true;
			rb.AddForce(new Vector2(conveyorPower * moveMultiplier * Time.deltaTime, 0));
		}
	}







	public void OnCollisionExit2D(Collision2D col) {

		if (col.collider.tag == "Platform") {
			standing = false;
		}

		if (col.collider.tag == "MovingPlatform") {
			standing = false;
			transform.parent = null;
		}

		if (col.collider.tag == "FallingPlatform") {
			standing = false;
		}

		if (col.collider.tag == "RotatingPlatform") {
			standing = false;
		}

		if (col.collider.tag == "ConveyorLeft") {
			standing = false;
		}

		if (col.collider.tag == "ConveyorRight") {
			standing = false;
		}
	}   







	void Die() {
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	void DrawUI() {
		if (ui_on == 1) {
			GameObject.Find("JumpButton").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
			GameObject.Find("JumpButton/Text").GetComponent<CanvasRenderer>().SetAlpha(1.0f);

			GameObject.Find("LeftButton").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
			GameObject.Find("LeftButton/Text").GetComponent<CanvasRenderer>().SetAlpha(1.0f);

			GameObject.Find("RightButton").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
			GameObject.Find("RightButton/Text").GetComponent<CanvasRenderer>().SetAlpha(1.0f);

			GameObject.Find("CurrentTime").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
			GameObject.Find("BestTime").GetComponent<CanvasRenderer>().SetAlpha(1.0f);

			GameObject.Find("ChangeLevel").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
			GameObject.Find("ChangeLevel/Text").GetComponent<CanvasRenderer>().SetAlpha(1.0f);

			GameObject.Find("CurrentLevel").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
			GameObject.Find("CoinsLabel").GetComponent<CanvasRenderer>().SetAlpha(1.0f);
			GameObject.Find("Coins").GetComponent<CanvasRenderer>().SetAlpha(1.0f);

		} else {
			GameObject.Find("JumpButton").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
			GameObject.Find("JumpButton/Text").GetComponent<CanvasRenderer>().SetAlpha(0.0f);

			GameObject.Find("LeftButton").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
			GameObject.Find("LeftButton/Text").GetComponent<CanvasRenderer>().SetAlpha(0.0f);

			GameObject.Find("RightButton").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
			GameObject.Find("RightButton/Text").GetComponent<CanvasRenderer>().SetAlpha(0.0f);

			GameObject.Find("CurrentTime").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
			GameObject.Find("BestTime").GetComponent<CanvasRenderer>().SetAlpha(0.0f);

			GameObject.Find("ChangeLevel").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
			GameObject.Find("ChangeLevel/Text").GetComponent<CanvasRenderer>().SetAlpha(0.0f);

			GameObject.Find("CurrentLevel").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
			GameObject.Find("CoinsLabel").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
			GameObject.Find("Coins").GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		}
	}

}
