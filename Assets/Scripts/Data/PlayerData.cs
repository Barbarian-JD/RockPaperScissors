
using Newtonsoft.Json;
using UnityEngine;

public class PlayerData : IJsonSerializable
{
    public static string SerializationKey => "PlayerData_547d424e-0e20-4494-9cc0-6c3150009747";

    public int HighScore = 0;

    public void SaveData()
    {
        string jsonStr = JsonConvert.SerializeObject(this);
        PlayerPrefs.SetString(SerializationKey, jsonStr);
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(SerializationKey))
        {
            string jsonDataStr = PlayerPrefs.GetString(PlayerData.SerializationKey);
            PlayerData serializedData = JsonConvert.DeserializeObject<PlayerData>(jsonDataStr);

            CopyFrom(serializedData);
        }
    }

    public void DeleteData()
    {
        if (PlayerPrefs.HasKey(SerializationKey))
        {
            PlayerPrefs.DeleteKey(SerializationKey);
        }
    }

    private void CopyFrom(PlayerData playerData)
    {
        this.HighScore = playerData.HighScore;
    }
}
