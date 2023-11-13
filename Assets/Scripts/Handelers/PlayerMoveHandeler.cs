using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveHandeler
{
    private List<MoveOptionView> _playerMoveOptionViews = new List<MoveOptionView>();

    public EventHandler<MoveOptionType> NextMoveSelected;

    public void Initialize(Transform optionsParentTransform)
    {
        SetupPlayerMoveOptionsViews(optionsParentTransform);
    }

    public void Reset()
    {
        _playerMoveOptionViews.Clear();
    }

    private void SetupPlayerMoveOptionsViews(Transform optionsParentTransform)
    {
        _playerMoveOptionViews.Clear();

        for (int enumIndex = 0; enumIndex < (int)MoveOptionType.COUNT; enumIndex++)
        {
            MoveOptionView moveOptionView = PrefabsDatabase.Instance.InstantiateMoveOptionView((MoveOptionType)enumIndex, optionsParentTransform);

            if (moveOptionView)
            {
                moveOptionView.Initialize((MoveOptionType)enumIndex, (MoveOptionType moveOptionType)=> {
                    if (BattleManager.Instance && BattleManager.Instance.IsBattleRunning)
                    {
                        NextMoveSelected?.Invoke(this, moveOptionType);
                    }
                });

                _playerMoveOptionViews.Add(moveOptionView);
            }
        }
    }
}
