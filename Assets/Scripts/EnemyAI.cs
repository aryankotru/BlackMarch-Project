using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, AI
{
    [SerializeField] float _moveSpeed = 2f;
    private Vector2Int _currentGridPosition;
    private Vector2Int _playerPosition;
    [SerializeField] ObstacleData _obstacleData; 
    [SerializeField] float _gridOffset = 1.1f;

    private bool _isMoving = false;
    private bool _isPlayerCurrentlyMoving = true;

    void Awake()
    {

    }
    void OnEnable()
    {
        PlayerMovement.OnPlayerMove += SetPlayerPosition;
    }

    void OnDisable()
    {
        PlayerMovement.OnPlayerMove -= SetPlayerPosition;
    }

    void Start()
    {
        _currentGridPosition = new Vector2Int(5, 5); // Starting position of the enemy, can [Serialize] it or something for multiple enemies
        transform.position = new Vector3(_currentGridPosition.x * 1.1f, 1.1f, _currentGridPosition.y * 1.1f);
    }

    void Update()
    {
        // Move if both player and enemy are not moving (basically if enemy has not started a new move cycle this frame)
        if (!_isMoving && !_isPlayerCurrentlyMoving)
        {
            Move();
        }
    }

    public void Move()
    {
        
        if (_obstacleData != null && _obstacleData._gridData != null)
        {
            
            Vector2Int targetPosition = GetAdjacentTileClosestToPlayer(); // Use the pathfinding script to get tile closest to the player whihc is also adjacent

            // Perform Check if the target position is not obstacle
            if (targetPosition != _currentGridPosition && !_obstacleData._gridData[targetPosition.x, targetPosition.y])
            {
                StartCoroutine(MoveToTile(targetPosition)); //use coroutine so other executions are not suspended
            }
        }
        else
        {
            Debug.Log("Obstacle data is not assigned or not initialized.");
        }

        _isPlayerCurrentlyMoving = true;
    }

    private Vector2Int GetAdjacentTileClosestToPlayer()
    {
        // Get the adjacent tiles to the player
        Vector2Int[] adjacentTiles = {
            _playerPosition + Vector2Int.up,
            _playerPosition + Vector2Int.down,
            _playerPosition + Vector2Int.left,
            _playerPosition + Vector2Int.right
        };

        Vector2Int bestTile = adjacentTiles[0];
        float shortestDistance = float.MaxValue;


        
        if (_obstacleData != null && _obstacleData._gridData != null)
        {
            foreach (var adjacentTile in adjacentTiles)
            {
                
                List<Vector2Int> path = Pathfinding.FindPath(_currentGridPosition, adjacentTile, _obstacleData._gridData);
                //use pathfinding script 
                if (path != null && path.Count < shortestDistance)
                {
                    bestTile = adjacentTile;
                    shortestDistance = path.Count; //initially path length is huge, with every "good" tile Found, update path to be shorter
                    
                }
            }
        }
        return bestTile;
        
    }

    private System.Collections.IEnumerator MoveToTile(Vector2Int targetPosition)
    {
        // isMoving = true;

        // Vector3 targetWorldPosition = new Vector3(targetPosition.x * 1.1f, 0.5f, targetPosition.y * 1.1f);
        // while (Vector3.Distance(transform.position, targetWorldPosition) > 0.1f)
        // {
        //     transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, moveSpeed * Time.deltaTime);
        //     yield return null;
        // }

        // currentGridPosition = targetPosition;
        // isMoving = false;

        _isMoving = true;

        List<Vector2Int> path = Pathfinding.FindPath(_currentGridPosition, targetPosition, _obstacleData._gridData);
        //perform pathfinding again because first time we just found the best tile, this time we are finding best route to best tile

        if (path == null)
        {
            Debug.Log("No path found!");
            _isMoving = false;
            yield break;
        }

        foreach (var position in path) //check the path grid by grid, one at a tine to get best route        
        {
            Vector3 targetWorldPosition = new Vector3(position.x * _gridOffset,_gridOffset , position.y * _gridOffset);

            while (Vector3.Distance(transform.position, targetWorldPosition) > Mathf.Epsilon) //keep moving as long as you are not at position
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, _moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        
        _currentGridPosition = targetPosition;
        _isMoving = false;
    }

    //is invoked every single time that player stops moving
    public void SetPlayerPosition(Vector2Int newPlayerPosition, bool isPlayerMoving)
    {
        _playerPosition = newPlayerPosition;
        Debug.Log(_playerPosition);
        _isPlayerCurrentlyMoving = isPlayerMoving;
    }
}
