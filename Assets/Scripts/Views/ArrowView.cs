// ArrowView.cs
using UnityEngine;
using UnityEngine.EventSystems;

namespace IDZ.Game
{
    public class ArrowView : MonoBehaviour,IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IView
    {
        private SequencingController controller;
        [SerializeField]private ArrowDirection direction;

        public ArrowDirection Direction => direction;
        private RectTransform rectTransform;

        void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        public void Initialize(SequencingController controller)
        {
            this.controller = controller;
            
        }

       
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            // Make the arrow transparent while dragging
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Move the arrow with the pointer
            rectTransform.anchoredPosition += eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Make the arrow visible again
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
          
        }
    }
}

