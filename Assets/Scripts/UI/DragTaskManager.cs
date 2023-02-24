using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devlike.Characters;
using Devlike.Tasks;

namespace Devlike.UI
{
    public class DragTaskManager : MonoBehaviour
    {
        public static DragTaskManager instance;

        [SerializeField]
        private RectTransform defaultLayer;
        [SerializeField]
        private RectTransform dragLayer;

        private List<ContainerArea> dropAreas;

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

        public void SetContainerAreas(List<ContainerArea> dropAreas)
        {
            this.dropAreas = dropAreas;
        }

        public void RegisterDraggedObject(DragTask drag)
        {
            currentDraggedObject = drag;
            drag.transform.SetParent(dragLayer);
        }

        public void UnregisterDraggedObject(DragTask drag)
        {
            drag.transform.SetParent(defaultLayer);
            currentDraggedObject = null;
        }

        public bool IsWithinBounds(Vector2 position)
        {
            return boundingBox.Contains(position);
        }

        public bool IsWithinDropAreaBounds(Vector2 position)
        {
            foreach (ContainerArea container in dropAreas)
            {
                Rect bbox = BoundingBoxRect(container.Area);
                Debug.Log("Drop Area");
                if (bbox.Contains(position))
                {
                    return true;
                }
            }

            return false;
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

        private void AssignTaskToList(RectTransform area)
        {

        }
    }
}