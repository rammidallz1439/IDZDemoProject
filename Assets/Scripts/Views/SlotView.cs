using UnityEngine;
using UnityEngine.EventSystems;

namespace IDZ.Game
{
    public class SlotView : MonoBehaviour, IDropHandler, IView
    {
        private SequencingController controller;

    
        public void Initialize(SequencingController controller)
        {
            this.controller = controller;
        }

        public void OnDrop(PointerEventData eventData)
        {
            ArrowView arrow = eventData.pointerDrag.GetComponent<ArrowView>();
            if (arrow != null)
            {
                controller.OnArrowDropped(arrow, this);
            }
        }
    }
}

