//Language Headers
using System;
using System.Collections;
using System.Collections.Generic;
//Engine Headers
using UnityEngine;
//Project Headers

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] ObstacleData _obstacleData;

    private Vector2Int _currentGridPosition;
    [SerializeField] float _gridOffset = 1.1f;
    private bool _isPlayerMoving;
    public static event Action<Vector2Int, bool> OnPlayerMove;
    
    void Awake()
    {

    }
    void OnEnable()
    {
        
    }
    void OnDisable()
    {
        
    }
    void Start()
    {
        // Initialize player's position on the grid
        _currentGridPosition = Vector2Int.zero;
        transform.position = new Vector3(_currentGridPosition.x * _gridOffset, _gridOffset, _currentGridPosition.y * _gridOffset);
    }

    void Update()
    {
        if (_isPlayerMoving) 
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                TileInfo tile = hit.collider.GetComponent<TileInfo>(); //get tile coords from raycast

                if (tile != null)
                {
                    Vector2Int targetGridPosition = new Vector2Int(); 

                    targetGridPosition.x = tile.x;
                    targetGridPosition.y = tile.y; //set target position in terms of tile coords

                    if (!_obstacleData._gridData[targetGridPosition.x, targetGridPosition.y])
                    {
                        StartCoroutine(MoveAlongPath(targetGridPosition)); //move to target pos
                    }
                }
            }
        }
    }

    private IEnumerator MoveAlongPath(Vector2Int targetPosition)
    {
        _isPlayerMoving = true;
        //Debug.Log("Is player moving: " + _isPlayerMoving);

        List<Vector2Int> path = Pathfinding.FindPath(_currentGridPosition, targetPosition, _obstacleData._gridData);
        //use a* to get path

        if (path == null)
        {
            Debug.Log("No path found!");
            _isPlayerMoving = false;
            yield break;
        }

        foreach (var position in path) //check the path grid by grid, one at a tine to get best route   
        {
            Vector3 targetWorldPosition = new Vector3(position.x * _gridOffset, _gridOffset, position.y * _gridOffset);

            while (Vector3.Distance(transform.position, targetWorldPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, _moveSpeed * Time.deltaTime);
                //keep moving as long as you are not at position
                yield return null;
            }
        }

        _currentGridPosition = targetPosition;
        OnPlayerMove?.Invoke(_currentGridPosition, false); //invoke C# Action event to update playerPos in EnemyAI
        _isPlayerMoving = false;
    }

    // public void ReturnPlayerPosition(Vector2Int newPlayerPosition)
    // {
    //     Debug.Log(_currentGridPosition);
    //     //return currentGridPosition;
    // }
}
