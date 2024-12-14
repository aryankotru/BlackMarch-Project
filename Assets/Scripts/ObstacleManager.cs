using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;
    [SerializeField] float _gridOffset = 1.1f;

    private GameObject[,] obstacles = new GameObject[10, 10];

    void Start()
    {
        Debug.Log("Getting called");

        if (obstacleData == null || obstaclePrefab == null)
        {
            
            return;
        }

        GenerateObstacles();
    }
    void Update()
    {
        GenerateObstacles();
    }
    public void GenerateObstacles()
    {
        if(obstacleData._gridData.Length == 0) return; //dont encumber cpu if no obstacles are present

        ClearObstacles();

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (obstacleData._gridData[x, y])
                {
                    //Debug.Log($"Spawning obstacle at: (" + x + "," + y + ")");

                    Vector3 position = new Vector3(x * _gridOffset, _gridOffset, y * _gridOffset);

                    obstacles[x, y] = Instantiate(obstaclePrefab, position, Quaternion.identity, this.transform);
                }
            }
        }
    }

    public void ClearObstacles() //self explanatory but clear obstacle list and destroy obsatcles on screen
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (obstacles[x, y] != null)
                {
                    Destroy(obstacles[x, y]);
                    obstacles[x, y] = null;
                }
            }
        }
    }
}
