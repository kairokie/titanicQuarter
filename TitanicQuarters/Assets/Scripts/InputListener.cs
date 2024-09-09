using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InputListener : MonoBehaviour
{



    public TextMeshProUGUI _textDisplay;
    public TextMeshProUGUI _questionTextDisplay;
    public TextMeshProUGUI _feedbackTextDisplay;
    public Telegraph _telegraph;
    // Start is called before the first frame update
    void Start()
    {
        if (_telegraph != null)
        {
            _telegraph.OnCorrectWord += Correct;
            _telegraph.OnCorrectLetter += Correct;
            _telegraph.OnIncorrectWord += Error;
            _telegraph.OnIncorrectLetter += Error;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MouseInputDetection();
        if (_textDisplay != null && _telegraph)
        {
            _textDisplay.text = _telegraph.GetCurrentText();
        }
        if (_questionTextDisplay != null && _telegraph)
        {
            _questionTextDisplay.text = _telegraph.GetCurrentMorseWord();
        }
    }

    void MouseInputDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // "Dot " in morse code
            _telegraph.ReadChar('•');
        }
        if (Input.GetMouseButtonDown(1))
        {
            // "Dash" in morse code
            _telegraph.ReadChar('-');
        }

        // if enter key is pressed validate the word
        if (Input.GetKeyUp(KeyCode.Return))
        {
            _telegraph.SendWord();
        }
    }

    void Error()
    {
        _textDisplay.color = Color.red;
    }

    void Correct()
    {
        _textDisplay.color = Color.black;
    }

}






