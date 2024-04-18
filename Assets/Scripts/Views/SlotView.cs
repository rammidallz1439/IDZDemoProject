using UnityEngine;
using UnityEngine.EventSystems;

namespace IDZ.Game
{
    public class SlotView : MonoBehaviour, IDropHandler, IView
    {
        private SequencingController controller;
        public bool filled;
    
        public void Initialize(SequencingController controller)
        {
            this.controller = controller;
        }

        public void OnDrop(PointerEventData eventData)
        {
            ArrowView arrow = eventData.pointerDrag.GetComponent<ArrowView>();
            if (arrow != null)
            {
                filled=true;
                eventData.pointerDrag.GetComponent<RectTransform>().position=arrow.originalPos;
                GameObject go=Instantiate(arrow.gameObject,transform);
                controller.OnArrowDropped(go.GetComponent<ArrowView>(), this);
            }
        }

    }
}

