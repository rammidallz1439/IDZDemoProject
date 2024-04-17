using System.Collections.Generic;


namespace IDZ.Game
{
    public interface IModel
    {
        void SetGoalSequence(List<ArrowDirection> goal);
        void AddToSequence(ArrowDirection direction);
        void ClearSequence();
        List<ArrowDirection> GetSequence();
        bool CheckWinCondition();
    }
}

