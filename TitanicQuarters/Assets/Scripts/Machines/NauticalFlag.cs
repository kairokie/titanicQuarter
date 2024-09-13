using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NauticalFlag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public int _flagId;
    public NauticalSpot _attachedSpot;

    private Image _image;
    [HideInInspector]
    public LayoutElement _layoutElement;
    [HideInInspector]
    public AspectRatioFitter _aspectRatioFitter;

    public bool _isSlotDropped = false;


    private void Start()
    {
        _image = GetComponent<Image>();
        _layoutElement = GetComponent<LayoutElement>();
        _aspectRatioFitter = GetComponent<AspectRatioFitter>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.isValid)
        {
            transform.position = eventData.pointerCurrentRaycast.worldPosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Nautical nautical = FindObjectOfType<Nautical>();
        nautical._nauticDragSound?.Play();
        _image.raycastTarget = false;
        _aspectRatioFitter.enabled = false;
        _isSlotDropped = false;

        if (_attachedSpot)
        {
            _layoutElement.ignoreLayout = true;
            transform.SetParent(_attachedSpot.transform.parent, true);
            _attachedSpot._attachedFlag = null;
        }
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Nautical nautical = FindObjectOfType<Nautical>();
        nautical._nauticDropSound?.Play();
        _image.raycastTarget = true;
        if (!_isSlotDropped)
        {
            _attachedSpot = null;
            _isSlotDropped = false;
        }
      
        //_switch = false;
    }
}
