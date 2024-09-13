using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class FrustrationManager : MonoBehaviour
{

    private Slider _slider;

    [SerializeField]
    private menuManager _menuManager;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private ScoreManager _scoreManager;

    [SerializeField]    
    Image _fill;

    [SerializeField]
    Color _colorStart;
    [SerializeField]
    Color _colorEnd;

    [SerializeField]
    AnimationCurve _curve;

    // Clamped between 0 and 1
    float _frustration
    {
        get { return _slider.value; }
        set { _slider.value = value; }
    }

    [SerializeField]
    private List<int> _frustrationLossPerSize;

    [SerializeField]
    [Tooltip("In percents")]
    public float _frustrationPerSeconds;

    [SerializeField]
    protected FMODUnity.StudioEventEmitter _calmSound;

    [SerializeField]
    protected FMODUnity.StudioEventEmitter _ticTacSound;

    [SerializeField]
    protected float _percentageTicTacStart = 0.84f;

    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isPaused)
        {
            IncrementFrustration();
            if (_frustration >= 1) // lose
            {
                _menuManager.EndGame();
                _scoreManager.SetEndScreenScoreDisplay();
                _gameManager.Pause();
            }
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PANIC", _frustration * 100);
            GameManager game = FindObjectOfType<GameManager>();
            if (game)
            {
                game._musicSound.SetParameter("TIME", (1 - _frustration) * 60);
            }

        }
    }

    public void IncrementFrustration()
    {
        // Increment frustration
        _frustration += _frustrationPerSeconds / 100f * Time.deltaTime;
        if(_frustration > _percentageTicTacStart && !_ticTacSound.IsPlaying())
        {
            _ticTacSound?.Play();
        }
        UpdateFrustrationColor();
    }

    public void DecrementFrustrationWithWordSize(int size)
    {
        if (size <= 1) return;
        _calmSound?.Play();
        if (size <= _frustrationLossPerSize.Count)
        {
            _frustration -= _frustrationLossPerSize[size-2] / 100f;
        }
        else
        {
            _frustration -= _frustrationLossPerSize[_frustrationLossPerSize.Count-2] / 100f;
        }

        UpdateFrustrationColor();

    }

    public void UpdateFrustrationColor()
    {
        _fill.color = Color.Lerp(_colorStart, _colorEnd, _curve.Evaluate(_frustration));
    }
}
