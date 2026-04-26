using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnimateButton : MonoBehaviour
{
    public UnityEvent actionAfterAnimation;
    private Button btn;
    void Awake()
    {
        // Automatically find the button and tell it to run "Click" when pressed
        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(Click);
        }
    }
    public void Click()
    {
        transform.DOKill(); // Stop any existing animations
        transform.localScale = Vector3.one; // Reset scale

        if (MenuAudioManager.Instance != null)
            MenuAudioManager.Instance.PlaySFX(MenuAudioManager.Instance.buttonsSFX);
        else if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.mainMenuSFX);

        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f)
            .SetUpdate(true)
            .OnComplete(() => {
                actionAfterAnimation?.Invoke();
            });
    }
}
