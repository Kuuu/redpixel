using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public Direction direction;
    public float speed = 3;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = 0, vertical = 0;

        if (direction == Direction.up)
        {
            vertical = 1;
        }
        else if (direction == Direction.down)
        {
            vertical = -1;
        }
        else if (direction == Direction.left)
        {
            horizontal = -1;
        }
        else if (direction == Direction.right)
        {
            horizontal = 1;
        }

        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "platformBouncer")
        {
            if (direction == Direction.up) direction = Direction.down;
            else if (direction == Direction.down) direction = Direction.up;
            else if (direction == Direction.left) direction = Direction.right;
            else if (direction == Direction.right) direction = Direction.left;
        }
    }
}
