using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Manager 
{
    #region Attributes

    [SerializeField]
    private GameObject player = null;
	
	#endregion

	#region UnityFunctions

	void Awake ()
	{
        player = GameObject.FindGameObjectWithTag("Player");
	}

	void Start () 
	{
        Init();
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

    void Init()
    {

    }

    #endregion
}
