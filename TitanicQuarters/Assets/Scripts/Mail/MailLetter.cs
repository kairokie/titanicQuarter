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

    [SerializeField]
    private string _machineName;

    public MailLetter(string word)
    {
        _word = new Word(word);
    }

    private void Awake()
    {
        int machineIndex = 0;
        machineIndex = Random.Range(0, 2);
        if (machineIndex == 0 || true)
        {
            _machine = FindObjectOfType<Telegraph>(true);
            _machineName = "Telegraph";
        }
        else
        {
            _machine = FindObjectOfType<Military>(true);
            _machineName = "Military";
        }
    }

    public void PickLetter()
    {
        Collider [] colliders = gameObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    public void DropLetter()
    {
        Collider[] colliders = gameObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }
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
