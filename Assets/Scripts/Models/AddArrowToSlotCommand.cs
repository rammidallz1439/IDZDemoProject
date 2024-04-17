using UnityEngine;

namespace IDZ.Game
{
    public class AddArrowToSlotCommand : ICommand
    {
        private ArrowView arrow;
        private SlotView slot;
        private Transform arrowParent;
        private Vector3 arrowPosition;

        public AddArrowToSlotCommand(ArrowView arrow, SlotView slot)
        {
            this.arrow = arrow;
            this.slot = slot;
            this.arrowParent = arrow.transform.parent;
            this.arrowPosition = arrow.transform.localPosition;
        }

        public void Execute()
        {
            arrow.transform.SetParent(slot.transform);
            arrow.transform.localPosition = Vector3.zero;
        }

        public void Undo()
        {
            arrow.transform.SetParent(arrowParent);
            arrow.transform.localPosition = arrowPosition;
        }
    }
}

