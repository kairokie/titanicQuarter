using System.Collections;
using System.Collections.Generic;
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
        _image.raycastTarget = false;
        _aspectRatioFitter.enabled = false;

        if (_attachedSpot)
        {
            _layoutElement.ignoreLayout = true;
            transform.SetParent(_attachedSpot.transform.parent, true);
            _attachedSpot._attachedFlag = null;
            _attachedSpot = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.raycastTarget = true;


    }
}
