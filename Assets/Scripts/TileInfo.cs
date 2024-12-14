using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int x;
    public int y;

    public Color32 _tileColour = new Color32(255,255,255,255);

    void Start()
    {
        
    }
    public void SetTileInfo(int xCoord, int yCoord)
    {
        x = xCoord;
        y = yCoord;
        gameObject.name = $"Tile_{x}_{y}"; 
        GetComponent<Renderer>().material.color =  _tileColour;
        //set properties for each tile
    }
}
