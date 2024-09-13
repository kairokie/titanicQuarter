using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //[SerializeField]
    //public List<int> _scorePerWordSize;

    [SerializeField]
    private int _score = 0;

    private int _highscore;

    [SerializeField]
    private TextMeshProUGUI _scoreGameDisplay;


    [SerializeField]
    private TextMeshProUGUI _highscoreMenuDisplay;

    [SerializeField]
    private TextMeshProUGUI _highscoreEndScreenDisplay;
    [SerializeField]
    private TextMeshProUGUI _scoreEndScreenDisplay;

    public void Start()
    {
        _highscore = PlayerPrefs.GetInt("highscore", _highscore);

        SetMenuScoreDisplay();
    }

    public void IncrementScore()
    {
        _score++;
        _scoreGameDisplay.text = "Score : " + _score.ToString();
        if (_score > _highscore)
        {
            _highscore = _score;
            PlayerPrefs.SetInt("highscore", _highscore);
        }
    }

    /*
    public void IncrementScoreWithWordSize(int size)
    {
        if (size <= 1) return;

        if (size <= _scorePerWordSize.Count)
        {
            _score += _scorePerWordSize[size-2];
        }
        else
        {
            _score += _scorePerWordSize[_scorePerWordSize.Count-2];
        }

        _scoreDisplay.text = "Score : " + _score.ToString();
    }
    */

    public void SetMenuScoreDisplay()
    {
        _highscoreMenuDisplay.text = _highscore.ToString();
    }

    public void SetEndScreenScoreDisplay()
    {
        _scoreEndScreenDisplay.text = _score.ToString();
        _highscoreEndScreenDisplay.text = "Highscore : " + _highscore.ToString();

    }

}
