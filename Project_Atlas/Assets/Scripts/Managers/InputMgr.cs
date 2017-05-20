using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : MonoBehaviour 
{
    #region Attributes

    [SerializeField]
    private Player player = null;

    private bool pause;
    public bool IsPaused
    {
        get { return pause; }
        set { GUIMgr.Instance.DisplayPause(pause = value); }
    }
    
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
        if (IsPaused)
            return;

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
            Hex hex = selection.GetComponent<Hex>();
            if (!hex)
                return;

            player.MoveToTile(hex);
        }
    }

    public void CheckKeyboardInputs()
    {
        if (IsPaused)
            return;
    }

    public void CheckPause(bool usedButton = false)
    {
        if (Input.GetKeyDown(KeyCode.Escape) || usedButton)
        {
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0f : 1f;
        }
    }
	
	#endregion
	
	#region Functions
	
	
	
	#endregion
}
