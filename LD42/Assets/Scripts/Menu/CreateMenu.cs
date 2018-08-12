using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenu : MonoBehaviour {

	public enum ButtonTypes
	{
		Single,
		Left_Right
	}

	[System.Serializable]
	public struct MenuButton
	{
		public string name;
		public ButtonTypes type;
	}
	public List<MenuButton> buttonsList;
	// Use this for initialization
	void Start () {
		float menustart = 0.5f - (buttonsList.Count * 0.5f);
	}
}
