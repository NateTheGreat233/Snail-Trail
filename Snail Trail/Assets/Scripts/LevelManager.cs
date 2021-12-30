using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager levelManager;
    private int levelNum;

    private void Awake()
    {
        if (levelManager == null)
        {
            levelManager = this;
            levelNum = 1;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseLevel()
    {
        levelNum++;
    }

    public int GetLevel()
    {
        return levelNum;
    }
}
