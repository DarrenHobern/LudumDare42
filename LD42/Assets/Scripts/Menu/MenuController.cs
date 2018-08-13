using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This should be broken into two different scripts for in game and the main menu
/// </summary>
public class MenuController : MonoBehaviour {

	[SerializeField] GameController gameController;

    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameoverMenu;

    [SerializeField] Selectable defaultMainmenuButton;
    [SerializeField] Selectable defaultPauseButton;
    [SerializeField] Selectable defaultGameoverButton;

    [SerializeField] Text playerCountText;
    private const string defaultPlayerCountText = "▲\n{0}\n▼";
    private string[] numbers = { "ONE", "TWO", "THREE", "FOUR" };
    private bool buttonReset = true;

    [SerializeField] ScoreManager scoreManager;

	int numberOfPlayers = 0;

    private void Start()
    {
        if (defaultMainmenuButton != null)
            defaultMainmenuButton.Select();
    }

    private void HideAllMenus()
    {
        pauseMenu.SetActive(false);
        gameoverMenu.SetActive(false);
        gameMenu.SetActive(false);
    }

    public void ShowGame()
    {
        HideAllMenus();
        gameMenu.SetActive(true);
    }

    public void ShowPause()
    {
        HideAllMenus();
        pauseMenu.SetActive(true);
        defaultPauseButton.Select();
    }

    public void ShowGameover()
    {
        HideAllMenus();
        gameoverMenu.SetActive(true);
        defaultGameoverButton.Select();
    }

    
    // Update is called once per frame
    void Update () {
        
		float vertical = Input.GetAxisRaw("Vertical0");

        if (Input.GetButtonDown("Pause"))
		{
			QuitGame();
		}

        
        if (vertical > 0 || vertical < 0)
        {
            if (buttonReset)
            {
                buttonReset = false;
                numberOfPlayers = (GameController.MAXPLAYERS + (numberOfPlayers + (int)vertical) % GameController.MAXPLAYERS) % GameController.MAXPLAYERS;
                UpdatePlayerCountText();
            }
        }
        else if (!buttonReset)
        {
            buttonReset = true;
        }
	}

	private void UpdatePlayerCountText()
    {
        playerCountText.text = string.Format(defaultPlayerCountText, numbers[numberOfPlayers]);
    }

    public void SetScoreText(int[] scores)
    {
        scoreManager.UpdateScoreTexts(scores);
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

	public void QuitGame()
	{
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }
}
