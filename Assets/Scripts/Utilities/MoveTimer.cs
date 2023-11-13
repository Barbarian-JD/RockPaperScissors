using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTimer : MonoBehaviour
{
    [SerializeField] MoveTimerProgressBarView _progressBarView;
    private Coroutine _timerCoroutine = null;

    public static EventHandler MoveTimerElapsed;

    private void OnEnable()
    {
        ConnectListener();
    }

    private void OnDisable()
    {
        DisconnectListener();
    }

    private void ConnectListener()
    {
        MoveManager.Instance.NewMoveStarted += OnNewMoveStarted;
        MoveManager.Instance.MoveCompleted += OnMoveCompleted;
    }

    private void DisconnectListener()
    {
        MoveManager.Instance.NewMoveStarted -= OnNewMoveStarted;
        MoveManager.Instance.MoveCompleted -= OnMoveCompleted;
    }

    private void OnNewMoveStarted(object sender, EventArgs e)
    {
        if (MoveManager.Instance.CurrentTurnType == TurnType.PLAYER)
        {
            RestartTimer();
        }
    }

    private void OnMoveCompleted(object sender, MoveData moveData)
    {
        if (moveData.CurrTurnType == TurnType.PLAYER)
        {
            StopTimer();
        }
    }

    public void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }

    public void RestartTimer()
    {
        StopTimer();

        _progressBarView.ResetProgressBar();

        _timerCoroutine = StartCoroutine(StartMoveTimerAnimCoroutine());
    }

    private IEnumerator StartMoveTimerAnimCoroutine()
    {
        float elapsedTime = 0f;
        float duration = GameConstants.PER_MOVE_TIME_DURATION;

        int lastIntShown = GameConstants.PER_MOVE_TIME_DURATION;

        while (elapsedTime < duration)
        {
            // Calculate the progress value from 0 to 1
            float progress = elapsedTime / duration;

            // Update the progress bar value
            _progressBarView.SetSliderValue(1 - progress);

            // Wait for the next frame
            yield return null;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            if (GameConstants.PER_MOVE_TIME_DURATION - Mathf.FloorToInt(elapsedTime) < lastIntShown)
            {
                _progressBarView.SetTimerText(GameConstants.PER_MOVE_TIME_DURATION - Mathf.FloorToInt(elapsedTime));
                lastIntShown = GameConstants.PER_MOVE_TIME_DURATION - Mathf.FloorToInt(elapsedTime);
            }
        }

        _progressBarView.SetSliderValue(0f);

        MoveTimerElapsed?.Invoke(this, null);
    }
}
