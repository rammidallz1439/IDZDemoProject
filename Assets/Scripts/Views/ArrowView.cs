// ArrowView.cs
using UnityEngine;
using UnityEngine.EventSystems;

namespace IDZ.Game
{
    public class ArrowView : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IView
    {
        public SequencingController controller;

        [SerializeField] private ArrowDirection direction;
        public Vector2 originalPos;
        public ArrowDirection Direction => direction;
        public bool isSelected;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Canvas mainCanvas;

        void Start()
        {

            if (controller.gameType == GameType.Looping)
            {

                rectTransform = GetComponent<RectTransform>();
                canvasGroup = GetComponent<CanvasGroup>();
                mainCanvas = FindObjectOfType<Canvas>();
            }


        }
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
            if (isSelected)
            {

                controller.model.ClearSequence();
                this.gameObject.transform.parent = null;
                if (controller.gameType == GameType.Sequencing)
                {
                    foreach (SlotView item in controller.slots)
                    {
                        if (item.filled)
                        {
                            if (item.transform.childCount > 0)
                            {
                                controller.model.AddToSequence(item.transform.GetChild(0).transform.GetComponent<ArrowView>().Direction);
                            }
                            else
                            {
                                item.filled = false;
                            }


                        }

                    }
                }
                else
                {

                    foreach (LoopingViewHolder item in controller.loopViewHolders)
                    {
                        if (item.filled)
                        {
                            if (item.spawnPoint.transform.childCount > 0)
                            {
                                controller.model.AddToSequence(item.spawnPoint.transform.GetChild(0).transform.GetComponent<ArrowView>().Direction);
                            }
                            else
                            {
                                item.filled = false;
                                item.ChangeAnimationStates(item.anim, GameConstants.LoopHolderIdleAnim);
    }
                        }
                    }

                }
                Destroy(this.gameObject);
            }
            else
            {
                rectTransform.position = originalPos;
            }
            canvasGroup.blocksRaycasts = true;


        }
        public void OnPointerDown(PointerEventData eventData)
        {
            originalPos = rectTransform.parent.transform.position;
        }


    }
}

