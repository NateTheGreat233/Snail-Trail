using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public GameObject topPanel;
    public Button musicButton;
    public Button sfxButton;

    private bool music;
    private bool sfx;

    private MusicManager musicManager;
    private Transition transition;

    public void PlayButtonAction()
    {
        transition.ChangeScene("Level Select");
    }
    public void InstructionsButtonAction()
    {
        transition.ChangeScene("Instructions");
    }

    private void Awake()
    {
        StartCoroutine(TopMove());
    }

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        transition = FindObjectOfType<Transition>();
        music = true;
        sfx = true;
        if (music != musicManager.GetMusic()) ToggleMusic();
        if (sfx != musicManager.GetSFX()) ToggleSFX();
    }

    private IEnumerator TopMove()
    {
        float y = Mathf.Max(topPanel.transform.position.y, 550);
        float amplitude = 20f;
        bool down = false;
        float currentTime = Time.realtimeSinceStartup;
        while (true)
        {
            float now = Time.realtimeSinceStartup;
            float deltaTime = now - currentTime;

            if (down)
            {
                topPanel.transform.position += Vector3.down * 0.1f * deltaTime * 100;
                if (topPanel.transform.position.y < (y - amplitude))
                {
                    down = false;
                }
            }
            else
            {
                topPanel.transform.position += Vector3.up * 0.1f * deltaTime * 100;
                if (topPanel.transform.position.y > (y + amplitude))
                {
                    down = true;
                }
            }

            yield return new WaitForSeconds(0.01f);
            currentTime = now;
        }
    }

    public void ToggleMusic()
    {
        music = !music;
        musicManager.PlayMusic(music);
        musicButton.GetComponent<Image>().color = music ? new Color(122 / 255f, 255 / 255f, 93 / 255f, 255 / 255f) : new Color(61/255f, 125/255f, 47/255f, 255/255f);
    }

    public void ToggleSFX()
    {
        sfx = !sfx;
        musicManager.PlaySFX(sfx);
        sfxButton.GetComponent<Image>().color = sfx ? new Color(122 / 255f, 255 / 255f, 93 / 255f, 255 / 255f) : new Color(61 / 255f, 125 / 255f, 47 / 255f, 255 / 255f);
    }
}
