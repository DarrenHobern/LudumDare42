﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

	public static GameData instance;

	public float numberOfPlayers;

	// Use this for initialization
	void Awake () {
		if(instance == null)
		{
			instance = this;
		}
		else if (instance != null)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public void SetNumberOfPlayers(int number)
	{
		numberOfPlayers = number;
	}
}