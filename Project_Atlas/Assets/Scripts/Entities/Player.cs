using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerAttributes
{
	public int health = 100;
	public int mana = 50;
	public float moveSpeed = 10f;
    public float rotationSpeed = 5f;
}

public class Player : MonoBehaviour, IMovable, ISelectable
{
    #region Attributes

    [SerializeField]
    private PlayerAttributes attributes;
    public PlayerAttributes Attributes { get { return attributes; } set { attributes = value; } }

    private int currentHealth = 0;
	private int currentMana = 0;

	public int MaxHealth { get { return Attributes.health; } set { Attributes.health = value; } }
    public int MaxMana { get { return Attributes.mana; } set { Attributes.mana = value; } }
    public float MoveSpeed { get { return Attributes.moveSpeed; } set { Attributes.moveSpeed = value; } }
    public float RealMoveSpeed { get { return MoveSpeed * Time.deltaTime; } }
    public float RotSpeed { get { return Attributes.rotationSpeed; } set { Attributes.rotationSpeed = value; } }
    public float RealRotSpeed { get { return RotSpeed * Time.deltaTime; } }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value > MaxHealth)
                value = MaxHealth;
            currentHealth = value;
            if(OnHealthChanged != null)
                OnHealthChanged(currentHealth);
        }
    }

    public int CurrentMana
    {
        get { return currentMana; }
        set
        {
            if (value > MaxMana)
                value = MaxMana;
            currentMana = value;
            if (OnManaChanged != null)
                OnManaChanged(currentMana);
        }
    }
    
    public bool IsMoving { get; set; }
    public bool IsRotating { get; set; }
    public bool IsSelected { get; set; }

    private Hex currentTile = null;
    public Hex CurrentTile
    {
        get { return currentTile; }
        set { currentTile = value; }
    }

    public delegate void OnValueChanged(int value);
    public event OnValueChanged OnHealthChanged;
    public event OnValueChanged OnManaChanged;

    #region GameObject

    [SerializeField]
    private Renderer rend = null;
    [SerializeField]
    private Material outlineMat = null;
    public Material OutlineMat { get { return outlineMat; } set { outlineMat = value; } }

    [SerializeField]
    private bool useOutliner = false;
    public bool UseOutliner { get { return useOutliner; } set { useOutliner = value; } }
    
    #endregion

    #region Instance

    private static Player instance = null;
    public static Player Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<Player>();
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
        outlineMat = Resources.Load<Material>("Materials/OutlineMaterial");
    }

    void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;

        transform.position = CurrentTile.Instance.GetPosition() - HexMap.Instance.transform.position;
        InitPosition();

        Camera.main.transform.position = CameraMgr.Instance.PlayerPos;
        //CurrentTile.Select();
        //outlineMat.color = rend.material.color;
    }


    void Update()
    {
		if (InputMgr.Instance.IsPaused)
			return;
        

    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Updates the transform position to match the given position using the given direction
    /// </summary>
    public IEnumerator Move(Vector3 position, Vector3 direction)
    {
        yield return new WaitWhile(() => IsRotating);
        
        while (IsMoving = (transform.position != position))
        {
            float distance = Mathf.Abs((position - transform.position).magnitude);
            Vector3 nextPosition = distance < 0.1f ? position : (transform.position + direction.normalized * RealMoveSpeed);
            yield return new WaitForFixedUpdate();
            transform.position = nextPosition;
        }

        InitPosition();
    }

    /// <summary>
    /// Updates the transform rotation to match the given rotation
    /// </summary>
    public IEnumerator Rotate(Quaternion rotation)
    {
        yield return new WaitWhile(() => IsMoving);

        while (IsRotating = (transform.rotation != rotation))
        {
            float angle = Mathf.Abs(Quaternion.Angle(transform.rotation, rotation));
            Quaternion nextRotation = angle < 1f ? rotation : Quaternion.Slerp(transform.rotation, rotation, RealRotSpeed);
            yield return new WaitForFixedUpdate();
            transform.rotation = nextRotation;
        }
    }

    #endregion

    #region Transformations

    /// <summary>
    /// Updates the transform
    /// </summary>
    void UpdateTransform(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        UpdateRotation(Quaternion.LookRotation(direction));
        UpdatePosition(position, direction);
    }

    /// <summary>
    /// Updates the position by starting a coroutine
    /// </summary>
    void UpdatePosition(Vector3 position, Vector3 direction)
	{
        IsMoving = true;
        StartCoroutine(Move(position, direction));
    }

    /// <summary>
    /// Updates the rotation by starting a coroutine
    /// </summary>
	void UpdateRotation(Quaternion direction)
	{
        IsRotating = true;
        StartCoroutine(Rotate(direction));
	}

    /// <summary>
    /// Updates the map by starting a coroutine
    /// </summary>
    void UpdateMap(bool hideRevealed)
    {
        StartCoroutine(HexMap.Instance.UpdateTiles(hideRevealed));
    }

    #endregion

    #region Actions

    /// <summary>
    /// Assigns the current tile the player is on.
    /// </summary>
    public void InitPosition()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return;
        
        CurrentTile = hit.collider.GetComponent<Hex>();
    }

    /// <summary>
    /// Selects the player
    /// </summary>
    public void Select()
    {
        if (!UseOutliner)
            return;

        Material otherMat = rend.material;
        rend.material = outlineMat;
        outlineMat = otherMat;
    }

    /// <summary>
    /// Selects a tile and moves the player if necessary
    /// </summary>
    public void MoveToTile(Hex tile)
    {
        if (IsMoving || IsRotating || CameraMgr.Instance.IsUpdating)
            return;
        
        Vector3 position = tile.Instance.GetPosition();

        //
        //List<GameObject> tilesOnTheWay = new List<GameObject>();

        //Ray rayban = new Ray(CurrentTile.GetPosition(), position - CurrentTile.GetPosition());
        //RaycastHit[] tilesHit = Physics.RaycastAll(rayban, (position - CurrentTile.GetPosition()).magnitude);
        //foreach(RaycastHit rh in tilesHit)
        //{
        //    GameObject latestTile = rh.collider.gameObject;
        //    Debug.Log(latestTile.name);

        //    tilesOnTheWay.Add(rh.collider.gameObject);
        //}


        position.y = transform.position.y;

        if (position == transform.position)
            return;

        UpdateTransform(position);
        UpdateMap(false);
    }

	#endregion

	#region Functions



    #endregion

}