//Language Headers
using System;

//Engine Headers
using UnityEngine;
using TMPro;

//Project Headers



public class TileHoverDisplay : MonoBehaviour
{
    TileInfo _tileInfo;
    [SerializeField] Camera _mainCamera; 
    [SerializeField] TextMeshProUGUI tileInfoText; 
    private GameObject _lastHoveredTile;
    Color32 _lastHoveredTileColour;
    [SerializeField] Color32 _tileHighlightColour = new Color32 (255,255,0,255);
    public static event Action OnTileHovered;

    void Awake()
    {

    }
    void OnEnable()
    {
        OnTileHovered += HighlightLastHoveredTile;
    }
    void OnDisable()
    {
        OnTileHovered -= HighlightLastHoveredTile;
    }
    void Start()
    {

    }
        

    void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                // Highlight the tile
                if (_lastHoveredTile != hit.collider.gameObject)
                {
                    if (_lastHoveredTile != null)
                        _lastHoveredTile.GetComponent<Renderer>().material.color = Color.white; // Reset the colour if tile is no longer highlighted

                    _lastHoveredTile = hit.collider.gameObject;
                    _lastHoveredTile.GetComponent<Renderer>().material.color = Color.yellow; // Highlight tile -> can also use Color32 but more tedious
                }

                tileInfoText.text = "Tile Info:" + "(" + tileInfo.x +"," +  tileInfo.y + ")";
                //OnTileHovered?.Invoke(); -> more complex implementation using 2 raycasts, also unneccesary
            }
        }
        else
        {
            if (_lastHoveredTile != null)
            {
                _lastHoveredTile.GetComponent<Renderer>().material.color = Color.white; // Reset colour to original
                _lastHoveredTile = null;
            }

            tileInfoText.text = "Tile Info: None";
        }
    }

    
    void FixedUpdate()
    {
        
    }

    void HighlightLastHoveredTile()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //_tileInfo = hit.collider.GetComponent<TileInfo>();

            if (_tileInfo != null)
            {
                // Highlight 
                if (_lastHoveredTile != hit.collider.gameObject)
                {
                    
                    if (_lastHoveredTile != null)
                        _lastHoveredTile.GetComponent<Renderer>().material.color = _lastHoveredTile.GetComponent<TileInfo>()._tileColour;
                        //reset the Tile colour
                    _lastHoveredTile = hit.collider.gameObject;

                    _lastHoveredTileColour = _lastHoveredTile.GetComponent<Renderer>().material.color ;
                    Debug.Log("Working");
                    
                    _lastHoveredTile.GetComponent<Renderer>().material.color = _tileHighlightColour; 
                }

                
            }
        }
        else
        {
            if (_lastHoveredTile != null)
            {
                _lastHoveredTileColour = Color.white; 
                _lastHoveredTile = null;
            }

            tileInfoText.text = "Tile Info: None";
        }
    }
}
