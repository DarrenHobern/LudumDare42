using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    #region board
    // ========== BOARD ==========
    [SerializeField] GameObject EntityPrefab;
    [SerializeField] Transform worldTransform;
    const int WIDTH = 18;
    const int HEIGHT = 10;
    Entity[,] board = new Entity[HEIGHT, WIDTH];
    #endregion

    #region Player
    // ========== PLAYERS ==========
    [SerializeField] Colour[] playerColours;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] int numberOfPlayers = 2; // TODO change this to be set in the menu
    [SerializeField] Transform[] spawnPoints;

    private List<PlayerScript> playersList = new List<PlayerScript>();
    #endregion

    #region Game Control
    [Tooltip("Time in seconds between move steps by the players")] [SerializeField] float moveTick;
    private WaitForSeconds moveWaitTime;
    [Tooltip("Time in seconds between blight spread events")] [SerializeField] float blightSpreadTick;
    private WaitForSeconds spreadWaitTime;
    #endregion


    // ========== FUNCTIONS ==========
    private void Awake()
    {
        // Error Checking
        Debug.Assert(PlayerPrefab.GetComponent<PlayerScript>() != null);
        Debug.Assert(playerColours.Length >= numberOfPlayers);

        moveWaitTime = new WaitForSeconds(moveTick);
        spreadWaitTime = new WaitForSeconds(blightSpreadTick);
    }

    private void Start()
    {
        StartGame();
    }

    void StartGame() {
        // Initialise the board
        GenerateBoard();
        // Spawn the players
        SpawnPlayers();
        StartCoroutine(MoveCycle());
        //StartCoroutine(SpreadCycle());
    }

    private void GenerateBoard()
    {
        for (int r = 0; r < HEIGHT; r++)
        {
            for (int c = 0; c < WIDTH; c++)
            {
                GameObject instance = Instantiate(EntityPrefab, new Vector3(c, r)+worldTransform.position, Quaternion.identity, worldTransform);
                board[r, c] = instance.GetComponent<Entity>();
            }
        }
    }

    IEnumerator MoveCycle()
    {
        while (true) // TODO change this to playing game state or something
        {
            for (int i = 0; i < playersList.Count; i++)
            { 
                playersList[i].MoveStep();
            }
            yield return moveWaitTime;
        }
    }

    IEnumerator SpreadCycle()
    {
        /*
        while (true) // TODO change this to playing game state or something
        {
            // TODO spread blight here
        }
         */
        yield return moveWaitTime;
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject playerInstance = Instantiate(PlayerPrefab, spawnPoints[i].position, Quaternion.identity, transform);
            playerInstance.name = "Player_" + i;
            PlayerScript playerScript = playerInstance.GetComponent<PlayerScript>();
            playerScript.SetPlayerColour(playerColours[i]);
            playerScript.SetPlayerNumber(i);
            playersList.Add(playerScript);

            //board[spawnPoints[i].y, spawnPoints[i].x] = playerScript;
        }
    }
	
}
