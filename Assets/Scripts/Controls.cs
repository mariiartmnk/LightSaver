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

    [Header("Settings")]
    public NPCDialogue tutorialData;

    public Image blackBackground;

    private bool canFinish = false;

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
        canFinish = true;
    }

    void Update()
    {
        if(canFinish && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FadeOutAndStartGame());
        }
    }

    IEnumerator FadeOutAndStartGame()
    {
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
