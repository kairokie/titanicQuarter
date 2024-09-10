using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class FrustrationManager : MonoBehaviour
{

    private Slider _slider;
    
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
    private float _frustrationLossOnWin;

    [SerializeField]
    [Tooltip("In percents")]
    public float _frustrationPerSeconds;



    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        IncrementFrustration();
    }

    public void IncrementFrustration()
    {
        // Increment frustration
        _frustration += _frustrationPerSeconds / 100f * Time.deltaTime;
        UpdateFrustrationColor();
    }

    public void DecrementFrustration()
    {
        // Decrement frustration
        _frustration -= _frustrationLossOnWin / 100f;
    }

    public void UpdateFrustrationColor()
    {
        _fill.color = Color.Lerp(_colorStart, _colorEnd, _curve.Evaluate(_frustration));
    }
}
