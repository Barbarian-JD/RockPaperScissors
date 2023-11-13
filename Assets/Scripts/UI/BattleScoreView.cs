using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScoreView : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _battleScoreText;

    // Start is called before the first frame update
    void Start()
    {
        SetScoretext(0);

        BattleData.NumMovesPlayedChanged += OnScoreChanged;
    }

    private void OnDestroy()
    {
        BattleData.NumMovesPlayedChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(object sender, int updatedNumMovesPlayed)
    {
        SetScoretext(updatedNumMovesPlayed);
    }

    private void SetScoretext(int score)
    {
        if (_battleScoreText)
        {
            _battleScoreText.text = string.Format("Score: {0}", score);
        }
    }
}
