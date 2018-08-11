using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerScript : Entity {

    const string HORIZONTALINPUT = "Horizontal{0}";
    const string VERTICALINPUT = "Vertical{0}";

    private SpriteRenderer spriteRenderer;

    [Tooltip("In tile per second")][SerializeField] int moveSpeed = 1;
    private string horizontalInput = "Horizontal0";
    private string verticalInput = "Vertical0";
    private Vector2Int direction = Vector2Int.up;
    public Vector2Int Direction
    {
        get
        {
            return direction;
        }
        private set
        {
            direction = value;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPlayerColour(Colour colour)
    {
        this.colour = colour;
        spriteRenderer.sprite = colour.playerSprite;
    }

    public void SetPlayerNumber(int number)
    {
        horizontalInput = string.Format(HORIZONTALINPUT, number);
        verticalInput = string.Format(VERTICALINPUT, number);
    }

	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxisRaw(horizontalInput);
        float vertical = Input.GetAxisRaw(verticalInput);

        if (horizontal > Mathf.Epsilon)
        {
            direction = Vector2Int.right;
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (horizontal < -Mathf.Epsilon)
        {
            direction = Vector2Int.left;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (vertical > Mathf.Epsilon)
        {
            direction = Vector2Int.up;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (vertical < -Mathf.Epsilon)
        {
            direction = Vector2Int.down;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        
    }

    public void MoveStep()
    {
        transform.position = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y);
    }
}
