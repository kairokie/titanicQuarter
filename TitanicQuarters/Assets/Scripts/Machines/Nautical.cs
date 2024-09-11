using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nautical : Machine
{

    [SerializeField]
    public List<Word> Words = new List<Word>();
    string _currentMorseWord;

    public string CurrentMorseWord { get => _currentMorseWord; set => _currentMorseWord = value; }

    string _currentLatinWord;

    public string CurrentLatinWord { get => _currentLatinWord; set => _currentLatinWord = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
