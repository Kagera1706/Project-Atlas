using UnityEngine;
using System;
using System.Collections;

public class CameraMgr : MonoBehaviour 
{
	#region Attributes

	[SerializeField]
	private Vector3 offsetCamera = Vector3.zero;
	[SerializeField]
	private Transform playerTr = null;
	private Transform cameraTr = null;

    #endregion

    #region UnityFunctions

    void Awake()
    {
		playerTr = GameObject.FindGameObjectWithTag("Player").transform;
		cameraTr = Camera.main.transform;
    }

    void Start()
    {
    }

    void Update()
    {
		StartCoroutine(UpdateCamera());
    }
	
	#endregion
	
	#region Coroutines
	
	IEnumerator UpdateCamera()
	{
		UpdatePosition();
		UpdateRotation();
		yield return new WaitForEndOfFrame();
	}
	
	#endregion
	
	#region Transformations
	
	void UpdatePosition()
	{
		cameraTr.position = playerTr.position + offsetCamera;
	}

	void UpdateRotation()
	{

	}
	
	#endregion
	
	#region Actions
	
	
	
	#endregion
	
	
}