using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Devlike.Tasks;

namespace Devlike.UI
{
    /// <summary>
    /// An individual task that can be dragged around on the UI
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class DragTask : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private TaskContainer task;
        private Transform curParent;
        private Vector2 centerPoint;
        private Vector2 worldCenterPoint => transform.TransformPoint(centerPoint);
        private Vector2 curPos;

        public void Awake()
        {
            task = GetComponent<TaskUIObject>().Task;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            curParent = transform.parent;
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
            if (DragTaskManager.instance.IsWithinDropAreaBoundsOfType(worldCenterPoint + eventData.delta, task.Type))
            {
                RectTransform rt = DragTaskManager.instance.CurrentDragContainer.Area;
                curParent = rt.transform;
                transform.SetParent(curParent);

                int children = rt.childCount;
                for (int i = 0; i < children; i++)
                {
                    if (i != transform.GetSiblingIndex())
                    {
                        Transform oc = rt.transform.GetChild(i);
                        if (oc.position.y < transform.position.y)
                        {
                            transform.SetSiblingIndex(i);
                            break;
                        }
                    }
                }

                curPos = transform.position;
            }
            else
            {
                //Return to the starting position
                transform.position = curPos;
                transform.SetParent(curParent);
            }            
        }
    }
}