using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slimez : MonoBehaviour
{
    public List<Sprite> possibleSprites;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = possibleSprites[Random.Range(0, possibleSprites.Count - 1)];
    }
}
