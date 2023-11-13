using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private TMPro.TMP_Text _highScoreText;

    private void Start()
    {
        SetHighScoreText();
    }

    private void OnEnable()
    {
        ConnectListeners();
    }

    private void OnDisable()
    {
        DisconnectListeners();
    }

    private void ConnectListeners()
    {
        if (_playButton)
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }
    }

    private void DisconnectListeners()
    {
        if (_playButton)
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("Play Button Clicked!");
        CustomSceneManager.Instance.LoadBattleScene();
    }

    private void SetHighScoreText()
    {
        if (_highScoreText && PlayerManager.Instance != null)
        {
            _highScoreText.text = string.Format("High Score: {0}", PlayerManager.Instance.GetHighScore());
        }
    }
}
