using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerScript : Entity {

    const string HORIZONTALINPUT = "Horizontal{0}";
    const string VERTICALINPUT = "Vertical{0}";

    [Tooltip("In tile per second")][SerializeField] int moveSpeed = 1;
    private string horizontalInput = "Horizontal0";
    private string verticalInput = "Vertical0";
    private Vector3Int direction = Vector3Int.up;

    public void SetPlayerColour(Colour playerColour)
    {
        colour = playerColour;
        // TODO change the player sprite to their colour
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

        if (horizontal > 0 || horizontal < 0)
        {
            direction = new Vector3Int((int)horizontal, 0, 0);
        }
        else if (vertical > 0 || vertical < 0)
        {
            direction = new Vector3Int(0, (int)vertical, 0);
        }
	}

    public void MoveStep()
    {
        transform.position = Vector3Int.RoundToInt(transform.position + direction);
        // Spawn a trail thing here?
    }
}
