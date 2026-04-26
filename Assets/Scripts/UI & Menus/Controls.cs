using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI tutorialText;
    public GameObject pressFPrompt;
    public GameObject controlsPanel;
    public Button closeButton;

    private bool isShowingControls = false;
    public bool canFinishTutorial = false;

    [Header("Settings")]
    public NPCDialogue tutorialData;

    public Image blackBackground;

    void Awake()
    {
        if (canvasGroup != null) canvasGroup.alpha = 1; 
        if (blackBackground != null) blackBackground.color = Color.black;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.PauseGame();
        }
    }

    void Start()
    {
        pressFPrompt.SetActive(false);
        
        StartCoroutine(RunTutorial());
    }

    IEnumerator RunTutorial()
    {
        float elapsed = 0;
        float fadeDuration = 3f;
        canvasGroup.alpha = 0; 
        
        while(elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed/fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;

        for (int i = 0; i < tutorialData.dialogueLines.Length; i++)
        {
            string line = tutorialData.dialogueLines[i];
            foreach (char letter in line.ToCharArray())
            {
                tutorialText.text += letter; 
                yield return new WaitForSecondsRealtime(tutorialData.typingSpeed);
            }
            if (i < tutorialData.dialogueLines.Length - 1)
            {
                tutorialText.text += "\n\n";
                yield return new WaitForSecondsRealtime(1f);
            }
        }

        pressFPrompt.SetActive(true);
        canFinishTutorial = true;
    }

    void Update()
    {
        if (canFinishTutorial && !isShowingControls && Input.GetKeyDown(KeyCode.F))
        {
            ShowControlsScreen();
        }
    }

    void ShowControlsScreen()
    {
        tutorialText.gameObject.SetActive(false);

        if(controlsPanel != null)
        {
            controlsPanel.SetActive(true);
        }

        isShowingControls = true;
    }

    public void StartGameFromButton()
    {
        if (isShowingControls)
        {
            isShowingControls = false; 
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(FadeOutAndStartGame());
            }
        }
    }

    IEnumerator FadeOutAndStartGame()
    {
        if (controlsPanel != null) 
        {
            controlsPanel.SetActive(false);
        }
        
        if (pressFPrompt != null) 
        {
            pressFPrompt.SetActive(false);
        }

        float elapsed = 0;
        while(elapsed < 1f)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed);
            blackBackground.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsed));
            yield return null;
        }

        SceneLoader.Instance.ResumeGame();
        gameObject.SetActive(false);
        blackBackground.gameObject.SetActive(false);
    }
}
