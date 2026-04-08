using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System;

public class LightBulbInteraction : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject interactTextUI;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private GameObject miniGameWindow;

    [Header("Settings")]
    [SerializeField] private PointerController pointerScript;

    [Header("Lighting Animation")]
    [SerializeField] private Light2D bulbLight;
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem sparkParticles;
    [SerializeField] private AudioSource localSparkSource;

    [Header("Navigation")]
    [SerializeField] public Transform navPoint;

    private bool canInteract = false;
    private float cooldownTimer = 0f;
    private bool isFixed = false;
    public bool IsFixed => isFixed;
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

            if(sparkParticles != null && !sparkParticles.isPlaying)
            {
                var main = sparkParticles.main;
                main.loop = true;
                if(!localSparkSource.isPlaying) localSparkSource.Play();
                sparkParticles.Play();
            }

            if(cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);
                cooldownText.text = "Repairing: " + Mathf.Ceil(cooldownTimer) + "s";
            }
            interactTextUI.SetActive(false);
        }
        else
        {
            if(sparkParticles != null && sparkParticles.isPlaying)
            {
                sparkParticles.Stop();
                if(localSparkSource.isPlaying) localSparkSource.Stop();
            }

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
            pointerScript.AssignBulb(this);

            miniGameWindow.SetActive(true);
            pointerScript.IsGameActive = true;

            if(PlayerMovement.Instance != null) PlayerMovement.Instance.canMove = false;
        }
    }

    public void MarkAsFixed()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.switchSFX);
        isFixed = true;
        interactTextUI.SetActive(false);

        if(PlayerMovement.Instance != null) PlayerMovement.Instance.canMove = true;

        if(bulbLight != null)
        {
            StartCoroutine(AnimateLightOff());
        }

        if(BrightnessManager.Instance != null)
        {
            BrightnessManager.Instance.RegisterBulbFixed();
        }
    }

    private IEnumerator AnimateLightOff()
    {
        float startIntensity = bulbLight.intensity;
        float elapsed = 0;

        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            bulbLight.intensity = Mathf.Lerp(startIntensity, 0, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        bulbLight.intensity = 0;
        bulbLight.enabled = false;
    }
}
