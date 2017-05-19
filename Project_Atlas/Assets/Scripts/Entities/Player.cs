using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class PlayerAttributes
{
	public int health = 100;
	public int mana = 50;
	public float moveSpeed = 10f;
    public float rotationSpeed = 2f;
}

public class Player : MonoBehaviour, IPausable, IMovable
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

    private bool pause = false;

    public delegate void OnValueChanged(int value);
    public event OnValueChanged OnHealthChanged;
    public event OnValueChanged OnManaChanged;

    #endregion

    #region UnityFunctions
    
    void Awake()
    {
        //OnHealthChanged += (val) => { GameObject.FindGameObjectsWithTag("Info")[0].GetComponent<Slider>(); };
        //OnManaChanged += (val) => { };
    }

    void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
    }


    void Update()
    {
		if (pause)
			return;
        
        CheckInputs();
    }

    #endregion

    #region Coroutines

    public IEnumerator Move(Vector3 position, Vector3 direction)
    {
        while (IsRotating)
            yield return new WaitForFixedUpdate();
        
        while (IsMoving = (transform.position != position))
        {
            float distance = Mathf.Abs((position - transform.position).magnitude);
            Vector3 nextPosition = distance < 0.1f ? position : (transform.position + direction.normalized * RealMoveSpeed);
            yield return new WaitForFixedUpdate();
            transform.position = nextPosition;
        }

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator Rotate(Quaternion rotation)
    {
        while (IsMoving)
            yield return new WaitForFixedUpdate();

        while (IsRotating = (transform.rotation != rotation))
        {
            float angle = Mathf.Abs(Quaternion.Angle(transform.rotation, rotation));
            Quaternion nextRotation = angle < 1f ? rotation : Quaternion.Slerp(transform.rotation, rotation, RealRotSpeed);
            yield return new WaitForFixedUpdate();
            transform.rotation = nextRotation;
        }

        yield return new WaitForSeconds(1f);
    }
    
    #endregion

    #region Transformations

    void UpdateTransform(Vector3 position)
    {
        if (position == transform.position)
            return;
        
        Vector3 direction = position - transform.position;
        UpdateRotation(Quaternion.LookRotation(direction));
        UpdatePosition(position, direction);
    }

    void UpdatePosition(Vector3 position, Vector3 direction)
	{
        IsMoving = true;
        StartCoroutine(Move(position, direction));
    }

	void UpdateRotation(Quaternion direction)
	{
        IsRotating = true;
        StartCoroutine(Rotate(direction));
	}

	#endregion

    #region Actions

    public void CheckInputs()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !IsMoving && !IsRotating)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (!Physics.Raycast(ray, out hit))
                return;

            GameObject tile = hit.collider.gameObject;
            Vector3 position = tile.GetPosition();
            position.y = transform.position.y;
            UpdateTransform(position);
        }
    }

	public void TakeDamage(int damage)
	{
        //Debug.Log("Losing " + damage);
		CurrentHealth -= damage;
        //Debug.Log("Currently at " + CurrentHealth + " HP");
	}

	#endregion

	#region Functions

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