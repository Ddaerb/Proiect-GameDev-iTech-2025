using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EndCutscene : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneFrame
    {
        public Sprite sprite;
        [TextArea(2, 4)] public string[] subtitles;
        public float displayDuration = 3f;
    }

    [Header("References")]
    public Image displayImage;
    public TextMeshProUGUI subtitleText;
    public GameObject endPrompt;

    [Header("Cutscene Settings")]
    public List<CutsceneFrame> frames = new List<CutsceneFrame>();
    public float fadeDuration = 0.5f;
    public float textDisplaySpeed = 0.05f;
    public Color backgroundColor = Color.black;

    private bool isPlaying = false;
    private bool cutsceneFinished = false;

    void Start()
    {
        InitializeBlackBackground();
        HideAllUI();
        StartCutscene();
    }

    void InitializeBlackBackground()
    {
        displayImage.color = backgroundColor;
        if (displayImage.sprite == null)
        {
            displayImage.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
        }
    }

    void HideAllUI()
    {
        displayImage.gameObject.SetActive(false);
        subtitleText.gameObject.SetActive(false);
        if (endPrompt != null) endPrompt.SetActive(false);
    }

    public void StartCutscene()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayCutscene());
        }
    }

    IEnumerator PlayCutscene()
    {
        isPlaying = true;
        displayImage.gameObject.SetActive(true);
        subtitleText.gameObject.SetActive(true);

        foreach (var frame in frames)
        {
            // Set sprite or black background
            if (frame.sprite != null)
            {
                displayImage.sprite = frame.sprite;
                displayImage.color = Color.white;
            }
            else
            {
                displayImage.sprite = null;
                displayImage.color = backgroundColor;
            }

            // Fade in
            yield return FadeImage(1);

            // Display subtitles
            foreach (var line in frame.subtitles)
            {
                yield return DisplayText(line);
                yield return new WaitForSeconds(frame.displayDuration);
            }

            // Don't fade out on the last frame
            if (frames.IndexOf(frame) != frames.Count - 1)
            {
                yield return FadeImage(0);
            }
        }

        // Ensure final frame stays black and visible
        if (displayImage.sprite == null)
        {
            displayImage.color = backgroundColor;
        }
        else
        {
            displayImage.color = Color.white;
        }

        ShowEndPrompt();
    }

    IEnumerator DisplayText(string text)
    {
        subtitleText.text = "";
        foreach (char c in text)
        {
            subtitleText.text += c;
            yield return new WaitForSeconds(textDisplaySpeed);
        }
    }

    IEnumerator FadeImage(float targetAlpha)
    {
        float elapsed = 0;
        Color color = displayImage.color;
        float startAlpha = color.a;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            displayImage.color = color;
            yield return null;
        }
    }

    void ShowEndPrompt()
    {
        cutsceneFinished = true;
        if (endPrompt != null)
        {
            endPrompt.SetActive(true);
        }
    }

    void Update()
    {
        if (cutsceneFinished && Input.anyKeyDown)
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}