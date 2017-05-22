using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : MonoBehaviour 
{
    #region Attributes

    [SerializeField]
    private Player player = null;

    [SerializeField]
    private ISelectable currentSelected = null;
    public ISelectable CurrentSelected
    {
        get { return currentSelected; }
        set
        {
            if (value == null || currentSelected == value)
                return;

            if (currentSelected != null)
                currentSelected.Select();
            currentSelected = value;
            currentSelected.Select();
        }
    }

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
            GetPointedObject();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ISelectable selectable = GetPointedObject();

            if (selectable == null)
                return;

            if (selectable is Hex)
                player.MoveToTile(selectable as Hex);
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
            Time.timeScale = (IsPaused = !IsPaused) ? 0f : 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region Functions

    public ISelectable GetPointedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
            return null;

        ISelectable selectable = hit.collider.GetComponent<ISelectable>();

        return CurrentSelected = selectable;
    }

    #endregion
}
