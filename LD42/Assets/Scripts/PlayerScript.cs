using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    [Tooltip("In tile per second")][SerializeField] int moveSpeed = 1;
    private Vector3Int direction = Vector3Int.up;
    private Colour playerColour = Colour.GREEN;

	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

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
        print("Moving player " + name);
        transform.position = transform.position + direction;
        // Spawn a trail thing here?
    }
}
