using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	[SerializeField]
	GameController gameController;
	public enum ButtonType
	{
		Start,
		Continue,
		Quit
	}

	[System.Serializable]
	public struct ButtonColours
	{
		public Color normalColour;
		public Color highlightedColour;
	}

	[System.Serializable]
	public struct MenuButton
	{
		public GameObject button;
		public ButtonType buttonType;
	}
	public ButtonColours buttonColour;
	public Text numberOfPlayersUIText;

	public List<MenuButton> buttons;

	int maxNumberOfPlayers = 4;
	int numberOfPlayers = 1;

	int currentIndex = 0;

	// Use this for initialization
	void Start () {
		ChangeGUI();
	}
	
	// Update is called once per frame
	void Update () {

		float horizontal = Input.GetAxisRaw("Horizontal0");
		float vertical = Input.GetAxisRaw("Vertical0");

		if (Input.GetButtonDown("Pause"))
		{
			QuitGame();
		}
		if (Input.GetButtonDown("Horizontal0"))
		{
			numberOfPlayers = (int)((numberOfPlayers + horizontal - 1 + maxNumberOfPlayers) % maxNumberOfPlayers) + 1;
			ChangeGUI();
		}
		if (Input.GetButtonDown("Vertical0"))
		{
			currentIndex += (int)vertical;
			currentIndex = (currentIndex + buttons.Count) % buttons.Count;
			ChangeGUI();
		}
		if (Input.GetButtonDown("Submit"))
		{
			ConfirmSelection(buttons[currentIndex].buttonType);
		}
	}

	void ChangeGUI()
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			//print(currentIndex);
			//print(i == currentIndex);
			if(i == currentIndex)
			{
				buttons[i].button.GetComponent<Image>().color = buttonColour.highlightedColour;
			}
			else
			{
				buttons[i].button.GetComponent<Image>().color = buttonColour.normalColour;
			}
		}
		numberOfPlayersUIText.text = numberOfPlayers.ToString();
		
	}

	void ConfirmSelection(ButtonType buttonType)
	{
		switch (buttonType)
		{
			case ButtonType.Start:
				GameData.instance.SetNumberOfPlayers(numberOfPlayers);
				SceneManager.LoadScene(1);
				break;
			case ButtonType.Continue:
				if (gameController != null)
				{
					gameController.playing = true;
					gameObject.SetActive(false);
				}
				break;
			case ButtonType.Quit:
				QuitGame();
				break;
		}
	}

	void QuitGame()
	{
		Application.Quit();
	}
}
