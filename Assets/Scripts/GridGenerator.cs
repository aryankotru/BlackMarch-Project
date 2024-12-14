//Language Headers
//Engine Headers
using UnityEngine;
//Project Headers

//https://github.com/ForlornU/GridGenerator/blob/main/Assets/Scripts/TileGenerator.cs
//[ExecuteInEditMode] -> to get a idea of how grid looks, only need to run once

public class GridGenerator : MonoBehaviour
{
    public GameObject TilePrefab; 
    [SerializeField] int _gridSizeX = 10;
    [SerializeField] int _gridSizeY = 10;
    [SerializeField] float _tileSpacing = 1.1f;

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
        GenerateGrid();
    }

    void Update()
    {

    }
    void GenerateGrid()
    {
        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                // calculate and set the position of current tile
                Vector3 position = new Vector3(x * _tileSpacing, 0, y * _tileSpacing);

                
                GameObject tile = Instantiate(TilePrefab, position, Quaternion.identity); // Spawn the tile

                
                TileInfo tileInfo = tile.GetComponent<TileInfo>();
                if (tileInfo != null)
                {
                    tileInfo.SetTileInfo(x, y); // Get the tile coords from the TileInfo script
                }

                // change parent of tile to the GridGenerator (not really neccessary but it looks messyu)
                tile.transform.parent = this.transform;
            }
        }
    }
}
