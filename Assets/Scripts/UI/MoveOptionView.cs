using System;
using UnityEngine;
using UnityEngine.UI;

public class MoveOptionView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMPro.TMP_Text _optionText;

    private MoveOptionType _moveOptionType;
    private Action<MoveOptionType> _onOptionClickedCallback = null;

    public void Initialize(MoveOptionType moveOptionType, Action<MoveOptionType> onOptionClickedCallback = null, bool isPlayerControlled = true)
    {
        _moveOptionType = moveOptionType;
        _onOptionClickedCallback = onOptionClickedCallback;

        if (isPlayerControlled)
        {
            // Disconnect the listener if the the option view is re-initialized somehow for the player option views.
            DisconnectButtonListener();
            ConnectButtonListener();
        }
        else
        {
            // Disable the button for AI's move view
            if (_button)
            {
                _button.enabled = false;
            }
        }

        if (_optionText)
        {
            _optionText.text = moveOptionType.ToString();
        }
    }

    private void OnDestroy()
    {
        DisconnectButtonListener();
    }

    private void ConnectButtonListener()
    {
        if (_button)
        {
            _button.onClick.AddListener(OnOptionClicked);
        }
    }

    private void DisconnectButtonListener()
    {
        if (_button)
        {
            _button.onClick.RemoveAllListeners();
        }
    }

    private void OnOptionClicked()
    {
        _onOptionClickedCallback?.Invoke(_moveOptionType);
    }
}
