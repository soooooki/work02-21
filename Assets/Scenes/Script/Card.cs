using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Sprite[] CardSprites;
    private string fileName = "2048";
    public int _currentIndex = 0;
    // Start is called before the first frame update
    void Awake()
    {
        CardSprites = Resources.LoadAll<Sprite>(fileName);
    }

    // Update is called once per frame

    public void Generate(int index)
    {
        _currentIndex = index;
        GetComponent<SpriteRenderer>().sprite = CardSprites[_currentIndex];
    }


    public void Merge()
    {
        _currentIndex++;
        GetComponent<SpriteRenderer>().sprite = CardSprites[_currentIndex];
    }


}
