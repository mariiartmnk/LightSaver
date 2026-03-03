using UnityEngine;

public class PointerController : MonoBehaviour
{
    [Header("Pathing")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("UI & Game Logic")]
    [SerializeField] private RectTransform safeZone;
    [SerializeField] private GameObject miniGameWindow;
    [SerializeField] private LightBulbInteraction bulbScript;
    [SerializeField] private float moveSpeed = 200f; 

    private RectTransform pointerTransform;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isGameActive = false;

    public bool IsGameActive
    {
        get{return isGameActive;}
        set{isGameActive = value;}
    }

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        pointerTransform.position = pointA.position;
        targetPosition = pointB.position;
    }

    void Update()
    {
        if(!isGameActive) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = true;
        }

        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isMoving = false;
            CheckSuccess();
        }

        if (isMoving)
        {
            MovePointer();
        }
    }

    private void MovePointer()
    {
        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointerTransform.position, targetPosition) < 0.1f)
        {
            targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }

    private void CheckSuccess()
    {
        
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            Debug.Log("Success!");
            bulbScript.MarkAsFixed();
        }
        else
        {
            Debug.Log("Fail! Wait 30 sec");
            bulbScript.StartCooldown(30f);
        }

        isGameActive = false;
        isMoving = false;

        pointerTransform.position = pointA.position;

        miniGameWindow.SetActive(false);

        
    }
}