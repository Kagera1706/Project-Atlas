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

    private bool updateCamera = false;

    #endregion

    #region UnityFunctions

    void Update()
    {
        if (updateCamera)
            return;

        if (playerTr.position.x < Screen.width * 0.25f ||
            playerTr.position.x > Screen.width * 0.75f ||
            playerTr.position.z < Screen.height * 0.25f ||
            playerTr.position.z > Screen.height * 0.75f)
        {
            UpdatePosition();
        }
    }

    #endregion

    #region Coroutines

    IEnumerator UpdateCamera()
    {
        while (updateCamera = (transform.position != playerTr.position + offsetCamera))
        {

            Vector3 nextPosition = ? playerTr.position + offsetCamera : Vector3.Lerp(transform.position, playerTr.position + offsetCamera, 2f * Time.deltaTime);
            yield return new WaitForFixedUpdate();
            transform.position = nextPosition;
        }
    }
	
	#endregion
	
	#region Transformations
	
	void UpdatePosition()
	{
        Debug.Log("Moving camera");
        StartCoroutine(UpdateCamera());
	}

	void UpdateRotation()
	{

	}
	
	#endregion
	
	#region Actions
	
	
	
	#endregion
	
	
}