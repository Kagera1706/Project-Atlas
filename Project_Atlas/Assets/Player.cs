using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class PlayerAttributes
{
	public int health = 100;
	public int mana = 50;
	public float moveSpeed = 10f;
}

public class Player : MonoBehaviour, IPausable
{
	#region Attributes

	[SerializeField]
	private PlayerAttributes attributes;

	private bool pause = false;

	private int currentHealth = 0;
	private int currentMana = 0;

	public int CurrentHealth { get { return currentHealth; } set { if (value > MaxHealth) value = MaxHealth; currentHealth = value; } }
	public int MaxHealth { get { return attributes.health; } }
	public int CurrentMana { get { return currentMana; } set { if (value > MaxHealth) value = MaxHealth; currentHealth = value; } }
	public int MaxMana { get { return attributes.mana; } }
	public float MoveSpeed { get { return attributes.moveSpeed; } }
	public float RealSpeed { get { return Time.deltaTime * MoveSpeed; } }

	private Vector3 moveDirection = Vector3.zero;

	#endregion

	#region UnityFunctions

	void Awake()
    {

    }

    void Start()
    {

    }


    void Update()
    {
		if (pause)
			return;

		UpdatePosition();
		UpdateRotation();
		//StartCoroutine(UpdatePlayer());
    }
	
	#endregion
	
	#region Coroutines
	
	IEnumerator UpdatePlayer()
	{
		UpdatePosition();
		UpdateRotation();
		yield return new WaitForEndOfFrame();
	}
	
	#endregion
	
	#region Transformations
	
	void UpdatePosition()
	{
		moveDirection = Vector3.zero;
		moveDirection.x += Input.GetAxis("Horizontal");
		moveDirection.z += Input.GetAxis("Vertical");
		//moveDirection.Normalize();

		transform.position += moveDirection * RealSpeed;
	}

	void UpdateRotation()
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), RealSpeed);
	}

	#endregion

	#region Actions

	public void TakeDamage(int damage)
	{
		CurrentHealth -= damage;
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