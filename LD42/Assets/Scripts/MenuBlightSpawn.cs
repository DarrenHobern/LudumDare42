using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBlightSpawn : MonoBehaviour {

	public GameObject blightTemplate;
	public int blightNumber;

	int WIDTHMIN = -9;
	int WIDTHMAX = 9;
	int HEIGHTMIN = -5;
	int HEIGHTMAX = 5;

	// Use this for initialization
	void Start () {
		List<Vector2> blightSpots = new List<Vector2>();
		for (int i = WIDTHMIN; i < WIDTHMAX; i++)
		{
			for(int j = HEIGHTMIN; j < HEIGHTMAX; j++)
			{
				blightSpots.Add(new Vector2(i,j));
			}
		}
		for(int n = 0; n < blightNumber; n++)
		{
			int randomInt = Random.Range(0, blightSpots.Count);
			Instantiate(blightTemplate, gameObject.transform).transform.position = blightSpots[randomInt];
			blightSpots.RemoveAt(randomInt);
		}
	}
}
