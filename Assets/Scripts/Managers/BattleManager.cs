using UnityEngine;
using System;

public enum BattleEndResultType
{
    PLAYER_WIN = 0,
    PLAYER_LOSE = 1,
}

public class BattleManager : SingletonMonoBehaviour<BattleManager>
{
    [SerializeField] private Transform _playerMoveOptionViewsContainer;
    [SerializeField] private MoveOptionView _AIMoveOptionView;

    [SerializeField] private MoveTimerProgressBarView _moveTimerProgressBar;
    public MoveTimer MoveTimer;

    public BattleData CurrentBattleData { get; private set; } = null;
    public bool IsBattleRunning { get; private set; } = false;

    private PlayerMoveHandeler _playerMoveHandeler = null;
    private AIMoveHandeler _aiMoveHandeler = null;

    public EventHandler<BattleData> BattleStarted;
    public EventHandler<BattleEndResultType> BattleEnded;

    void Start()
    {
        LoadBattleData();

        ConnectListeners();

        InitializeBattle();
    }

    private void LoadBattleData()
    {
        CurrentBattleData = new BattleData();
    }

    private void DeleteBattleData()
    {
        if (CurrentBattleData != null)
        {
            CurrentBattleData = null;
        }
    }

    private void OnDestroy()
    {
        DestroyAI();
        DestroyPlayerHandeler();

        DisconnectListeners();

        MoveManager.Instance.Reset();
    }

    private void ConnectListeners()
    {
        MoveManager.Instance.MoveCompleted += OnMoveCompleted;
        MoveTimer.MoveTimerElapsed += OnMoveTimerElapsed;
    }

    private void DisconnectListeners()
    {
        MoveManager.Instance.MoveCompleted -= OnMoveCompleted;
        MoveTimer.MoveTimerElapsed -= OnMoveTimerElapsed;
    }

    private void InitializeBattle()
    {
        InitializeAIHandeler();
        InitializePlayerHandeler();

        _moveTimerProgressBar.Initialize();

        IsBattleRunning = true;

        MoveManager.Instance.Initialize(_aiMoveHandeler, _playerMoveHandeler);

        BattleStarted?.Invoke(this, CurrentBattleData);
    }

    private void InitializeAIHandeler()
    {
        _aiMoveHandeler = new AIMoveHandeler();
        _aiMoveHandeler.Initialize();

        _aiMoveHandeler.NextMoveSelected += OnNextAIMoveSelected;
    }

    private void DestroyAI()
    {
        if (_aiMoveHandeler != null)
        {
            _aiMoveHandeler.Reset();
            _aiMoveHandeler.NextMoveSelected -= OnNextAIMoveSelected;
            _aiMoveHandeler = null;
        }
    }

    private void OnNextAIMoveSelected(object sender, MoveOptionType moveOptionType)
    {
        CurrentBattleData?.UpdateLastAISelectedMove(moveOptionType);

        if (_AIMoveOptionView)
        {
            _AIMoveOptionView.Initialize(moveOptionType, onOptionClickedCallback: null, isPlayerControlled: false);
        }
    }

    private void InitializePlayerHandeler()
    {
        _playerMoveHandeler = new PlayerMoveHandeler();
        _playerMoveHandeler.Initialize(_playerMoveOptionViewsContainer);

        _playerMoveHandeler.NextMoveSelected += OnNextPlayerMoveSelected;
    }

    private void DestroyPlayerHandeler()
    {
        if (_playerMoveHandeler != null)
        {
            _playerMoveHandeler.Reset();
            _playerMoveHandeler.NextMoveSelected -= OnNextPlayerMoveSelected;
            _playerMoveHandeler = null;
        }
    }

    private void OnNextPlayerMoveSelected(object sender, MoveOptionType moveOptionType)
    {
        
    }
    

    private bool CheckIfBattleIsOver(TurnType lastTurnType, MoveOptionType moveOptionType)
    {
        if (IsBattleRunning &&
            (lastTurnType == TurnType.AI
                                || (lastTurnType == TurnType.PLAYER
                                    && CurrentBattleData != null && RulesConfig.Instance.DoesSourceBeatTarget(moveOptionType, CurrentBattleData.LastSelectedAIMoveType))))
        {
            // Increment round and keep playing
            return false;
        }
        else
        {
            //End The Battle
            return true;
        }
    }

    /// <summary>
    /// Checks the conditions and returns the battle result -- currently hardcoded to Losing because there is no win scenario.
    /// </summary>
    /// <returns></returns>
    private BattleEndResultType ConcludeBattleResults()
    {
        return BattleEndResultType.PLAYER_LOSE;
    }

    private void OnMoveCompleted(object sender, MoveData lastMoveData)
    {
        if (CheckIfBattleIsOver(lastMoveData.CurrTurnType, lastMoveData.CurrMoveOptionType))
        {
            EndTheBattle();
        }
        else
        {
            CurrentBattleData?.UpdateNumMovesPlayed();
        }
    }

    /// <summary>
    /// Ends the current battle
    /// </summary>
    private void EndTheBattle()
    {
        IsBattleRunning = false;

        BattleEndResultType battleResultType = ConcludeBattleResults();

        BattleEnded?.Invoke(this, battleResultType);

        // Update the high score for the player.
        PlayerManager.Instance.TryUpdateHighScore(CurrentBattleData.NumMovesPlayed);

        // Delete the battle data, if required.
        //DeleteBattleData();

        // Move back to main menu scene.
        CustomSceneManager.Instance.LoadMainMenuScene();
    }

    private void OnMoveTimerElapsed(object sender, EventArgs e)
    {
        EndTheBattle();
    }
}
