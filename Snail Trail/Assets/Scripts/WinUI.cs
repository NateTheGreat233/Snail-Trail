using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    private MusicManager musicManager;
    private Transition transition;

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        transition = FindObjectOfType<Transition>();
    }

    public void MenuButtonAction()
    {
        transition.ChangeScene("Menu");
        musicManager.ChangeToMenuMusic();
    }

    public void LevelSelectButtonAction()
    {
        transition.ChangeScene("Level Select");
        musicManager.ChangeToMenuMusic();
    }
}
