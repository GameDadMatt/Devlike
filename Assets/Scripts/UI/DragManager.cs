using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public static DragManager instance;

    [SerializeField]
    private RectTransform defaultLayer;
    [SerializeField]
    private RectTransform dragLayer;

    private List<RectTransform> dropAreas = new List<RectTransform>();

    private Rect boundingBox;

    private DragObject currentDraggedObject;
    public DragObject CurrentDraggedObject { get { return currentDraggedObject; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        boundingBox = BoundingBoxRect(dragLayer);
    }

    public void RegisterDropArea(RectTransform dropArea)
    {
        dropAreas.Add(dropArea);
    }

    public void UnregisterDropArea(RectTransform dropArea)
    {
        dropAreas.Remove(dropArea);
    }

    public void RegisterDraggedObject(DragObject drag)
    {
        currentDraggedObject = drag;
        drag.transform.SetParent(dragLayer);
    }

    public void UnregisterDraggedObject(DragObject drag)
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
        foreach(RectTransform area in dropAreas)
        {
            Rect bbox = BoundingBoxRect(area);
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
}