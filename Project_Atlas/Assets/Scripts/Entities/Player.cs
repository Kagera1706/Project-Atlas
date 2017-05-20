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
    public float rotationSpeed = 2f;
}

public class Player : MonoBehaviour, IPausable, IMovable, ISelectable
{
	#region Attributes

	[SerializeField]
	private PlayerAttributes attributes;

	private int currentHealth = 0;
	private int currentMana = 0;

	public int MaxHealth { get { return attributes.health; } }
    public int MaxMana { get { return attributes.mana; } }
    public float MoveSpeed { get { return attributes.moveSpeed; } }
    public float RealMoveSpeed { get { return MoveSpeed * Time.deltaTime; } }
    public float RotSpeed { get { return attributes.rotationSpeed; } }
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

    private Hex currentTile = null;
    public Hex CurrentTile { get { return currentTile; } set { currentTile = value; } }

    private bool pause = false;

    public delegate void OnValueChanged(int value);
    public event OnValueChanged OnHealthChanged;
    public event OnValueChanged OnManaChanged;

    #region GameObject

    [SerializeField]
    private Renderer rend = null;
    [SerializeField]
    private Shader outlineShader = null;

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
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;

        rend = GetComponent<Renderer>();
        outlineShader = Resources.Load<Shader>("Shaders/OutlineDiffuse");
    }

    void Start()
    {
        transform.position = CurrentTile.Instance.GetPosition() - HexMap.Instance.transform.position;
        InitPosition();

        Camera.main.transform.position = CameraMgr.Instance.PlayerPos;
    }


    void Update()
    {
		if (pause)
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
            if (pause)
                continue;
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
            if (pause)
                continue;
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
    /// <param name="position"></param>
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
    /// Selects the player
    /// </summary>
    public void Select()
    {
        Shader tempShader = rend.material.shader;
        rend.material.shader = outlineShader;
        outlineShader = tempShader;
    }

    /// <summary>
    /// Selects a tile and moves the player if necessary
    /// </summary>
    public void MoveToTile(GameObject tile)
    {
        if (IsMoving || IsRotating || CameraMgr.Instance.IsUpdating)
            return;

        Vector3 position = tile.GetPosition();

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
        //

        if (position == transform.position)
            return;

        UpdateTransform(position);
        UpdateMap(false);
    }

	#endregion

	#region Functions

    public void InitPosition()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            CurrentTile = hit.collider.GetComponent<Hex>();
    }

	public void Pause()
	{
		pause = true;
	}

	public void Resume()
	{
		pause = false;
	}

    #endregion

}