using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove3 : MonoBehaviour {

	//COLOR STUFF
	public Sprite red;
	public Sprite green;
	public Sprite blue;
	public Sprite yellow;
	public string color;
	SpriteRenderer spriterend;

	//END


	Rigidbody2D rb;

	public float moveSpeed = 10;
	public float jumpPower = 3.2f;
	public int jumperPower = 10;
	int jumpMultiplier = 5000;
	int moveMultiplier = 100;

	bool standing; // If we're standing we can jump
	bool isJumperAllowed = true;
	bool gravity_up = false;

	bool jump, jumper, left, right;

	public int dieY = -10; // Player will die if goes below this


	// Use this for initialization
	void Start () {
        /*
		CrossPlatformInputManager.SetButtonUp("Left");
		CrossPlatformInputManager.SetButtonUp("Right");
		CrossPlatformInputManager.SetButtonUp("Jump");
        */
		Application.targetFrameRate = 60;

		rb = GetComponent<Rigidbody2D>();
		spriterend = GetComponent<SpriteRenderer>();
		gravity_up = false;
		Physics2D.gravity = new Vector2(0f, -9.81f);

		GameObject.Find("ColoredPlatforms").GetComponent<ColoredPlatforms>().Init();
		GameObject.Find("ColoredPlatforms").GetComponent<ColoredPlatforms>().Adjust(color);
	}

	void Update() {
			if (Input.GetButtonDown ("Vertical") && standing) {
				jump = true;
			}

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                right = true;
            }
            else
            {
                right = false;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                left = true;
            }
            else
            {
                left = false;
            }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        if (transform.position.y <= dieY) {
			Die();
		}
	}

	void FixedUpdate() {
		if(jump) {
			rb.AddForce(new Vector2(0, jumpPower*jumpMultiplier * Time.deltaTime));
			jump = false;
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
		if (col.tag == "Spike") {
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
			Destroy(col.gameObject);
			Destroy(col.gameObject.transform.parent.gameObject);
			rb.AddForce(new Vector2(0, jumpPower*jumpMultiplier*2 * Time.deltaTime));
		}

		if (col.gameObject.tag == "Dye") {
			ChangeTo(col.gameObject.GetComponent<ColorManager>().color);
			Destroy(col.gameObject);
			GameObject.Find("ColoredPlatforms").GetComponent<ColoredPlatforms>().Adjust(color);
		}

		if (col.gameObject.tag == "Gravitator") {
			col.gameObject.GetComponent<GravityManager>().ChangeDirection();
			if (gravity_up != col.gameObject.GetComponent<GravityManager>().up) {
				jumpPower = -jumpPower;
				jumperPower = -jumperPower;
				gravity_up = col.gameObject.GetComponent<GravityManager>().up;
			}
			Destroy(col.gameObject);
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
		if ((col.tag == "ColoredPlatform") || (col.tag == "ColoredFallingPlatform"))
		{
			standing = false;
		}
	}







	public void OnCollisionEnter2D(Collision2D col) {
		if (col.collider.tag == "FallingPlatform") {
			standing = true;
			col.gameObject.GetComponent<PlatformFall>().Fall();
		}

		if (col.collider.tag == "ColoredFallingPlatform") {
			standing = true;
			col.gameObject.GetComponent<PlatformFall>().Fall();
		}

		if (col.collider.tag == "Platform") {
			standing = true;
		}

		if ((col.collider.tag == "ColoredPlatform") || (col.collider.tag == "ColoredFallingPlatform")) {
			standing = true;
		}

		if (col.collider.tag == "MovingPlatform") {
			standing = true;
			transform.parent = col.gameObject.transform;
		}
	}









	public void OnCollisionStay2D(Collision2D col) {

		if (col.collider.tag == "Platform") {
			standing = true;
		}

		if ((col.collider.tag == "ColoredPlatform") || (col.collider.tag == "ColoredFallingPlatform")) {
			standing = true;
		}

		if (col.collider.tag == "MovingPlatform") {
			standing = true;
			transform.parent = col.gameObject.transform;
		}

		if (col.collider.tag == "FallingPlatform") {
			standing = true;
		}
	}








	public void OnCollisionExit2D(Collision2D col) {

		if (col.collider.tag == "Platform") {
			standing = false;
		}

		if (col.collider.tag == "ColoredPlatform") {
			standing = false;
		}

		if (col.collider.tag == "ColoredFallingPlatform") {
			standing = false;
		}

		if (col.collider.tag == "MovingPlatform") {
			standing = false;
			transform.parent = null;
		}

		if (col.collider.tag == "FallingPlatform") {
			standing = false;
		}
	}   









	void Die() {
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}




	//										 COLOR STUFF


	public void ChangeTo(string new_color) {
		switch (new_color) {
		case "red":
			color = "red";
			spriterend.sprite = red;
			break;
		case "green":
			color = "green";
			spriterend.sprite = green;
			break;
		case "blue":
			color = "blue";
			spriterend.sprite = blue;
			break;
		case "yellow":
			color = "yellow";
			spriterend.sprite = yellow;
			break;
		}
	}

}
