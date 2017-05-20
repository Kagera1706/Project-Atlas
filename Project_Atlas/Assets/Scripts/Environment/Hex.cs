using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour, ISelectable
{
    #region Attributes

    [SerializeField]
    private float radius = 5f;

    [SerializeField]
    private Renderer rend = null;
    [SerializeField]
    private Material outlineMat = null;

    public int Q { get; private set; } //column
    public int R { get; private set; } //row
    public int S { get; private set; } //spacing
    public int RawValue { get { return Q + R; } }

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;
    
    private TileType tileType;
    public TileType HexType
    {
        get { return tileType; }
        set
        {
            tileType = value;
            if (!gameObject.GetMaterial().name.Contains(tileType.ToString()))
                GetComponent<Renderer>().material = HexMap.Instance.TileMats[(int)tileType];
        }
    }

    public bool IsSelected { get; set; }

    #region Instance

    private GameObject instance;
    public GameObject Instance
    {
        get
        {
            if (!instance)
                instance = gameObject;
            return instance;
        }
    }

    #endregion

    #endregion

    #region UnityFunctions

    void Awake()
    {
        rend = GetComponent<Renderer>();
        //outlineShader = Shader.Find("Outlined/Outline Diffuse");//Resources.Load<Shader>("Shaders/OutlineDiffuse");
        
    }

    void Start()
    {
        outlineMat = Player.Instance.OutlineMat;
        //outlineMat.color = rend.material.color;
    }

    #endregion

    #region Coroutines



    #endregion

    #region Actions

    /// <summary>
    /// Initializes hex data
    /// </summary>
    public void Init(int q, int r, TileType type)
    {
        Q = q;
        R = r;
        S = -(q + r);

        InitPosition();
        name = ToString();
        HexType = type;
    }

    /// <summary>
    /// Initializes position
    /// </summary>
	public void InitPosition()
    {
        float height = radius * 2;
        float width = height * WIDTH_MULTIPLIER;

        float posY = height * (R * 0.75f);
        float posX = width * (Q + R / 2f);

        transform.position = new Vector3(posX, 0, posY);
    }

    /// <summary>
    /// Selects the tile
    /// </summary>
    public void Select()
    {
        Material otherMat = rend.material;
        rend.material = outlineMat;
        outlineMat = otherMat;
    }

    #endregion

    #region Functions

    /// <summary>
    /// Returns the distance in tiles between this tile and the one given as parameter
    /// </summary>
    public int DistanceTo(Hex hex)
    {
        //int abq = Mathf.Abs(Q - hex.Q);
        //int abqr = Mathf.Abs(Q + R - hex.Q - hex.R);
        //int abr = Mathf.Abs(R - hex.R) / 2;

        //Debug.Log(hex.ToString());
        //Debug.Log(abq);
        //Debug.Log(abqr);
        //Debug.Log(abr);

        //return (int)Math.Sqrt(abq + abqr + abr);

        int colDist = Math.Abs(Q - hex.Q);
        int rowDist = Math.Abs(R - hex.R);
        //int spaceDist = Math.Abs(S - hex.S);
        int distance = colDist + rowDist;//+ spaceDist;

        return distance;
    }

    /// <summary>
    /// Checks if this tile is within the given hex range
    /// </summary>
    public bool IsWithinRange(Hex hex, int range)
    {
        return Math.Abs(Q - hex.Q).IsWithin(0, range) && Math.Abs(R - hex.R).IsWithin(0, range) && Math.Abs(S - hex.S).IsWithin(0, range);
    }

    /// <summary>
    /// Returns the hex name
    /// </summary>
    public override string ToString()
    {
        return "Hex_" + Q + "_" + R;
    }

    #endregion
}
