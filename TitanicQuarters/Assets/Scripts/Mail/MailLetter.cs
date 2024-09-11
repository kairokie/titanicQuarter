using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailLetter : MonoBehaviour
{
    Word _word;
    Vector3 Vector3 = new Vector3(162, -0, -529);

    public Word Word
    {
        get { return _word; }
        set { _word = value; }
    }

    Machine _machine;

    public Machine Machine
    {
        get { return _machine; }
        set { _machine = value; }
    }

    // Test editor only
    [SerializeField]
    private string _wordTest;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_word != null)
        {
            _wordTest = _word.GetWord(Alphabets.LATIN);
        }
    }
}
