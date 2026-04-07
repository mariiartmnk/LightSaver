using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance;

    [Header("Global Light Settings")]
    [SerializeField] private Light2D globalLight;
    [SerializeField] private float startIntensity = 0.7f;
    [SerializeField] private float darkIntensity = 0.1f;
    
    [Header("Game Over Settings")]
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private float fadeDuration = 2.5f;

    private int totalBulbs;
    private int fixedBulbs = 0;
    private bool isGameOver = false;
    
    void Awake()
    {
        Instance = this;    
    }

    void Start()
    {
        totalBulbs = GameObject.FindObjectsByType<LightBulbInteraction>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length;

        if(gameOverCanvasGroup != null) 
        {
            gameOverCanvasGroup.alpha = 0;
            gameOverCanvasGroup.interactable = false;
            gameOverCanvasGroup.blocksRaycasts = false;
        }

        if(globalLight != null) globalLight.intensity = startIntensity;
    }

    public void RegisterBulbFixed()
    {
        if(isGameOver) return;

        fixedBulbs++;
        UpdateGlobalLight();

        if(fixedBulbs >= totalBulbs && totalBulbs > 0)
        {
            StartCoroutine(SmoothFade());
        }
    }

    private void UpdateGlobalLight()
    {
        if(totalBulbs == 0 || globalLight == null) return;

        float targetAlpha = (float)fixedBulbs / (float)totalBulbs;

        if(!isGameOver)
        {
            globalLight.intensity = Mathf.Lerp(startIntensity, darkIntensity, targetAlpha);
        }
    }

    private IEnumerator SmoothFade()
    {
        isGameOver = true;
        if(PlayerMovement.Instance != null) PlayerMovement.Instance.canMove = false;
        
        float elapsed = 0;
        float currentIntencity = globalLight != null ? globalLight.intensity : 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            if(globalLight != null)
            {
                globalLight.intensity = Mathf.Lerp(currentIntencity, 0f, smoothT);
            }

            if(gameOverCanvasGroup != null)
            {
                gameOverCanvasGroup.alpha = smoothT;
            }
            
            yield return null;
        }

        if(gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.interactable = true;
            gameOverCanvasGroup.blocksRaycasts = true;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
