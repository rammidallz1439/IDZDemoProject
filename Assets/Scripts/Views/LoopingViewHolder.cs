using IDZ.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IDZ.Game
{
    public class LoopingViewHolder : MonoBehaviour, IDropHandler, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private SequencingController controller;

        public bool filled;
        public Transform spawnPoint;
        public Animator anim;
        public int loopCount = 1;
        public List<ArrowDirection> direction = new List<ArrowDirection>();


        [SerializeField] private GameObject[] ExtendedArrow;
        [SerializeField] private TMP_Text loopCountText;
        private RectTransform rectTransform;
        private Canvas mainCanvas;
        private CanvasGroup canvasGroup;
        private Vector2 origin;
        void OnEnable()
        {
            controller = FindObjectOfType<SequencingController>();
            rectTransform = GetComponent<RectTransform>();
            mainCanvas=FindObjectOfType<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ChangeAnimationStates(Animator anim, string newState)
        {
            string currentState = null;
            if (currentState == newState) return;
            anim.Play(newState);
            currentState = newState;

        }
        ArrowDirection dir;
        public void OnDrop(PointerEventData eventData)
        {
            if (!filled)
            {
                ArrowView arrow = eventData.pointerDrag.GetComponent<ArrowView>();
                ChangeAnimationStates(anim, GameConstants.LoopHolderExtendAnim);
                if (arrow != null)
                {
                    filled = true;
                   // controller.model.AddToSequence(arrow.Direction);
                    direction.Add(arrow.Direction);
                    ChangeExtendedArrow(arrow.Direction);
                    dir = arrow.Direction;
                }

            }

        }
        public void ChangeExtendedArrow(ArrowDirection dir)
        {
            ArrowView obj = null;
            switch (dir)
            {
                case ArrowDirection.Up:
                    obj = Instantiate(ExtendedArrow[0], spawnPoint).transform.GetComponent<ArrowView>();
                    obj.isSelected = true;
                    break;
                case ArrowDirection.Down:
                    obj = Instantiate(ExtendedArrow[1], spawnPoint).transform.GetComponent<ArrowView>();
                    obj.isSelected = true;
                    break;
                case ArrowDirection.Left:
                    obj = Instantiate(ExtendedArrow[3], spawnPoint).transform.GetComponent<ArrowView>();
                    obj.isSelected = true;
                    break;
                case ArrowDirection.Right:
                    obj = Instantiate(ExtendedArrow[2], spawnPoint).transform.GetComponent<ArrowView>();
                    obj.isSelected = true; break;
                default:
                    break;
            }
            obj.controller = this.controller;
        }

        public void OnLoopNumberButton()
        {
            if (loopCount < 3)
            {
               loopCount++;
                controller.loopCount++;
                loopCountText.text = loopCount.ToString();
                direction.Add(dir);
            }
          

        }

        public void OnPointerDown(PointerEventData eventData)
        {
           origin= eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            if (filled)
            {
                direction.Clear();
            }
            Destroy(this.gameObject);
        }
    }
}
