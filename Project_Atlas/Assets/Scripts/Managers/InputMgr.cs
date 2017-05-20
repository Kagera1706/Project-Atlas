using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : MonoBehaviour 
{
    #region Attributes

    [SerializeField]
    private Player player = null;

    private bool pause = false;

    #region Instance

    private static InputMgr instance = null;
    public static InputMgr Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<InputMgr>();
            return instance;
        }
    }

    #endregion

    #endregion

    #region UnityFunctions

    void Awake ()
	{
        player = Player.Instance;
	}

	void Start () 
	{
		
	}
	
	void Update () 
	{
        CheckMouseInputs();
        CheckKeyboardInputs();
        CheckPause();
	}
	
	#endregion
	
	#region Coroutines
	
	
	
	#endregion
	
	#region Transformations



	#endregion
	
	#region Actions
	
	public void CheckMouseInputs()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (!Physics.Raycast(ray, out hit))
                return;
            
            GameObject selection = hit.collider.gameObject;
            
            if (selection.layer == player.gameObject.layer)
                player.Select();
            else
            {
                Hex hex = selection.GetComponent<Hex>();
                if (hex && hex != player.CurrentTile)
                    hex.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (!Physics.Raycast(ray, out hit))
                return;

            GameObject selection = hit.collider.gameObject;
            player.MoveToTile(selection);
        }
    }

    public void CheckKeyboardInputs()
    {

    }

    public void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MonoBehaviour[] mbhs = FindObjectsOfType<MonoBehaviour>();
            if (pause)
            {
                foreach (IPausable pausable in mbhs)
                    pausable.Resume();
            }
            else
            {
                foreach (IPausable pausable in mbhs)
                    pausable.Pause();
            }
            pause = !pause;
        }
    }
	
	#endregion
	
	#region Functions
	
	
	
	#endregion
}
