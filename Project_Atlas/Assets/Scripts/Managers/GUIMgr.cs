using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[Serializable]
public class GUIWindows
{
    public GameObject pauseMenu = null;
}

public class GUIMgr : MonoBehaviour 
{
    #region Attributes
    
    [SerializeField]
    private Player player = null;

    [SerializeField]
    private GUIWindows menus;

    public GameObject PauseMenu { get { return menus.pauseMenu; } set { menus.pauseMenu = value; } }

    #region Instance

    private static GUIMgr instance = null;
    public static GUIMgr Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<GUIMgr>();
            return instance;
        }
    }

    #endregion

    #endregion

    #region UnityFunctions

    void Awake ()
	{
        player = Player.Instance;
        PauseMenu = transform.FindChild("PauseMenu").gameObject;
    }

	void Start () 
	{
        DisplayPause(false);
    }
	
	void Update () 
	{
		
	}

    #endregion

    #region Coroutines



    #endregion

    #region Actions



    #endregion

    #region Functions

    public void DisplayPause(bool state)
    {
        PauseMenu.SetActive(state);
    }

    #endregion
}
