using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexMapData
{
    //public int width = 1;
    //public int height = 1;
    public int mapSize = 1;
    public int tilesWithin = 3;
    public GameObject tilePrefab = null;
    public Material[] tileMats = null;
}

public enum TileType
{
    Grass, Ground, Mountain, Plains, Water
}

public class HexMap : MonoBehaviour 
{
    #region Attributes

    [SerializeField]
    private HexMapData mapData;
    private Hex[,] hexes = null;
    public Hex[,] Map { get { return hexes; } }

    //[SerializeField]
    //private CameraMgr cam = null;
    [SerializeField]
    private Player player = null;
    [SerializeField]
    private bool hideOnStart = false;

    public int MapSize { get { return mapData.mapSize; } }
    public int HalfMap { get { return MapSize / 2; } }
    public int TilesWithin { get { return mapData.tilesWithin; } }
    public GameObject TilePrefab { get { return mapData.tilePrefab; } }
    public Material[] TileMats { get { return mapData.tileMats; } }

    #region Instance

    private static HexMap instance = null;
    public static HexMap Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<HexMap>();
            return instance;
        }
    }

    #endregion

    
    #endregion
    #region UnityFunctions

    void Awake()
    {
        player = Player.Instance;
        //cam = CameraMgr.Instance;
        GenerateHexes();
    }

    void Start ()
    {
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Updates the tiles on the map
    /// </summary>
    public IEnumerator UpdateTiles(bool hideRevealed)
    {
        yield return new WaitWhile(() => player.IsMoving);

        DisplaySurroundingTiles(hideRevealed);
    }

    #endregion

    #region Actions

    /// <summary>
    /// Generates the map and displays it
    /// </summary>
    public void GenerateHexes()
    {
        TileType[] hexTypes = (TileType[])Enum.GetValues(typeof(TileType));
        hexes = new Hex[MapSize, MapSize];
        for (int column = 0; column < MapSize; column++)
        {
            for (int row = 0; row < MapSize; row++)
            {
                GameObject tile =
                    Instantiate
                    (
                        TilePrefab,
                        transform.position,
                        transform.rotation,
                        transform
                    );

                TileType hexType = RNG.Generate(hexTypes);
                hexes[column, row] = tile.GetComponent<Hex>();
                hexes[column, row].Init(column, row, hexType);
            }
        }

        Player.Instance.CurrentTile = hexes[HalfMap, HalfMap];
        StartCoroutine(UpdateTiles(hideOnStart));
    }

    /// <summary>
    /// Display the tiles surrounding the player
    /// </summary>
    public void DisplaySurroundingTiles(bool hideRevealed)
    {
        if (Map.Length == 0)
            return;

        Hex playerTile = player.CurrentTile;
        foreach (Hex tile in Map)
        {
            if(hideRevealed)
                tile.gameObject.SetActive(playerTile.IsWithinRange(tile, TilesWithin));
            else if(!tile.gameObject.activeSelf)
                tile.gameObject.SetActive(playerTile.IsWithinRange(tile, TilesWithin));
        }
    }

    #endregion

    #region Functions

   

    #endregion
}
