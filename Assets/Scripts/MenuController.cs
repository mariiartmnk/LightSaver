using UnityEngine;
using System.Collections;

public class MenuController: MonoBehaviour
{
    public CanvasGroup mainMenu;
    public CanvasGroup optionsMenu;
    public CanvasGroup controlsMenu;
    public float transitionDuration = .25f;
    

    void Start()
    {
        Time.timeScale = 1f;
        ShowMainMenuInstant();
    }

    public void OpenOptionsMenu()
    {
        StartCoroutine(TransitionMenus(mainMenu, optionsMenu));
    }

    public void BackToMainMenu()
    {
        StartCoroutine(TransitionMenus(optionsMenu, mainMenu));
    }

    public void OpenControls()
    {
        StartCoroutine(TransitionMenus(mainMenu, controlsMenu));
    }

    public void CloseControls()
    {
        StartCoroutine(TransitionMenus(controlsMenu, mainMenu));
    }

    void ShowMainMenuInstant()
    {
        mainMenu.alpha = 1;
        mainMenu.interactable = true;
        mainMenu.blocksRaycasts = true;

        optionsMenu.alpha = 0;
        optionsMenu.interactable = false;
        optionsMenu.blocksRaycasts = false;
        optionsMenu.gameObject.SetActive(false);

        controlsMenu.alpha = 0;
        controlsMenu.interactable = false;
        controlsMenu.blocksRaycasts = false;
        controlsMenu.gameObject.SetActive(false);
    }

    IEnumerator TransitionMenus(CanvasGroup currentMenu, CanvasGroup nextMenu)
    {
        float timer = 0f;
        nextMenu.gameObject.SetActive(true);

        while(timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float progress = timer/transitionDuration;
            currentMenu.alpha = 1 - progress;
            nextMenu.alpha = progress;
            yield return null;
        }
        currentMenu.interactable = false;
        currentMenu.blocksRaycasts = false;
        currentMenu.gameObject.SetActive(false);

        nextMenu.interactable = true;
        nextMenu.blocksRaycasts = true;
    }
}