using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class FrustrationManager : MonoBehaviour
{

    private Slider _slider;

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
    }

    public void DecrementFrustration()
    {
        // Decrement frustration
        _frustration -= _frustrationLossOnWin / 100f;
    }
}
