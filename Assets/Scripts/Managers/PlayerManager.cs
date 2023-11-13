using UnityEngine;
using Newtonsoft.Json;

public class PlayerManager : Singleton<PlayerManager>
{
    private PlayerData _playerSaveData = new PlayerData();

    public void Save()
    {
        _playerSaveData.SaveData();
    }

    public void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey(PlayerData.SerializationKey))
        {
            string jsonDataStr = PlayerPrefs.GetString(PlayerData.SerializationKey);
            _playerSaveData = JsonConvert.DeserializeObject<PlayerData>(jsonDataStr);
        }
        else
        {
            // First time game launch, setup default data which is required.
            _playerSaveData.HighScore = 0;
        }
    }

    public int GetHighScore() => _playerSaveData.HighScore;

    public void TryUpdateHighScore(int score)
    {
        if (score > _playerSaveData.HighScore)
        {
            _playerSaveData.HighScore = score;

            _playerSaveData.SaveData();
        }
    }
}
