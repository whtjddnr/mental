using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class _Card : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("asd");
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}