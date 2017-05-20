using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIMgr : MonoBehaviour 
{
    #region Attributes

    [SerializeField]
    private Player player = null;

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

    }

	void Start () 
	{

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



    #endregion
}
