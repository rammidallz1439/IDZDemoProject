using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IDZ.Game
{
    public class LoopView : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler,IView
    {
        private SequencingController controller;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Canvas mainCanvas;
        private Vector2 originalPos;

        public void Initialize(SequencingController controller)
        {
            this.controller = controller;
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            mainCanvas = FindObjectOfType<Canvas>();
       
        }
 
        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
        }

  
        public void OnEndDrag(PointerEventData eventData)
        {
            rectTransform.position = originalPos;
            canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            originalPos = rectTransform.parent.transform.position;
        }
    }
}

