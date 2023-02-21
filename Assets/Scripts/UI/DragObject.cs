using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 centerPoint;
    private Vector2 worldCenterPoint => transform.TransformPoint(centerPoint);
    private Vector2 curPos;

    private void Awake()
    {
        centerPoint = (transform as RectTransform).rect.center;
        curPos = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragManager.instance.RegisterDraggedObject(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DragManager.instance.IsWithinBounds(worldCenterPoint + eventData.delta))
        {
            transform.Translate(eventData.delta);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragManager.instance.UnregisterDraggedObject(this);
        if (DragManager.instance.IsWithinDropAreaBounds(worldCenterPoint + eventData.delta))
        {
            curPos = transform.position;
        }
        else
        {
            //Return to the starting position
            transform.position = curPos;
        }
    }
}