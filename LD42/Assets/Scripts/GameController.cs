using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    #region board
    // ========== BOARD ==========
    [SerializeField] GameObject entityPrefab;
    [SerializeField] Colour neutralColour;
    [SerializeField] Transform worldTransform;
    const int WIDTH = 36;
    const int HEIGHT = 20;
    Entity[,] gameBoard = new Entity[HEIGHT, WIDTH];
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
    // ========== GAME ==========
    [Tooltip("Time in seconds between move steps by the players")]
    [SerializeField] float moveTick;
    private WaitForSeconds moveWaitTime;

    [Tooltip("Time in seconds between blight spread events")]
    [SerializeField] float blightSpreadTick;
    private WaitForSeconds spreadWaitTime;

    [Tooltip("Number of adjacent entities blight will spread to each SpreadTick")]
    [SerializeField] int blightSpreadRate = 1;

    [Tooltip("Time in seconds between spawning a new piece of blight")]
    [SerializeField] float blightSpawnRate = 6;
    private WaitForSeconds spawnWaitTime;

    // PAUSING
    private bool playing = true;
    #endregion


    // ========== FUNCTIONS ==========
    private void Awake()
    {
        // Error Checking
        Debug.Assert(PlayerPrefab.GetComponent<PlayerScript>() != null);
        Debug.Assert(playerColours.Length >= numberOfPlayers);

        moveWaitTime = new WaitForSeconds(moveTick);
        spreadWaitTime = new WaitForSeconds(blightSpreadTick);
        spawnWaitTime = new WaitForSeconds(blightSpawnRate);
    }

    private void Start()
    {
        if (GameData.instance != null)
            numberOfPlayers = GameData.instance.numberOfPlayers;

        StartGame();
    }

    public void StartGame() {
        GenerateBoard();
        SpawnPlayers();

        StartCoroutine(MoveCycle());
        StartCoroutine(SpreadCycle());
        StartCoroutine(SpawnCycle());
    }

    private void GenerateBoard()
    {
        gameBoard = new Entity[HEIGHT, WIDTH];
        for (int r = 0; r < HEIGHT; r++)
        {
            for (int c = 0; c < WIDTH; c++)
            {
                GameObject instance = Instantiate(entityPrefab, new Vector3(c, r)+worldTransform.position, Quaternion.identity, worldTransform);
                Entity entity = instance.GetComponent<Entity>();
                entity.name = string.Format("{0}_{1},{2}", entity.type, c, r);
                gameBoard[r, c] = instance.GetComponent<Entity>();
            }
        }

        // Spawn a blight of each type
        for (int i = 0; i < numberOfPlayers; i++)
        {
            SpawnBlight(playerColours[i]);
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

        Entity nextCell = gameBoard[position.y, position.x];
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
                return (!entity.colour.Equals(nextCell.colour));
            }
        }
        return false;
    }

    private void SetEntityType(Entities type, Colour colour, Vector2Int position)
    {
        gameBoard[position.y, position.x].type = type;
        gameBoard[position.y, position.x].colour = colour;
        gameBoard[position.y, position.x].name = string.Format("{0}_{1},{2}", type, position.x, position.y);
        gameBoard[position.y, position.x].SetEffectsActive(false);
        switch (type)
        {
            case Entities.NEUTRAL:
                gameBoard[position.y, position.x].SetSprite(neutralColour.trail);
                break;
            case Entities.PLAYER:
            case Entities.TRAIL:
                gameBoard[position.y, position.x].SetSprite(colour.trail);
                break;
            case Entities.BLIGHT:
                gameBoard[position.y, position.x].SetSprite(colour.blight);
                gameBoard[position.y, position.x].SetEffectsActive(true);
                break;
            default:
                Debug.LogWarning("Attempting to set unknown entity type: " + type);
                break;
        }
    }

    IEnumerator MoveCycle()
    {
        while (true)
        {
            if (!playing)
                yield return null;

            for (int i = 0; i < playersList.Count; i++)
            {
                PlayerScript player = playersList[i];
                Vector2Int oldPos = Vector2Int.RoundToInt(player.transform.position);
                Vector2Int newPos = oldPos + player.Direction;
                if (CheckAhead(player, newPos)) // if we can move
                {
                    // move the player
                    player.MoveStep();

                    SetEntityType(Entities.TRAIL, player.colour, oldPos);                    
                    gameBoard[newPos.y, newPos.x].type = Entities.PLAYER;
                }
            }
            // TODO add score here
            yield return moveWaitTime;
        }
    }

    IEnumerator SpreadCycle()
    {
        while (true)
        {
            if (!playing)
                yield return null;

            yield return spreadWaitTime;

            Dictionary<Vector2Int, Entity> spreadPositions = new Dictionary<Vector2Int, Entity>();
            // Iterate over the gameBoard
            for (int r = 0; r < HEIGHT; r++)
            {
                for (int c = 0; c < WIDTH; c++)
                {
                    // Search for all blight entities
                    Entity entity = gameBoard[r, c];
                    if (entity.type == Entities.BLIGHT)
                    {
                        // Spread to valid entities
                        List<Vector2Int> validEntities = GetValidAdjacent(entity, new Vector2Int(c, r));
                        SpreadToEntities(spreadPositions, entity, validEntities);
                    }
                }
            }
            ApplySpread(spreadPositions);
            
        }
    }

    IEnumerator SpawnCycle()
    {
        while(true) // TODO change to playing
        {
            if (!playing)
                yield return null;

            yield return spawnWaitTime;
            SpawnBlight();
        }
    }

    private void SpawnBlight(Colour playerColour = null)
    {
        int row = Random.Range(0, HEIGHT-1);
        int col = Random.Range(0, WIDTH-1);
        Vector2Int position = new Vector2Int(col, row);

        if (playerColour == null)
        {
            int colourIndex = Random.Range(0, numberOfPlayers);
            playerColour = playerColours[colourIndex];
        }
        
        if (gameBoard[position.y, position.x].type == Entities.TRAIL || gameBoard[position.y, position.x].type == Entities.NEUTRAL)
        {
            SetEntityType(Entities.BLIGHT, playerColour, position);
        }
    }

    /// <summary>
    /// Sets the entities at all the stored positions in the given dictionary.
    /// </summary>
    /// <param name="spreadPositions"></param>
    private void ApplySpread(Dictionary<Vector2Int, Entity> spreadPositions)
    {
        foreach (Vector2Int pos in spreadPositions.Keys)
        {
            Entity entity = spreadPositions[pos];
            SetEntityType(entity.type, entity.colour, pos);
        }
    }

    /// <summary>
    /// Checks all the valid spaces adjacent to the given position on the gameBoard.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private List<Vector2Int> GetValidAdjacent(Entity entity, Vector2Int position)
    {
        List<Vector2Int> adjacencyList = new List<Vector2Int>();
        int startX = -1;
        int startY = -1;
        int endX = 1;
        int endY = 1;

        // Edge Checking
        if (position.x == 0)
            startX = 0;
        else if (position.x == WIDTH - 1)
            endX = 0;

        if (position.y == 0)
            startY = 0;
        else if (position.y == HEIGHT - 1)
            endY = 0;

        // Count adjacent in square around position
        for (int r = startY; r <= endY; r++)
        {
            for (int c = startX; c <= endX ; c++)
            {
                Vector2Int adjPos = new Vector2Int(c, r);
                if (adjPos == Vector2Int.zero) // ignore self
                    continue;

                if (CheckAhead(entity, position+adjPos))
                {
                    adjacencyList.Add(position+adjPos);
                }
            }
        }
        
        return adjacencyList;
    }

    /// <summary>
    /// Randomly selects a valid position to spread to number is set by the blightSpreadRate
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="positions"></param>
    private void SpreadToEntities(Dictionary<Vector2Int, Entity> spreadPositions, Entity entity, List<Vector2Int> validPositions)
    {
        for (int i = 0; i < blightSpreadRate; i++)
        {
            if (i >= validPositions.Count)
                return;

            int index = Random.Range(0, validPositions.Count);
            // If there is a conflict between two entities spreading to the same location,
            // randomly pick one to override the other
            if (spreadPositions.ContainsKey(validPositions[index]))
            {  
                float roll = Random.Range(0f, 1f);
                if (roll > 0.5f)
                {
                    spreadPositions[validPositions[index]] = entity;
                }
            }
            else
            {
                spreadPositions.Add(validPositions[index], entity);
            }
            validPositions.RemoveAt(index);
        }
    }

    /// <summary>
    /// Spawns all the players in the game.
    /// </summary>
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

            gameBoard[(int)spawnPoints[i].localPosition.y, (int)spawnPoints[i].localPosition.x].type = Entities.PLAYER;
        }
    }
	
}
