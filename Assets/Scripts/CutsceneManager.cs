using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SpriteCutsceneManager : MonoBehaviour
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
    public GameObject skipPrompt;

    [Header("Cutscene Settings")]
    public List<CutsceneFrame> frames = new List<CutsceneFrame>();
    public float fadeDuration = 0.5f;
    public float textDisplaySpeed = 0.05f;

    [Header("Skip Settings")]
    public float skipPromptDelay = 2f;
    public KeyCode skipKey = KeyCode.X;

    private bool isPlaying = false;
    private bool isSkipping = false;

    void Start()
    {
        HideAllUI();
        StartCutscene();
    }

    void HideAllUI()
    {
        displayImage.gameObject.SetActive(false);
        subtitleText.gameObject.SetActive(false);
        if (skipPrompt != null) skipPrompt.SetActive(false);
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

        if(SoundManager.Instance.musicSource != null && SoundManager.Instance.introMusic != null)
        {
            SoundManager.Instance.musicSource.clip = SoundManager.Instance.introMusic;
            SoundManager.Instance.musicSource.loop = true;
            SoundManager.Instance.musicSource.Play();
        }

        // Show skip prompt after delay
        if (skipPrompt != null)
        {
            yield return new WaitForSeconds(skipPromptDelay);
            if (!isSkipping) skipPrompt.SetActive(true);
        }

        foreach (var frame in frames)
        {
            if (isSkipping) break;

            // Set and fade in sprite
            displayImage.sprite = frame.sprite;
            displayImage.color = new Color(1, 1, 1, 0);
            yield return FadeImage(1);  // Fade in

            // Display each subtitle line
            foreach (var line in frame.subtitles)
            {
                if (isSkipping) break;
                yield return DisplayText(line);
                yield return WaitOrSkip(frame.displayDuration);
            }

            if (!isSkipping) yield return FadeImage(0);  // Fade out
        }

        // Cutscene ended naturally - load Level1
        LoadLevel1();
    }

    IEnumerator DisplayText(string text)
    {
        subtitleText.text = "";
        foreach (char c in text)
        {
            if (isSkipping)
            {
                subtitleText.text = text;
                break;
            }
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
            if (isSkipping)
            {
                color.a = targetAlpha;
                displayImage.color = color;
                break;
            }

            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            displayImage.color = color;
            yield return null;
        }
    }

    IEnumerator WaitOrSkip(float duration)
    {
        float timer = 0;
        while (timer < duration && !isSkipping)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }

    void Update()
    {
        if (isPlaying && !isSkipping && Input.GetKeyDown(skipKey))
        {
            SkipCutscene();
        }
    }

    void SkipCutscene()
    {
        if (isPlaying && !isSkipping)
        {
            isSkipping = true;
            if (skipPrompt != null) skipPrompt.SetActive(false);
            LoadLevel1();
        }
    }

    void LoadLevel1()
    {
        EndCutscene();
        SceneManager.LoadScene("CharacterCreator");
    }

    void EndCutscene()
    {
        HideAllUI();
        isPlaying = false;
        isSkipping = false;

        // Stop the intro sound
        if (SoundManager.Instance.musicSource != null && SoundManager.Instance.musicSource)
        {
            SoundManager.Instance.musicSource.Stop();
        }
    }
}