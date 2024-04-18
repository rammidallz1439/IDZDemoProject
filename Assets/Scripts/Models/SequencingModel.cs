using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IDZ.Game
{
    public class SequencingModel : IModel
    {
        private List<ArrowDirection> sequence = new List<ArrowDirection>();
       private List<ArrowDirection> goalSequence = new List<ArrowDirection>();

        public void SetGoalSequence(List<ArrowDirection> goal)
        {
            goalSequence = new List<ArrowDirection>(goal);
        }

        public void AddToSequence(ArrowDirection direction)
        {
            sequence.Add(direction);
        }

        public void ClearSequenceAt(int index)
        {
            sequence.RemoveAt(index);
        }
        public void ClearSequence()
        {
            sequence.Clear();
        }

        public List<ArrowDirection> GetSequence()
        {
            return sequence;
        }

        public bool CheckWinCondition()
        {
            if (sequence.Count != goalSequence.Count)
                return false;

            for (int i = 0; i < sequence.Count; i++)
            {
                if (sequence[i] != goalSequence[i])
                    return false;
            }

            return true;
        }
    }


    public enum ArrowDirection
    {
        Up,
        Down, 
        Left, 
        Right
    }
}

