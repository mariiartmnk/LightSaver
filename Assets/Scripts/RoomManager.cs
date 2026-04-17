using System;
using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Settings")]
    private SpriteRenderer[] childSprites;
    public float fadeSpeed = 3.0f;

    private Coroutine fadeRoutine;

    void Start()
    {
        childSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Fade(0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Fade(1f);
        }
    }

    private void Fade(float targetAlpha)
    {
        if(fadeRoutine != null) StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    IEnumerator FadeRoutine(float targetAlpha)
    {
        float currentAlpha = childSprites[0].color.a;
        while(!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            foreach (SpriteRenderer sr in childSprites)
            {
                Color c = sr.color;
                c.a = currentAlpha;
                sr.color = c;
            }
            yield return null;
        }
    }
}
