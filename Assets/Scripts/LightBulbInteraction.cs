using TMPro;
using UnityEngine;

public class LightBulbInteraction : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject interactTextUI;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private GameObject miniGameWindow;

    [Header("Settings")]
    [SerializeField] private PointerController pointerScript;

    private bool canInteract = false;
    private float cooldownTimer = 0f;
    private bool isFixed = false;
    void Start()
    {
        interactTextUI.SetActive(false);
        miniGameWindow.SetActive(false);
        if(cooldownText != null) cooldownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if(isFixed) return;

        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;

            if(cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);
                cooldownText.text = "Repairing: " + Mathf.Ceil(cooldownTimer) + "s";
            }
            interactTextUI.SetActive(false);
        }
        else
        {
            cooldownTimer = 0;
            if(cooldownText != null && cooldownText.gameObject.activeSelf)
            {
                cooldownText.gameObject.SetActive(false);

                if(canInteract) interactTextUI.SetActive(true);
            }
        }
    }

    public void StartCooldown(float seconds)
    {
        cooldownTimer = seconds;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFixed && cooldownTimer <= 0)
        {
            canInteract = true;
            interactTextUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            interactTextUI.SetActive(false);
            canInteract = false;
        }
    }

    public void OpenMiniGame()
    {
        if(cooldownTimer <= 0 && !isFixed)
        {
            interactTextUI.SetActive(false);
            miniGameWindow.SetActive(true);
            pointerScript.IsGameActive = true;
        }
    }

    public void MarkAsFixed()
    {
        isFixed = true;
        interactTextUI.SetActive(false);

        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
