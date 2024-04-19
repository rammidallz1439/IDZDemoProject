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
            if (controller.gameType == GameType.Sequencing)
            {
                ArrowView arrow = eventData.pointerDrag.GetComponent<ArrowView>();
                if (!filled)
                {

                    if (arrow != null)
                    {
                        filled = true;

                        GameObject go = Instantiate(arrow.gameObject, transform);
                        controller.OnArrowDropped(go.GetComponent<ArrowView>(), this);
                    }
                }
                eventData.pointerDrag.GetComponent<RectTransform>().position = arrow.originalPos;
            }else if(controller.gameType == GameType.Looping)
            {
                if (!filled)
                {
           
                    ArrowView arrow = eventData.pointerDrag.GetComponent<ArrowView>();
                    LoopView loop=eventData.pointerDrag.GetComponent<LoopView>();
                    if(loop != null)
                    {
                        LoopingViewHolder LoopView = Instantiate(controller.loopingviewPrefab.gameObject,this.transform).transform.GetComponent<LoopingViewHolder>();
                        controller.loopViewHolders.Add(LoopView);
                    }
                }
            }
         
        }

    }
}

