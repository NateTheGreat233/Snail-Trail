using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public Canvas canvas;
    public string levelName;
    private TextMeshProUGUI levelText;
    private Button musicButton;
    private Button sfxButton;
    private bool music;
    private bool sfx;

    private MusicManager musicManager;
    private Transition transition;

    private void Start()
    {
        RectTransform[] gos = canvas.GetComponentsInChildren<RectTransform>();
        TextMeshProUGUI[] texts = canvas.GetComponentsInChildren<TextMeshProUGUI>();
        Button[] buttons = canvas.GetComponentsInChildren<Button>();

        foreach (TextMeshProUGUI t in texts)
        {
            if (t.name == "Level Title")
            {
                levelText = t;
                levelText.text = levelName;
                break;
            }
        }

        foreach (Button b in buttons)
        {
            if (b.name == "Music Button")
            {
                musicButton = b;
            }
            else if (b.name == "SFX Button")
            {
                sfxButton = b;
            }
        }

        musicManager = FindObjectOfType<MusicManager>();
        transition = FindObjectOfType<Transition>();
        music = true;
        sfx = true;
        if (music != musicManager.GetMusic()) ToggleMusic();
        if (sfx != musicManager.GetSFX()) ToggleSFX();

        StartCoroutine(Fade());
    }

    public void MenuButtonAction()
    {
        transition.ChangeScene("Menu");
        musicManager.ChangeToMenuMusic();
    }

    public void RestartButtonAction()
    {
        transition.ChangeScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleMusic()
    {
        music = !music;
        musicManager.PlayMusic(music);
        musicButton.GetComponent<Image>().color = music ? Color.white : Color.gray;
    }

    public void ToggleSFX()
    {
        sfx = !sfx;
        musicManager.PlaySFX(sfx);
        sfxButton.GetComponent<Image>().color = sfx ? Color.white : Color.gray;
    }

    IEnumerator Fade()
    {
        levelText.alpha = 0;

        float currentTime = Time.realtimeSinceStartup;
        while (levelText.alpha < 1)
        {
            float now = Time.realtimeSinceStartup;
            float deltaTime = now - currentTime;
            levelText.alpha += 0.01f*deltaTime*100;
            currentTime = now;
            yield return null;
        }

        currentTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - currentTime < 2)
        {
            yield return null;
        }

        currentTime = Time.realtimeSinceStartup;
        while (levelText.alpha > 0)
        {
            float now = Time.realtimeSinceStartup;
            float deltaTime = now - currentTime;
            levelText.alpha -= 0.01f * deltaTime * 100;
            currentTime = now;
            yield return null;
        }
    }
}
