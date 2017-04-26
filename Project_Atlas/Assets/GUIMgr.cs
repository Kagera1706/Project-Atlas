using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIMgr : Manager 
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

	#endregion

	#region UnityFunctions

	void Awake ()
	{
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Info"))
        {
            switch (go.name)
            {
                case "PlayerStats":
                    playerStats = go;
                    break;
                case "PlayerSkillBar":
                    playerSkillBar = go;
                    break;
                default:
                    break;
            }
        }
    }

    void Init()
    {
        Slider[] bars = playerStats.GetComponentsInChildren<Slider>();
        if (bars.Length > 0)
        {
            hpBar = bars[0];
            manaBar = bars[1];

            player.OnHealthChanged += (val) => { hpBar.value = val; Debug.Log("Did 1"); };
            player.OnManaChanged += (val) => { manaBar.value = val; Debug.Log("Did 2"); };
            Debug.Log("Its all fine");
        }
    }

    #endregion
}
