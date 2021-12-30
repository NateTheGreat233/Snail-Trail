using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    public Button buttonPrefab;
    public Canvas canvas;
    public int xPos;
    public int yPos;
    public int spacingX;
    public int spacingY;

    private LevelManager levelManager;
    private Transition transition;
    private MusicManager musicManager;

    private bool wiggling;

    private void Awake()
    {
        wiggling = false;
    }

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        transition = FindObjectOfType<Transition>();
        musicManager = FindObjectOfType<MusicManager>();
        CreateLevelButtons();
    }

    public void MenuButtonAction()
    {
        transition.ChangeScene("Menu");
    }

    private void CreateLevelButtons()
    {
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                Button b = Instantiate(buttonPrefab, canvas.transform, true).GetComponent<Button>();
                b.transform.position = new Vector2(xPos + x*spacingX, yPos - y*spacingY);
                int level = y * 4 + x + 1;
                b.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + level;
                b.GetComponentInChildren<TextMeshProUGUI>().name = "Level " + level + " text";
                b.name = "Level " + level;

                b.onClick.AddListener(delegate { EnterLevel(b.gameObject, level); });

                if (level <= levelManager.GetLevel())
                {
                    Image[] ims = b.GetComponentsInChildren<Image>();
                    foreach (Image im in ims)
                    {
                        if (im.gameObject.name == "Locked Image")
                        {
                            im.gameObject.SetActive(false);
                        }
                    }
                }

                b.GetComponent<Image>().color = level <= levelManager.GetLevel() ? new Color(122 / 255f, 255 / 255f, 93 / 255f, 255 / 255f) : new Color(61 / 255f, 125 / 255f, 47 / 255f, 255 / 255f);
            }
        }
    }

    private void EnterLevel(GameObject b, int level)
    {
        if (levelManager.GetLevel() >= level)
        {
            transition.ChangeScene("Level " + level);
            musicManager.ChangeToLevelMusic();
        }
        else
        {
            if (wiggling) return;

            musicManager.PlaySFX("locked level");
            Image[] ims = b.GetComponentsInChildren<Image>();
            foreach (Image im in ims)
            {
                if (im.gameObject.name == "Locked Image")
                {
                    StartCoroutine(Wiggle(im.gameObject));
                }
            }
        }
    }

    private IEnumerator Wiggle(GameObject go)
    {
        wiggling = true;

        while (go.transform.rotation.z < 0.15)
        {
            go.transform.eulerAngles += Vector3.forward * 1;
            yield return new WaitForSeconds(0.005f);
        }
        go.transform.eulerAngles += Vector3.back * 1;
        while (go.transform.rotation.z < 0.15)
        {
            go.transform.eulerAngles += Vector3.back * 1;
            yield return new WaitForSeconds(0.005f);
        }
        while (go.transform.rotation.z > 0.005)
        {
            go.transform.eulerAngles += Vector3.forward * 1;
            yield return new WaitForSeconds(0.005f);
        }

        wiggling = false;
    }
}
