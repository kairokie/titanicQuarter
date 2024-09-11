using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NauticalSpot : MonoBehaviour, IDropHandler
{
    public NauticalFlag _attachedFlag;

    [HideInInspector]
    public float _distanceBetweenSpots;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out NauticalFlag nauticalFlag))
        {
            if (_attachedFlag)
            {
                float dist = GetComponent<RectTransform>().sizeDelta.y + _distanceBetweenSpots;
                dist *= transform.parent.lossyScale.y;

                _attachedFlag._attachedSpot = null;
                _attachedFlag.transform.SetParent(transform.parent, true);
                _attachedFlag.transform.position = transform.position + transform.parent.up * dist;
                _attachedFlag = null;
            }

            nauticalFlag.transform.SetParent(transform, true);
            nauticalFlag.transform.position = transform.position;
            nauticalFlag._attachedSpot = this;
            _attachedFlag = nauticalFlag;
        }
    }
}
