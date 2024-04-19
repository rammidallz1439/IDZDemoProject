using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;
using DG;
using DG.Tweening;
using System.Collections;
using System;
namespace IDZ.Game
{
    public class SequencingController : MonoBehaviour
    {
        public Transform pencilUI;
        public GameObject linePrefab;
        public SlotView[] slots;
        public ArrowView[] arrowViews;
        public LoopView[] loopViews;
        public List<LoopingViewHolder> loopViewHolders;
        public List<ArrowDirection> goalSequence;
        public IModel model;
        public GameType gameType;
        public LoopingViewHolder loopingviewPrefab = null;
        public int loopCount = 1;

        private List<ICommand> commandBuffer = new List<ICommand>();
        [SerializeField] private Transform[] puzzleDots;
        [SerializeField] private int currentIndex;
        [SerializeField] private int loopsToWin = 0;
        [SerializeField] private GameObject loopingHolder = null;

        void OnEnable()
        {
            model = new SequencingModel();
            ((SequencingModel)model).SetGoalSequence(goalSequence);
            InitializeViews();

        }

        private void InitializeViews()
        {
            foreach (ArrowView arrowView in arrowViews)
            {
                arrowView.Initialize(this);
            }
            if (slots != null)
            {
                foreach (SlotView slotView in slots)
                {
                    slotView.Initialize(this);
                }
            }
            if (loopViews != null)
            {
                foreach (LoopView loopView in loopViews)
                {
                    loopView.Initialize(this);
                }
            }


        }

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            commandBuffer.Add(command);
        }

        public void UndoLastCommand()
        {
            if (commandBuffer.Count > 0)
            {
                ICommand lastCommand = commandBuffer[commandBuffer.Count - 1];
                lastCommand.Execute();
                commandBuffer.RemoveAt(commandBuffer.Count - 1);
            }
        }

        public void OnOkButtonClick()
        {
           
            //DrawLines();
            if(gameType == GameType.Looping)
            {
                foreach(LoopingViewHolder loopView in loopViewHolders)
                {
                    for (int i = 0; i < loopView.direction.Count; i++)
                    {
                        model.AddToSequence(loopView.direction[i]);
                    }
                  
                }
               
            }

            StartCoroutine(MovePencil());

            if (!model.CheckWinCondition())
            {
                Debug.Log("Try again!");
                GameSceneManager.SwitchNextButtonevent?.Invoke();

            }
            commandBuffer.Clear();
        }

        public void OnArrowDropped(ArrowView arrow, SlotView slot)
        {
            arrow.transform.SetParent(slot.transform);
            ICommand command = new AddArrowToSlotCommand(arrow, slot);
            ExecuteCommand(command);
            model.AddToSequence(arrow.Direction);
        }
        private IEnumerator MovePencil()
        {
            Vector3 destination;

            foreach (ArrowDirection direction in model.GetSequence())
            {
                switch (direction)
                {
                    case ArrowDirection.Up:
                        DrawLine(puzzleDots[currentIndex].transform, new Vector3(0, 140, 0));
                        pencilUI.DOMoveY(puzzleDots[currentIndex - 4].transform.position.y - 0.1f, 0.7f).WaitForCompletion();
                        currentIndex = currentIndex - 4;
                        break;
                    case ArrowDirection.Down:
                        DrawLine(puzzleDots[currentIndex].transform, new Vector3(0, -140, 0));
                        pencilUI.DOMoveY(puzzleDots[currentIndex + 4].transform.position.y + 0.1f, 0.7f).WaitForCompletion();
                        currentIndex = currentIndex + 4;
                        break;
                    case ArrowDirection.Left:
                        if (currentIndex != 3 && currentIndex != 7 && currentIndex != 11 && currentIndex != 15)
                        {
                            DrawLine(puzzleDots[currentIndex].transform, new Vector3(120, 0, 0));
                            pencilUI.DOMoveX(puzzleDots[currentIndex + 1].transform.position.x + 0.1f, 0.7f).WaitForCompletion();
                            currentIndex = currentIndex + 1;
                        }
                        break;
                    case ArrowDirection.Right:
                        if(currentIndex != 0 && currentIndex != 4 && currentIndex != 8 && currentIndex != 12)
                        {
                            DrawLine(puzzleDots[currentIndex].transform, new Vector3(-120, 0, 0));
                            pencilUI.DOMoveX(puzzleDots[currentIndex - 1].transform.position.x + 0.1f, 0.7f).WaitForCompletion();
                            currentIndex = currentIndex - 1;
                        }
                      
                        break;
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.8f);
            GameSceneManager.ResultPopupEvent?.Invoke(model.CheckWinCondition());
        }


        private void DrawLine(Transform startPos, Vector3 endPos)
        {
            GameObject line = Instantiate(linePrefab, startPos);
            line.transform.position = startPos.position;
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos.position);
            lineRenderer.SetPosition(1, endPos);

        }

    }


    public enum GameType
    {
        Sequencing,
        Looping
    }
}

