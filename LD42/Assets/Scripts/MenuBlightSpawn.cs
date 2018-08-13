using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBlightSpawn : MonoBehaviour {
    private enum Mode { FILL, RANDOM}

	[SerializeField] GameObject blightTemplate;
	[SerializeField] int blightNumber;
    [SerializeField] Mode fillMode = Mode.RANDOM;

	[SerializeField] int WIDTHMIN = -9;
    [SerializeField] int WIDTHMAX = 9;
    [SerializeField] int HEIGHTMIN = -5;
    [SerializeField] int HEIGHTMAX = 5;

	// Use this for initialization
	void Start () {
		List<Vector2> blightSpots = new List<Vector2>();
		for (int i = WIDTHMIN; i < WIDTHMAX; i++)
		{
			for(int j = HEIGHTMIN; j < HEIGHTMAX; j++)
			{
                Vector2 position = new Vector2(transform.localPosition.x + i * transform.localScale.x, transform.localPosition.y + j * transform.localScale.y);
                if (fillMode == Mode.RANDOM) {
                    blightSpots.Add(position);
                }
                else {
                    Instantiate(blightTemplate, position, Quaternion.identity, transform);
                }
			}
		}
        if (fillMode == Mode.RANDOM)
        {
            for (int n = 0; n < blightNumber; n++)
            {
                int randomInt = Random.Range(0, blightSpots.Count);
                GameObject instance = Instantiate(blightTemplate, blightSpots[randomInt], Quaternion.identity, transform);
                blightSpots.RemoveAt(randomInt);
            }
        }
	}
}
