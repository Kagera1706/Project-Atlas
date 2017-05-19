using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexMapData
{
    public int width = 1;
    public int height = 1;
    public GameObject tilePrefab = null;
    public Material[] tileMats = null;
}

public enum TileType
{
    Grass, Plains, Water, Mountain, Ground
}

public class HexMap : MonoBehaviour 
{
    #region Attributes

    [SerializeField]
    private HexMapData mapData;

    public int Width { get { return mapData.width; } }
    public int Height { get { return mapData.height; } }
    public GameObject TilePrefab { get { return mapData.tilePrefab; } }
    public Material[] TileMats { get { return mapData.tileMats; } }

    private TileType tileType = TileType.Ground;

    #endregion

    #region UnityFunctions

    void Start () 
	{
        GenerateHexes();
	}

    #endregion

    #region Actions

    public void GenerateHexes()
    {
        for (int column = 0; column < Width; column++)
        {
            for (int row = 0; row < Height; row++)
            {
                Hex hex = new Hex(column, row);
                
                GameObject tile = 
                    Instantiate
                    (
                        TilePrefab, 
                        hex.Position() + transform.position, 
                        transform.rotation, 
                        transform
                    );
                tile.name = hex.ToString();
                tile.GetComponentInChildren<Renderer>().material = RNG.Generate(TileMats);
            }
        }
    }

    #endregion

    #region Functions



    #endregion
}
