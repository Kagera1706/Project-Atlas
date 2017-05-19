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
    [SerializeField]
    private GameObject playerStats = null;
    [SerializeField]
    private GameObject playerSkillBar = null;

    [SerializeField]
    private Slider hpBar = null;
    [SerializeField]
    private Slider manaBar = null;

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
        InitGUI();
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

    void InitGUI()
    {
        //foreach (GameObject go in GameObject.FindGameObjectsWithTag("Info"))
        //{
        //    switch (go.name)
        //    {
        //        case "PlayerStats":
        //            playerStats = go;
        //            break;
        //        case "PlayerSkillBar":
        //            playerSkillBar = go;
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }

    void Init()
    {
        player.OnHealthChanged += (val) => { hpBar.value = val; Debug.Log("Did 1"); };
        player.OnManaChanged += (val) => { manaBar.value = val; Debug.Log("Did 2"); };
        Debug.Log("Its all fine");
    }

    #endregion
}
