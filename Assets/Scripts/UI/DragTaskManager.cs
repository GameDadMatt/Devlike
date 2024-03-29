using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;
using Devlike.Tasks;

namespace Devlike.UI
{
    /// <summary>
    /// Manages dragged tasks in the UI
    /// </summary>
    public class DragTaskManager : MonoBehaviour
    {
        public static DragTaskManager instance;

        [SerializeField]
        private RectTransform dragLayer;
        public TaskColumn CurrentDragContainer { get; private set; }

        private List<TaskColumn> dropAreas;

        private Rect boundingBox;

        private DragTask currentDraggedObject;
        public DragTask CurrentDraggedObject { get { return currentDraggedObject; } }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            boundingBox = BoundingBoxRect(dragLayer);
        }

        public void SetContainerAreas(List<TaskColumn> dropAreas)
        {
            this.dropAreas = dropAreas;
        }

        public void RegisterDraggedObject(DragTask drag)
        {
            drag.transform.SetParent(dragLayer);
            currentDraggedObject = drag;
        }

        public void UnregisterDraggedObject(DragTask drag)
        {
            currentDraggedObject = null;
        }

        public bool IsWithinBounds(Vector2 position)
        {
            return boundingBox.Contains(position);
        }

        public bool IsWithinDropAreaBounds(Vector2 position)
        {
            foreach (TaskColumn container in dropAreas)
            {
                Rect bbox = BoundingBoxRect(container.Area);
                if (bbox.Contains(position))
                {
                    CurrentDragContainer = container;
                    return true;
                }
            }

            return false;
        }

        public bool IsWithinDropAreaBoundsOfType(Vector2 position, TaskType type)
        {
            foreach (TaskColumn container in dropAreas)
            {
                Rect bbox = BoundingBoxRect(container.Area);
                if (bbox.Contains(position))
                {
                    if (!container.onlyOneType || container.onlyOneType && container.type == type)
                    {
                        CurrentDragContainer = container;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public TaskColumn GetTaskColumn(Vector2 position)
        {
            foreach (TaskColumn container in dropAreas)
            {
                Rect bbox = BoundingBoxRect(container.Area);
                if (bbox.Contains(position))
                {
                    return container;
                }
            }

            Debug.LogError("Failed to find TaskColumn at position " + position);
            return null;
        }

        private Rect BoundingBoxRect(RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var position = corners[0];

            Vector2 size = new Vector2(
                rectTransform.lossyScale.x * rectTransform.rect.size.x,
                rectTransform.lossyScale.y * rectTransform.rect.size.y);

            return new Rect(position, size);
        }

        private List<GameObject> OrderByTaskPosition()
        {
            return null;
        }
    }
}