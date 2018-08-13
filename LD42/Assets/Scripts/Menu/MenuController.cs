using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	[SerializeField] GameController gameController;
    [SerializeField] Selectable defaultSelection;
	[SerializeField] Text playerCountText;
    [SerializeField] Text[] scoreTexts;
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

    public void SetScoreText(int[] scores)
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            scoreTexts[i].text = string.Format("P{0}: {1:0000000}", i+1, scores[i]);
        }
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
