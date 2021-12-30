using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private static Transition t;
    private bool inTransition;

    public Image screen;

    private void Awake()
    {
        //Screen.SetResolution(960, 540, false);

        if (t == null)
        {
            t = this;
            inTransition = false;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(string name)
    {
        if (!inTransition)
        {
            StartCoroutine(SceneTransition(name));
        }
    }

    private IEnumerator SceneTransition(string name)
    {
        inTransition = true;
        screen.gameObject.SetActive(true);
        float currentTime = Time.realtimeSinceStartup;
        while(screen.color.a < 1)
        {
            float now = Time.realtimeSinceStartup;
            float deltaTime = currentTime - now;
            screen.color = new Color(0, 0, 0, screen.color.a - 0.04f*deltaTime*100);
            yield return null;
            currentTime = now;
        }
        SceneManager.LoadScene(name);
        currentTime = Time.realtimeSinceStartup;
        while (screen.color.a > 0)
        {
            float now = Time.realtimeSinceStartup;
            float deltaTime = currentTime - now;
            screen.color = new Color(0, 0, 0, screen.color.a + 0.04f * deltaTime * 100);
            yield return null;
            currentTime = now;
        }
        screen.gameObject.SetActive(false);
        inTransition = false;
    }

}
