using UnityEngine;
using System;
using System.Collections;

public class CameraMgr : MonoBehaviour 
{
    #region Attributes

    [SerializeField]
    private bool displayBoundaries = false;
    [SerializeField]
    private float boundariesRatio = 2f;
	[SerializeField]
	private Vector3 offsetCamera = Vector3.zero;
    [SerializeField]
    private Player player = null;

    public Vector3 PlayerPos { get { return player.transform.position + offsetCamera; } }

    public bool IsUpdating { get; set; }


    public Vector2 BoundaryMinPos { get { return new Vector2(Screen.width, Screen.height) / boundariesRatio; } }
    public Vector2 BoundaryMaxPos { get { return BoundaryMinPos + new Vector2(Screen.width - 2f * BoundaryMinPos.x, Screen.height - 2f * BoundaryMinPos.y); } }
    public float RealCamSpeed { get { return boundariesRatio * Time.deltaTime; } }

    #region Instance

    private static CameraMgr instance = null;
    public static CameraMgr Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<CameraMgr>();
            return instance;
        }
    }

    #endregion

    #endregion

    #region UnityFunctions

    void OnGUI()
    {
        if(displayBoundaries)
            GUI.Box(new Rect(BoundaryMinPos.x, BoundaryMinPos.y, BoundaryMaxPos.x - BoundaryMinPos.x, BoundaryMaxPos.y - BoundaryMinPos.y), (Texture)null);
    }

    void Awake()
    {
        player = Player.Instance;
    }

    void Start()
    {
    }

    void Update()
    {
        if (!IsUpdating && !PlayerWithinBoundaries())
            UpdatePosition();
    }

    void FixedUpdate()
    {
        
    }

    #endregion

    #region Coroutines

    IEnumerator UpdateCamera()
    {
        yield return new WaitWhile(() => player.IsMoving);

        while (IsUpdating = (transform.position != PlayerPos))
        {
            Vector3 distance = PlayerPos - transform.position;
            Vector3 nextPosition = distance.magnitude <= 0.1f ? PlayerPos : Vector3.Lerp(transform.position, PlayerPos, RealCamSpeed);
            yield return new WaitForFixedUpdate();
            transform.position = nextPosition;
        }
    }
	
	#endregion
	
	#region Transformations
	
	void UpdatePosition()
	{
        IsUpdating = true;
        StartCoroutine(UpdateCamera());
	}

	void UpdateRotation()
	{

	}

    #endregion

    #region Actions



    #endregion

    #region Functions

    public bool PlayerWithinBoundaries()
    {
        if (player.IsMoving || player.IsRotating)
            return true;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        return screenPos.x.IsWithin(BoundaryMinPos.x, BoundaryMaxPos.x) && screenPos.y.IsWithin(BoundaryMinPos.y, BoundaryMaxPos.y);
    }
    
    #endregion
}