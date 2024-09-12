using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NauticalSpot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    public NauticalFlag _attachedFlag;
    public TextMeshProUGUI _previewLetter;

    [HideInInspector]
    public float _distanceBetweenSpots;
 

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out NauticalFlag nauticalFlag))
        {
            //
            NauticalSpot tempSpot = nauticalFlag._attachedSpot;
            // NF2 
            NauticalFlag tempFlag = _attachedFlag;

            //nauticalFlag = tempFlag;
            nauticalFlag.transform.SetParent(transform, true);
            nauticalFlag.transform.position = transform.position;
            nauticalFlag._attachedSpot = this;
            nauticalFlag._isSlotDropped = true;
            // NS2 flag = NF1 Correct
            _attachedFlag = nauticalFlag;

            nauticalFlag._layoutElement.ignoreLayout = false;
            nauticalFlag._aspectRatioFitter.enabled = true;


            if (tempSpot && tempFlag)
            {

                tempFlag._attachedSpot = tempSpot;
                tempSpot._attachedFlag = tempFlag;

                tempFlag.transform.SetParent(tempSpot.transform, true);
                tempFlag.transform.position = tempSpot.transform.position;

               
            }
            else if (tempFlag != null)
            {
                tempFlag._layoutElement.ignoreLayout = true;
                tempFlag._aspectRatioFitter.enabled = false;
                tempFlag._attachedSpot = null;
                tempFlag.transform.SetParent(transform.parent, true);

                float dist = GetComponent<RectTransform>().sizeDelta.y + _distanceBetweenSpots;
                dist *= transform.parent.lossyScale.y;
                tempFlag.transform.position = transform.position + transform.parent.up * dist;
            }
            else
            {
                _attachedFlag = nauticalFlag;
                nauticalFlag._attachedSpot = this;
            }
        }
    }
}
