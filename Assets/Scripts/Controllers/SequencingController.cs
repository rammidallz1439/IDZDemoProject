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
        public List<ArrowDirection> goalSequence;
        public IModel model;


        private List<ICommand> commandBuffer = new List<ICommand>();
        [SerializeField] private Transform[] puzzleDots;
        [SerializeField] private int currentIndex;
        void Start()
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

            foreach (SlotView slotView in slots)
            {
                slotView.Initialize(this);
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
            StartCoroutine(MovePencil());
            //DrawLines();

            if (model.CheckWinCondition())
            {
                Debug.Log("You win!");

            }
            else
            {
                Debug.Log("Try again!");

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
                        pencilUI.DOMoveY(puzzleDots[currentIndex - 4].transform.position.y + 0.1f, 0.7f).WaitForCompletion();
                        // DrawLine(puzzleDots[currentIndex].transform.position, puzzleDots[currentIndex - 4].transform.position);
                        currentIndex = currentIndex - 4;
                        break;
                    case ArrowDirection.Down:
                        pencilUI.DOMoveY(puzzleDots[currentIndex + 4].transform.position.y +0.1f , 0.7f).WaitForCompletion();
                        //  DrawLine(puzzleDots[currentIndex].transform.position, puzzleDots[currentIndex + 4].transform.position);
                        currentIndex = currentIndex + 4;
                        break;
                    case ArrowDirection.Left:
                        pencilUI.DOMoveX(puzzleDots[currentIndex + 1].transform.position.x + 0.1f, 0.7f).WaitForCompletion();
                        //  DrawLine(puzzleDots[currentIndex].transform.position, puzzleDots[currentIndex + 1].transform.position);
                        currentIndex = currentIndex + 1;
                        break;
                    case ArrowDirection.Right:
                        pencilUI.DOMoveX(puzzleDots[currentIndex - 1].transform.position.x + 0.1f, 0.7f).WaitForCompletion();
                        //  DrawLine(puzzleDots[currentIndex].transform.position, puzzleDots[currentIndex - 1].transform.position);
                        currentIndex = currentIndex - 1;
                        break;
                }
                yield return new WaitForSeconds(0.5f);
            }

        }

        private void DrawLines()
        {
            // Clear previous lines
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }



            // Draw lines between arrows
            for (int i = 0; i < slots.Length - 1; i++)
            {
                if (slots[i].transform.childCount > 0 && slots[i + 1].transform.childCount > 0)
                {
                    ArrowView arrow = slots[i].transform.GetChild(0).GetComponent<ArrowView>();
                    ArrowView nextArrow = slots[i + 1].transform.GetChild(0).GetComponent<ArrowView>();
                    if (arrow != null && nextArrow != null)
                    {
                        Vector3 startPosition = GetArrowEndPosition(arrow);
                        Vector3 endPosition = GetArrowStartPosition(nextArrow);
                        DrawLine(startPosition, endPosition);
                    }
                }
            }
        }

        private void DrawLine(Vector3 startPos, Vector3 endPos)
        {
            GameObject line = Instantiate(linePrefab, startPos, Quaternion.identity);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }

        private Vector3 GetArrowEndPosition(ArrowView arrow)
        {
            switch (arrow.Direction)
            {
                case ArrowDirection.Up:
                    return arrow.transform.position + Vector3.up * 0.5f;
                case ArrowDirection.Down:
                    return arrow.transform.position + Vector3.down * 0.5f;
                case ArrowDirection.Left:
                    return arrow.transform.position + Vector3.left * 0.5f;
                case ArrowDirection.Right:
                    return arrow.transform.position + Vector3.right * 0.5f;
                default:
                    return arrow.transform.position;
            }
        }

        private Vector3 GetArrowStartPosition(ArrowView arrow)
        {
            switch (arrow.Direction)
            {
                case ArrowDirection.Up:
                    return arrow.transform.position - Vector3.up * 0.5f;
                case ArrowDirection.Down:
                    return arrow.transform.position - Vector3.down * 0.5f;
                case ArrowDirection.Left:
                    return arrow.transform.position - Vector3.left * 0.5f;
                case ArrowDirection.Right:
                    return arrow.transform.position - Vector3.right * 0.5f;
                default:
                    return arrow.transform.position;
            }
        }


    }

}

