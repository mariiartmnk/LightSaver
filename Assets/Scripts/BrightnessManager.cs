using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance;

    [Header("Darkness Settings")]
    [SerializeField] private Image darknessOverlay;
    [UnityEngine.Range(0f, 1f)] [SerializeField] private float maxDarkness = 1.0f;

    [Header("Game Over Settings")]
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private float fadeDuration = 5.0f;
    

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
        UpdateBrightness();
    }

    public void RegisterBulbFixed()
    {
        if(isGameOver) return;

        fixedBulbs++;
        UpdateBrightness();

        if(fixedBulbs >= totalBulbs && totalBulbs > 0)
        {
            StartCoroutine(UltimateSmoothFade());
        }
    }

    private void UpdateBrightness()
    {
        if(darknessOverlay == null || totalBulbs == 0) return;

        float targetAlpha = ((float)fixedBulbs / totalBulbs) * maxDarkness;

        if(!isGameOver)
        {
            Color newColor = darknessOverlay.color;
            newColor.a = targetAlpha;
            darknessOverlay.color = newColor;
        }
    }

    private IEnumerator UltimateSmoothFade()
    {
        isGameOver = true;
        if(PlayerMovement.Instance != null) PlayerMovement.Instance.canMove = false;
        
        float elapsed = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float startAlpha = darknessOverlay.color.a;

        while(elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            float cinematicT = smoothT * smoothT * (3f - 2f * smoothT); 

            if(darknessOverlay != null)
            {
                Color c = darknessOverlay.color;
                c.a = Mathf.Lerp(startAlpha, 1f, cinematicT);
                darknessOverlay.color = c;
            }

            if(gameOverCanvasGroup != null)
            {
                gameOverCanvasGroup.alpha = cinematicT;
            }
            
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;
        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.blocksRaycasts = true;
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
