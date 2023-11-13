using System;

public class AIMoveHandeler
{
    private Random _randomGenerator = new Random();

    public EventHandler<MoveOptionType> NextMoveSelected;

    public void Initialize()
    {
        MoveManager.Instance.NewMoveStarted += OnNewMoveStarted;
    }

    public void Reset()
    {
        MoveManager.Instance.NewMoveStarted -= OnNewMoveStarted;
    }

    private void OnNewMoveStarted(object sender, EventArgs eventArgs)
    {
        if (BattleManager.Instance && BattleManager.Instance.IsBattleRunning && MoveManager.Instance.CurrentTurnType == TurnType.AI)
        {
            NextMoveSelected?.Invoke(this, GetARandomMoveOption());
        }
    }

    private MoveOptionType GetARandomMoveOption()
    {
        return (MoveOptionType)_randomGenerator.Next(0, (int)MoveOptionType.COUNT);
    }
}
