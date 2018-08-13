using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBlightSpawn : MonoBehaviour {

	public GameObject blightTemplate;
	public int blightNumber;

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
				blightSpots.Add(new Vector2(transform.position.x + i*transform.localScale.x, transform.position.y + j*transform.localScale.y));
			}
		}
		for(int n = 0; n < blightNumber; n++)
		{
			int randomInt = Random.Range(0, blightSpots.Count);
            GameObject instance = Instantiate(blightTemplate, blightSpots[randomInt], Quaternion.identity, gameObject.transform);
			blightSpots.RemoveAt(randomInt);
		}
	}
}
