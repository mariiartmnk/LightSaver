using UnityEngine;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    [Header("UI & Game Logic")]
    [SerializeField] Image lightBulb;
    [SerializeField] private GameObject miniGameWindow;
    [SerializeField] private LightBulbInteraction bulbScript;
    private LightBulbInteraction currentBulb;
    [SerializeField] private float fillSpeed = 0.5f;
    [SerializeField] private float safeZone = 0.05f;

    private bool isMoving = false;
    private bool isGameActive = false;
    private float timer = 0f;

    public bool IsGameActive
    {
        get{return isGameActive;}
        set
        {
            isGameActive = value;
            if (isGameActive)
            {
                timer = 1f;
                lightBulb.fillAmount = 1f;
            }
        }
    }

    void Start()
    {
        if(lightBulb != null) lightBulb.fillAmount = 1f;
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
            MoveFill();
        }
    }

    private void MoveFill()
    {
        timer += Time.deltaTime * fillSpeed;
        lightBulb.fillAmount = Mathf.PingPong(timer, 1f);
    }

    private void CheckSuccess()
    {
        
        if (lightBulb.fillAmount <= safeZone)
        {
            Debug.Log("Success!");
            currentBulb.MarkAsFixed();
        }
        else
        {
            Debug.Log("Fail! Wait 30 sec");
            if(PlayerMovement.Instance != null) PlayerMovement.Instance.canMove = true;
            currentBulb.StartCooldown(30f);
            ParticleSystem ps = currentBulb.GetComponentInChildren<ParticleSystem>();
            //if(ps != null) ps.Emit(15);
        }
                    
        isGameActive = false;
        isMoving = false;
        lightBulb.fillAmount = 1f;
        miniGameWindow.SetActive(false);
    }

    public void AssignBulb(LightBulbInteraction bulb)
    {
        currentBulb = bulb;
    }
}