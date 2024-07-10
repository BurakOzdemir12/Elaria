/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;



    private void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

    }


    public void OnBeginDrag(PointerEventData eventData)
    {

        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        //So the ray cast will ignore the item itself.
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        
        itemBeingDragged = gameObject;

    }

    public void OnDrag(PointerEventData eventData)
    {
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta ;
        //  / canvas.scaleFactor
    }



    public void OnEndDrag(PointerEventData eventData)
    {

        itemBeingDragged = null;

        if (transform.parent == startParent || transform.parent == transform.root)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);

        }

        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }



}*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool _resetPositionOnRelease = true;

    public static GameObject itemBeingDragged;

    Vector3 _startPosition;

    public void OnDrag(PointerEventData eventData)
    {
       // Debug.Log($"Dragging {eventData.position}");
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log($"Begin Drag {eventData.position}");

        if (_resetPositionOnRelease)
            _startPosition = transform.position;
        itemBeingDragged = gameObject;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       // Debug.Log($"End Drag {eventData.position}");
        itemBeingDragged = null;
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);

        var hit = hits.FirstOrDefault(t => t.gameObject.CompareTag("Droppable"));
        if (hit.isValid)
        {
            Debug.Log($"Dropped {gameObject} on {hit.gameObject}");
            return;
        }

        if (_resetPositionOnRelease)
            transform.position = _startPosition;
    }
}
