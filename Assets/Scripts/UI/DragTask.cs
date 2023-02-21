using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Devlike.Characters;

namespace Devlike.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class DragTask : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Character owner;
        private Vector2 centerPoint;
        private Vector2 worldCenterPoint => transform.TransformPoint(centerPoint);
        private Vector2 curPos;

        private void Awake()
        {
            centerPoint = (transform as RectTransform).rect.center;
            curPos = transform.position;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            DragTaskManager.instance.RegisterDraggedObject(this);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (DragTaskManager.instance.IsWithinBounds(worldCenterPoint + eventData.delta))
            {
                transform.Translate(eventData.delta);
            }
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            DragTaskManager.instance.UnregisterDraggedObject(this);
            if (DragTaskManager.instance.IsWithinDropAreaBounds(worldCenterPoint + eventData.delta))
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
}