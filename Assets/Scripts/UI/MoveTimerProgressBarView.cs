using UnityEngine;
using UnityEngine.UI;

public class MoveTimerProgressBarView : MonoBehaviour
{
    [SerializeField] private Slider _progressBarSlider;
    [SerializeField] private TMPro.TMP_Text _durationText;

    public void Initialize()
    {
        ResetProgressBar();
    }

    public void SetTimerText(int timeRemaining)
    {
        _durationText.text = string.Format("Timer: {0}s", timeRemaining);
    }

    public void SetSliderValue(float value)
    {
        _progressBarSlider.value = Mathf.Clamp01(value);
    }

    public void ResetProgressBar()
    {
        _progressBarSlider.value = 1;

        SetTimerText(GameConstants.PER_MOVE_TIME_DURATION);
    }
}
