using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnType
{
    PLAYER = 0,
    AI = 1,
}

public enum MoveState
{
    NOT_STARTED = 0,
    IN_PROGRESS = 1,
}

public struct MoveData {
    public TurnType CurrTurnType;
    public MoveOptionType CurrMoveOptionType;

    public MoveData(TurnType turnType, MoveOptionType moveOptionType)
    {
        CurrTurnType = turnType;
        CurrMoveOptionType = moveOptionType;
    }
}

public class MoveManager : Singleton<MoveManager>
{
    /// <summary>
    /// Sum of all player's and AI's moves 
    /// </summary>
    public int NumTotalTurnsPlayed { get; private set; }

    public TurnType CurrentTurnType { get => NumTotalTurnsPlayed % 2 == 0 ? TurnType.AI : TurnType.PLAYER; }

    public EventHandler<MoveData> MoveCompleted;
    public EventHandler NewMoveStarted;

    private AIMoveHandeler _aiMoveHandeler = null;
    private PlayerMoveHandeler _playerMoveHandeler = null;

    public void Initialize(AIMoveHandeler aiMoveHandeler, PlayerMoveHandeler playerMoveHandeler)
    {
        NumTotalTurnsPlayed = 0;
        _aiMoveHandeler = aiMoveHandeler;
        _playerMoveHandeler = playerMoveHandeler;

        ConnectListeners();

        TriggerNextMove();
    }

    public void Reset()
    {
        DisconnectListeners();

        _aiMoveHandeler = null;
        _playerMoveHandeler = null;
    }

    private void ConnectListeners()
    {
        _aiMoveHandeler.NextMoveSelected += OnMoveSelected;
        _playerMoveHandeler.NextMoveSelected += OnMoveSelected;
    }

    private void DisconnectListeners()
    {
        _aiMoveHandeler.NextMoveSelected -= OnMoveSelected;
        _playerMoveHandeler.NextMoveSelected -= OnMoveSelected;
    }

    private void OnMoveSelected(object sender, MoveOptionType moveOptionType)
    {
        if (BattleManager.Instance && BattleManager.Instance.IsBattleRunning)
        {
            TurnType cachedTurnType = CurrentTurnType;

            NumTotalTurnsPlayed++;

            MoveCompleted?.Invoke(this, new MoveData(cachedTurnType, moveOptionType));

            TriggerNextMove();
        }
    }

    private void TriggerNextMove()
    {
        if (BattleManager.Instance && BattleManager.Instance.IsBattleRunning)
        {
            // Give some delay for move completion to take effect, else, inputs/AI attack can happen in next frame.
            NewMoveStarted?.Invoke(this, null);
        }
    }
}
