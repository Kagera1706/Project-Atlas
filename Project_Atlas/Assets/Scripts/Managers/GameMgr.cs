using System;
using UnityEngine;

public class GameMgr : MonoBehaviour 
{
    #region Attributes

    [SerializeField]
    private GameObject player = null;
    public GameObject Player { get { return player; } set { player = value; } }

    #region Instance

    private static GameMgr instance = null;
    public static GameMgr Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<GameMgr>();
            return instance;
        }
    }

    #endregion

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
