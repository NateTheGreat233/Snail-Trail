using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    public void MenuButtonAction()
    {
        SceneManager.LoadScene("Menu");
    }
}
