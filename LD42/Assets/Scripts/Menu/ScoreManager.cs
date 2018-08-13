using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    [SerializeField] Text[] player1Scores;
    [SerializeField] Text[] player2Scores;
    [SerializeField] Text[] player3Scores;
    [SerializeField] Text[] player4Scores;
    private Text[][] playerScores = new Text[4][];

    private void Awake()
    {
        playerScores = new Text[][] { player1Scores, player2Scores, player3Scores, player4Scores};
    }

    /// <summary>
    /// Updates all the score text boxes for all players
    /// </summary>
    /// <param name="scores"></param>
    public void UpdateScoreTexts(int[] scores)
    {
        for (int pIndex = 0; pIndex < GameController.MAXPLAYERS; pIndex++)
        {
            for (int i = 0; i < playerScores[pIndex].Length; i++)
            {
                // If the player is playing the game
                if (pIndex < scores.Length)
                {
                    // Enable the text box and set the score - doesn't matter that we will enable gameover textbox in game b.c the panel is disabled
                    playerScores[pIndex][i].gameObject.SetActive(true);
                    playerScores[pIndex][i].text = string.Format("P{0}: {1:0000000}", pIndex + 1, scores[pIndex]);
                }
                else
                {
                    // else disable the text box
                    playerScores[pIndex][i].gameObject.SetActive(false);
                }
            }
        }
    }
}
