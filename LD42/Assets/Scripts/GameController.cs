using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    #region board
    // ========== BOARD ==========
    [SerializeField] GameObject entityPrefab;
    [SerializeField] Colour neutralColour;
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

    public void StartGame() {
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
                GameObject instance = Instantiate(entityPrefab, new Vector3(c, r)+worldTransform.position, Quaternion.identity, worldTransform);
                board[r, c] = instance.GetComponent<Entity>();
            }
        }
    }

    /// <summary>
    /// Returns true if the given position is able to be spread/moved to by the given entity.
    /// </summary>
    /// <returns></returns>
    private bool CheckAhead(Entity entity, Vector2Int position)
    {
        // Check if attempting to move off the board
        if (position.x >= WIDTH || position.x < 0 || position.y >= HEIGHT || position.y < 0)
        {
            return false;
        }

        Entity nextCell = board[position.y, position.x];
        // Anything can move onto neutral tiles
        if (nextCell.type == Entities.NEUTRAL)
        {
            return true;
        }

        if (entity.type == Entities.PLAYER)
        {
            if (nextCell.type == Entities.TRAIL)
            {
                return true;
            }
        }
        else if (entity.type == Entities.BLIGHT)
        {
            if (nextCell.type == Entities.TRAIL)
            {
                if (!nextCell.colour.Equals(entity.colour))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void SetEntityType(Entities type, Vector2Int position, Colour colour)
    {
        board[position.y, position.x].type = type;
        board[position.y, position.x].SetEffectsActive(false);
        switch (type)
        {
            case Entities.NEUTRAL:
                board[position.y, position.x].SetSprite(neutralColour.trail);
                break;
            case Entities.PLAYER:
            case Entities.TRAIL:
                board[position.y, position.x].SetSprite(colour.trail);
                break;
            case Entities.BLIGHT:
                board[position.y, position.x].SetSprite(colour.blight);
                board[position.y, position.x].SetEffectsActive(true);
                break;
            default:
                Debug.LogWarning("Attempting to set unknown entity type: " + type);
                break;
        }
    }

    IEnumerator MoveCycle()
    {
        while (true) // TODO change this to playing game state or something
        {
            for (int i = 0; i < playersList.Count; i++)
            {
                PlayerScript player = playersList[i];
                Vector2Int oldPos = Vector2Int.RoundToInt(player.transform.position);
                Vector2Int newPos = oldPos + player.Direction;
                if (CheckAhead(player, newPos)) // if we can move
                {
                    // move the player
                    player.MoveStep();

                    // set the old position to a trail
                    SetEntityType(Entities.TRAIL, oldPos, player.colour);

                    board[newPos.y, newPos.x].type = Entities.PLAYER;
                }
            }

            yield return moveWaitTime;
        }
    }

    IEnumerator SpreadCycle()
    {
        /*
        while (true) // TODO change this to playing game state or something
        {
            Iterate over the board
            if the tile is BLIGHT:
                 add all available adjacent spaces to list
                pick X spaces from the list and spread to there
        }
         */
        yield return moveWaitTime;
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            GameObject playerInstance = Instantiate(PlayerPrefab, spawnPoints[i].position, Quaternion.identity, spawnPoints[i]);
            playerInstance.name = "Player_" + i;
            PlayerScript playerScript = playerInstance.GetComponent<PlayerScript>();
            playerScript.SetPlayerColour(playerColours[i]);
            playerScript.SetPlayerNumber(i);
            playersList.Add(playerScript);

            board[(int)spawnPoints[i].localPosition.y, (int)spawnPoints[i].localPosition.x].type = Entities.PLAYER;
        }
    }
	
}
