using System;

public class BattleData
{
    // Number of moves played by the player in the ongoing battle.
    public int NumMovesPlayed { get; private set; } = 0;

    // Last move that the AI played.
    public MoveOptionType LastSelectedAIMoveType { get; private set; }

    public static EventHandler<int> NumMovesPlayedChanged;

    public void UpdateNumMovesPlayed()
    {
        NumMovesPlayed = MoveManager.Instance.NumTotalTurnsPlayed/2;

        NumMovesPlayedChanged?.Invoke(this, NumMovesPlayed);
    }

    public void UpdateLastAISelectedMove(MoveOptionType moveOptionType) {
        LastSelectedAIMoveType = moveOptionType;
    }
}
