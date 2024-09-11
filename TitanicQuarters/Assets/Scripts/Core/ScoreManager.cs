using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    public List<int> _scorePerWordSize;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private TextMeshProUGUI _scoreDisplay;

    public void IncrementScoreWithWordSize(int size)
    {
        if (size <= 0) return;

        if (size <= _scorePerWordSize.Count)
        {
            _score += _scorePerWordSize[size-1];
        }
        else
        {
            _score += _scorePerWordSize[_scorePerWordSize.Count-1];
        }

        _scoreDisplay.text = "Score : " + _score.ToString();
    }
}
