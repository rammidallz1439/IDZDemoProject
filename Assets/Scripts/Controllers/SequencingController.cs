using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;

namespace IDZ.Game
{
    public class SequencingController : MonoBehaviour
    {
        public GameObject pencilUI;
        public GameObject linePrefab;
        public SlotView[] slots;
        public ArrowView[] arrowViews;
        public List<ArrowDirection> goalSequence;

        private IModel model;
        private List<ICommand> commandBuffer = new List<ICommand>();

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
                lastCommand.Execute(); // Implement Undo logic in each command if needed
                commandBuffer.RemoveAt(commandBuffer.Count - 1);
            }
        }

        public void OnOkButtonClick()
        {
            MovePencil();
            DrawLines();

            if (model.CheckWinCondition())
            {
                Debug.Log("You win!");
                // Implement win condition logic, such as showing a win screen or advancing to the next level
            }
            else
            {
                Debug.Log("Try again!");
                // Implement lose condition logic, such as showing a lose screen or resetting the game
            }

            commandBuffer.Clear();
        }

        public void OnArrowDropped(ArrowView arrow, SlotView slot)
        {
            // Clear the slot before adding the arrow
            foreach (SlotView s in slots)
            {
                if (s != slot && s.transform.childCount > 0)
                {
                    Destroy(s.transform.GetChild(0).gameObject);
                }
            }

            arrow.transform.SetParent(slot.transform);
            ICommand command = new AddArrowToSlotCommand(arrow, slot);
            ExecuteCommand(command);
        }
        private void MovePencil()
        {
            Vector3 currentPosition = pencilUI.transform.position;
            foreach (ArrowDirection direction in model.GetSequence())
            {
                switch (direction)
                {
                    case ArrowDirection.Up:
                        currentPosition += Vector3.up;
                        break;
                    case ArrowDirection.Down:
                        currentPosition += Vector3.down;
                        break;
                    case ArrowDirection.Left:
                        currentPosition += Vector3.left;
                        break;
                    case ArrowDirection.Right:
                        currentPosition += Vector3.right;
                        break;
                }
            }
            pencilUI.transform.position = currentPosition;
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
            GameObject line = Instantiate(linePrefab, transform);
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

