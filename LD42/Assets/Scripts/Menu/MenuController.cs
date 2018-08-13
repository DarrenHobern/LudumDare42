using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	[SerializeField] GameController gameController;
    [SerializeField] Selectable defaultSelection;
	[SerializeField] Text playerCountText;
    private const string defaultPlayerCountText = "▲\n{0}\n▼";
    private string[] numbers = { "ONE", "TWO", "THREE", "FOUR" };

	const int maxNumberOfPlayers = 4;
	int numberOfPlayers = 0;

    private void Start()
    {
        defaultSelection.Select();
    }

    public void ShowPause()
    {
        gameObject.SetActive(true);
        defaultSelection.Select();
    }

    // Update is called once per frame
    void Update () {
        
		float vertical = Input.GetAxisRaw("Vertical0");

        if (Input.GetButtonDown("Pause"))
		{
			QuitGame();
		}

        if (Input.GetButtonDown("Vertical0"))
        {
            numberOfPlayers = (maxNumberOfPlayers + (numberOfPlayers + (int)vertical) % maxNumberOfPlayers) % maxNumberOfPlayers;
            UpdatePlayerCountText();
        }
	}

	private void UpdatePlayerCountText()
    {
        playerCountText.text = string.Format(defaultPlayerCountText, numbers[numberOfPlayers]);
    }

    public void PlayGame()
    {
        GameData.instance.SetNumberOfPlayers(numberOfPlayers + 1);
        SceneManager.LoadScene(1);
    }

    public void ResumeGame()
    {
        if (gameController == null)
            return;

        gameController.ResumeGame();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

	//void ConfirmSelection(ButtonType buttonType)
	//{
	//	switch (buttonType)
	//	{
	//		case ButtonType.Start:
	//			GameData.instance.SetNumberOfPlayers(numberOfPlayers);
	//			SceneManager.LoadScene(1);
	//			break;
	//		case ButtonType.Continue:
	//			if (gameController != null)
	//			{
	//				gameController.playing = true;
	//				gameObject.SetActive(false);
	//			}
	//			break;
	//		case ButtonType.ReturnToMainMenu:
	//			SceneManager.LoadScene(0);
	//			break;
	//		case ButtonType.Quit:
	//			QuitGame();
	//			break;
	//	}
	//}

	public void QuitGame()
	{
		Application.Quit();
	}
}
